namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.Doctor.Validators;
    using Vivus.Core.Model;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.ViewModels.Notifications;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the notifications page.
    /// </summary>
    public class NotificationsViewModel : BaseViewModel
    {

        #region Private Fields
        private string nin;
        private string message;
        private BasicEntity<string> selectedPersonName;
        private BasicEntity<string> selectedPersonType;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Doctor> appViewModel;
        private bool sendingIsRunning;
        private IDictionary<int, string> nins;
        #endregion

        #region Public Members
        public IPage ParentPage { get; set; }

        public bool SendingIsRunning
        {
            get => sendingIsRunning;
            set
            {
                if (sendingIsRunning == value)
                    return;

                sendingIsRunning = value;

                OnPropertyChanged();
            }
        }
        public List<BasicEntity<string>> PersonTypes { get; }
        public BasicEntity<string> SelectedPersonType
        {
            get => selectedPersonType;
            set
            {
                if (selectedPersonType == value)
                    return;

                selectedPersonType = value;

                OnPropertyChanged();
            }
        }


        public List<BasicEntity<string>> PersonNames { get; }
        public BasicEntity<string> SelectedPersonName
        {
            get => selectedPersonName;
            set
            {
                if (selectedPersonName == value)
                    return;

                // if (selectedPersonName is null)
                //   return;

                selectedPersonName = value;
                Console.WriteLine(selectedPersonName + " should be in the dict");

                if (selectedPersonName != null&&selectedPersonName.Id>0)
                    NationalIdentificationNumber = nins[selectedPersonName.Id];

                OnPropertyChanged();
            }

        }

        public string NationalIdentificationNumber
        {
            get => nin;

            set
            {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => message;

            set
            {
                if (message == value)
                    return;

                message = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the send command.
        /// </summary>
        public ICommand SendCommand { get; }


        /// <summary>
        /// Gets the collection of the notifications.
        /// </summary>
        public ObservableCollection<NotificationViewModel> Items { get; }

        /// <summary>
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(SelectedPersonType))
                    return GetErrorString(propertyName, DoctorValidator.PersonTypeValidation(SelectedPersonType));

                if (propertyName == nameof(SelectedPersonName))
                    return GetErrorString(propertyName, DoctorValidator.PersonNameValidation(SelectedPersonName));

                if (propertyName == nameof(Message))
                    return GetErrorString(propertyName, DoctorValidator.MessageValidation(Message));

                return null;
            }
        }
        #endregion

        #region Constructors
        public NotificationsViewModel():base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<Doctor>>();
            nins = new Dictionary<int, string>();
            //nins.Add(-1, "Select person type");
            //SelectedPersonType = new BasicEntity<string>(-1, "Select person type");
            PersonTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select person type"), new BasicEntity<string>(0, "DC Personnel") };
            //SelectedPersonName = new BasicEntity<string>(-1, "Select person name");
            PersonNames = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select person name") };
            SendCommand = new RelayCommand(async () => await SendAsync());

            Items = new ObservableCollection<NotificationViewModel>();
            // Test whether the binding was done right or not
            LoadPersonNamesAsync();
            UpdateNotifications();
            /* Application.Current.Dispatcher.Invoke(() =>
             {
                 Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "AP", "andreipopescu", new DateTime(2018, 4, 28), "This is dă message."));
             });*/

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Send a message.
        /// </summary>
        private async Task SendAsync()
        {
            await RunCommand(() => SendingIsRunning, async () =>
            {
                await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors > 0)
                {
                    Console.WriteLine(Error);

                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                await Task.Run(() =>
                {
                    try
                    {
                        Message message = new Message();
                        message.SenderID = appViewModel.User.PersonID;
                        message.RecieverID = SelectedPersonName.Id;
                        message.Content = Message;
                        message.SendDate = DateTime.Today;

                        unitOfWork.Messages.Add(message);
                        unitOfWork.Complete();

                        Popup("Message sent successfuly!", PopupType.Successful);
                    }
                    catch
                    {
                        Popup("An error occured while sending the message.", PopupType.Error);
                    }
                });

            });

        }

        /// <summary>
        /// loads the person names for the dcpersonnel
        /// </summary>
        private async void LoadPersonNamesAsync()
        {
            await Task.Run(() =>
            {
                PersonNames.Clear();
                PersonNames.Add(new BasicEntity<string>(-1, "Select person name"));
                unitOfWork.DCPersonnel
                .Entities
                .ToList()
                .ForEach(dcPersonnel =>
                {
                    Person person = unitOfWork.Persons.Find(p => p.PersonID == dcPersonnel.PersonID).Single();
                    nins.Add(person.PersonID, person.Nin);
                    Console.WriteLine(person.PersonID + " " + person.Nin + " are at load");
                    PersonNames.Add(new BasicEntity<string>(person.PersonID, person.FirstName + " " + person.LastName));

                });
            });
        }
        /// <summary>
        /// clears the fields of the viewmodel
        /// </summary>
        private void ClearFields()
        {
            SelectedPersonType = PersonTypes[0];
            NationalIdentificationNumber = string.Empty;
            Message = string.Empty;
            selectedPersonName = PersonNames[0];

        }



        /// <summary>
        /// loads the notifications for the user
        /// </summary>
        private async Task LoadNotifications()
        {
            await Task.Run(() =>
            {
                
                Doctor doctor = null;
                doctor=unitOfWork.Persons[appViewModel.User.PersonID].Doctor;
                unitOfWork.CompleteAsync();
                List<Message> messages = unitOfWork.Messages.Entities.Where(m => m.RecieverID == doctor.PersonID).ToList();
                                        

                messages.Sort((m1, m2) =>
                {
                    if (m1.SendDate > m2.SendDate)
                        return -1;

                    if (m1.SendDate < m2.SendDate)
                        return 1;

                    return 0;
                });

                messages.ForEach(m =>
                {
                    char initial1 = m.Sender.FirstName[0];
                    char initial2 = m.Sender.LastName[0];
                    String initials = "" + initial1 + initial2;
                    dispatcherWrapper.InvokeAsync(() => Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), initials, FormatSenderName(m.Sender), m.SendDate, m.Content)));
                });
                string FormatSenderName(Person sender)
                {
                    return $"{ sender.FirstName.Replace("-", string.Empty).Replace(" ", string.Empty).ToLower() }.{ sender.LastName.Replace("-", string.Empty).Replace(" ", string.Empty).ToLower() }";
                }
            });
        }

        /// <summary>
        /// updates the notifications in every five seconds
        /// </summary>
        private async void UpdateNotifications()
        {
            while (true)
            {
                Items.Clear();
                await LoadNotifications();
                await Task.Delay(5000);
            }
        }

        #endregion
    }
}
