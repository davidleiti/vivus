namespace Vivus.Core.DCPersonnel.ViewModels {

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
    using System.Linq;
	using System.Windows;
	using System.Windows.Input;
    using System.Threading.Tasks;

	using Vivus.Core.DataModels;
	using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
	using Vivus.Core.ViewModels.Notifications;
    using Vivus.Core.UoW;
    using Vivus.Core.DCPersonnel.IoC;
    using Vivus.Core.Model;

	using Vivus = Console;

	/// <summary>
	/// Represents a view model for the notifications page.
	/// </summary>
	public class NotificationsViewModel : BaseViewModel {

		#region Private fields
		private string nin;
		private BasicEntity<string> personType;
		private BasicEntity<string> personName;
		private string message;

        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Model.DCPersonnel> appViewModel;

        private bool sendingIsRunning;

        private int lastMessageId;
		#endregion

		#region Public Properties

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

		/// <summary>
		/// Gets gets the list of person types (Donor, Doctor, or DCPersonnel)
		/// </summary>
		public List<BasicEntity<string>> PersonTypes { get; }

		/// <summary>
		/// Gets or sets the type of the selected person 
		/// </summary>
		public BasicEntity<string> PersonType {
			get => personType;
			set {
				if (value == personType)
					return;

				personType = value;

				OnPropertyChanged();

                LoadPersons();
			}
		}

		/// <summary>
		/// Gets gets the list of person names
		/// </summary>
		public ObservableCollection<BasicEntity<string>> Persons { get; }

		/// <summary>
		/// Gets or sets the name of the selected person 
		/// </summary>
		public BasicEntity<string> PersonName {
			get => personName;
			set {
				if (value == personName)
					return;

				personName = value;
				OnPropertyChanged();

                LoadNin();
			}
		}

		/// <summary>
		/// Gets or sets the message of the notification
		/// </summary>
		public string Message {
			get => message;
			set {
				if (value == message)
					return;

				message = value;

				OnPropertyChanged();

                /*
                 * Used to check fields values when validated.
                Console.WriteLine(PersonType is null);
                Console.WriteLine(PersonName is null);
                Console.WriteLine(string.IsNullOrEmpty(Message));

                Console.WriteLine(PersonType.Id);
                Console.WriteLine(PersonName.Id);
                */
            }
		}

		/// <summary>
		/// Gets or sets the National Identification Number of the person
		/// </summary>
		public string NationalIdentificationNumber {
			get => nin;
			set {
				if (value == nin)
					return;

				nin = value;

				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets the list of notifications 
		/// </summary>
		public ObservableCollection<NotificationViewModel> Items { get; }

		/// <summary>
		/// Gets the error string of a property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns></returns>
		public override string this[string propertyName] {
			get {
				if (propertyName == nameof(PersonType))
                {
                    if (PersonType is null || PersonType.Id < 0)
                        return GetErrorString(propertyName, new List<string> { "Person type field is mandatory." });
                    else
                        return GetErrorString(propertyName, null);
				}
				if (propertyName == nameof(PersonName))
                {
                    if (PersonName is null || PersonName.Id < 0)
                        return GetErrorString(propertyName, new List<string> { "Person name field is mandatory." });
                    else
                        return GetErrorString(propertyName, null);
				}
                if (propertyName == nameof(Message))
                {
                    if (string.IsNullOrEmpty(Message))
                        return GetErrorString(propertyName, new List<string> { "Message field is mandatory." });
                    else
                        return GetErrorString(propertyName, null);
                }

                return null;
			}
		}

		/// <summary>
		/// Gets the send notification command.
		/// </summary>
		public ICommand SendCommand { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NotificationsViewModel"/> class with the default values.
		/// </summary>
		public NotificationsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
			PersonTypes = new List<BasicEntity<string>>();
			PersonTypes.Add(new BasicEntity<string>(-1, "Select person type"));
			PersonTypes.Add(new BasicEntity<string>(0, "Donors"));
			PersonTypes.Add(new BasicEntity<string>(1, "Doctors"));
			PersonTypes.Add(new BasicEntity<string>(2, "DC Personnel"));

			Persons = new ObservableCollection<BasicEntity<string>>();
			Persons.Add(new BasicEntity<string>(-1, "Select person name"));

			Items = new ObservableCollection<NotificationViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<DCPersonnel>>();

			SendCommand = new RelayCommand(async() => await SendAsync());

            lastMessageId = -1;

            UpdateNotifications();
		}

		#endregion

		/// <summary>
		/// Sends a notification
		/// </summary>
		#region Private Methods
		private async Task SendAsync() {
            await RunCommand(() => SendingIsRunning, async() =>
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
                        message.RecieverID = PersonName.Id;
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

        private async void LoadPersons()
        {
            Console.WriteLine("Selected " + PersonType);

            Console.WriteLine("Clearing fields.");
            Persons.Clear();
            Persons.Add(new BasicEntity<string>(-1, "Select person name"));
            PersonName = Persons[0];
            NationalIdentificationNumber = null;

            if (PersonType.Value.Equals("Doctors"))
                await LoadDoctors();

            if (PersonType.Value.Equals("DC Personnel"))
                await LoadDCPersonnel();
        }

        private async Task LoadDoctors()
        {
            await Task.Run(() =>
            {
                unitOfWork.Doctors.Entities.ToList().ForEach(doctor =>
                dispatcherWrapper.InvokeAsync(() => Persons.Add(new BasicEntity<string>(doctor.PersonID, doctor.Person.FirstName + " " + doctor.Person.LastName))));
            });
        }

        private async Task LoadDCPersonnel()
        {
            await Task.Run(() => 
            {
                unitOfWork.DCPersonnel.Entities.ToList().ForEach(dcp => 
                {
                    dispatcherWrapper.InvokeAsync(() =>
                    {
                        if (dcp.PersonID != appViewModel.User.PersonID)
                            Persons.Add(new BasicEntity<string>(dcp.PersonID, dcp.Person.FirstName + " " + dcp.Person.LastName));
                    });
                });
            });
        }

        private async Task LoadDonors()
        {
            await Task.Run(() => 
            {
                unitOfWork.Donors.Entities.ToList().ForEach(donor => 
                {
                    dispatcherWrapper.InvokeAsync(() => 
                    {
                        if (donor.DonationCenterID == appViewModel.User.DonationCenterID)
                            Persons.Add(new BasicEntity<string>(donor.PersonID, donor.Person.FirstName + " " + donor.Person.LastName));
                    });
                });
            });
        }

        private async void LoadNin()
        {
            Console.WriteLine("Loading Nin");
            await LoadPersonNin();
        }

        private async Task LoadPersonNin()
        {
            await Task.Run(() => 
            {
                try
                {
                    NationalIdentificationNumber = unitOfWork.Persons.Entities.ToList().Find(p =>
                    {
                        return (p.FirstName + " " + p.LastName) == PersonName.Value;
                    }).Nin;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Nin could not be loaded. Person name not found.");
                }
            });
        }

        private async void UpdateNotifications()
        {
            while (true)
            {
                await LoadNotifications();
                await Task.Delay(5000);
            }
        }

        private async Task LoadNotifications()
        {
            await Task.Run(() =>
            {
                DCPersonnel dcPersonnel = unitOfWork.Persons[appViewModel.User.PersonID].DCPersonnel;
                unitOfWork.CompleteAsync();
                List<Message> messages = new List<Message>();

                foreach (Message m in unitOfWork.Messages.Entities.Where(m => m.RecieverID == dcPersonnel.PersonID))
                {
                    if (m.MessageID > lastMessageId)
                        messages.Add(m);
                }

                messages.Sort((m1, m2) =>
                {
                    if (m1.SendDate > m2.SendDate)
                        return -1;

                    if (m1.SendDate < m2.SendDate)
                        return 1;

                    return 0;
                });

                if (messages.Count > 0 && messages[0].MessageID > lastMessageId)
                    lastMessageId = messages[0].MessageID;

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
		#endregion

	}
}
