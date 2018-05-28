namespace Vivus.Core.DCPersonnel.ViewModels
{
    using Vivus.Core.ViewModels;
    using Vivus.Core.DataModels;
    using System;
    using System.Windows.Input;
    using System.Collections.ObjectModel;
    using Vivus.Core.DCPersonnel.Validators;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.Security;
    using Vivus.Core.Model;
    using System.Windows;
    using Vivus.Core.DCPersonnel.IoC;
    using System.Threading.Tasks;
    using System.Linq;

    /// <summary>
    /// Represents a view model for the blood donation requests page.
    /// </summary>

    public class BloodDonationRequestsViewModel : BaseViewModel
    {
        #region Private Members
        private string fullName;
        private string nationalIdentificationNumber;
        private int? age;
        private int? weight;
        private int? heartRate;
        private int? systolicBP;
        private int? diastolicBP;
        private DateTime? applyDate;
        private string pastSurgeries;
        private string travelStatus;
        private string messages;
        private bool actionIsRunning;
        private BloodDonationRequestItem selectedBloodDonationRequestItem;
        private ObservableCollection<BloodDonationRequestItem> bloodDonationRequestItems;

        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<DCPersonnel> appViewModel;
        private ISecurity security;

        #endregion

        #region Public Properties
        public IPage Parentpage { get; set; }
        public string FullName
        {
            get => fullName;
            set
            {
                if (fullName == value)

                    return;

                fullName = value;

                OnPropertyChanged();

            }

        }
        public string NationalIdentificationNumber
        {
            get => nationalIdentificationNumber;
            set
            {
                if (nationalIdentificationNumber == value)

                    return;

                nationalIdentificationNumber = value;

                OnPropertyChanged();

            }
        }
        public int? Age
        {
            get => age;
            set
            {
                if (age == value)

                    return;

                age = value;

                OnPropertyChanged();

            }

        }
        public int? Weight
        {
            get => weight;
            set
            {
                if (weight == value)

                    return;

                weight = value;

                OnPropertyChanged();
            }

        }
        public int? HeartRate
        {
            get => heartRate;

            set
            {
                if (heartRate == value)

                    return;

                heartRate = value;

                OnPropertyChanged();


            }
        }
        public int? SystolicBP
        {
            get => systolicBP;

            set
            {
                if (systolicBP == value)

                    return;

                systolicBP = value;

                OnPropertyChanged();
            }
        }
        public int? DiastolicBP
        {
            get => diastolicBP;

            set
            {
                if (diastolicBP == value)

                    return;

                diastolicBP = value;

                OnPropertyChanged();
            }
        }
        public DateTime? ApplyDate
        {
            get => applyDate;

            set
            {
                if (applyDate == value)

                    return;

                applyDate = value;

                OnPropertyChanged();

            }

        }

        public string PastSurgeries
        {

            get => pastSurgeries;

            set
            {
                if (pastSurgeries == value)

                    return;

                pastSurgeries = value;

                OnPropertyChanged();
            }
        }
        public string TravelStatus
        {
            get => travelStatus;

            set
            {
                if (travelStatus == value)

                    return;

                travelStatus = value;

                OnPropertyChanged();

            }

        }
        public string Messages
        {
            get => messages;
            set
            {
                if (messages == value)

                    return;

                messages = value;

                OnPropertyChanged();
            }
        }

        public BloodDonationRequestItem SelectedBloodDonationRequestItem
        {
            get => selectedBloodDonationRequestItem;
            set
            {
                if (selectedBloodDonationRequestItem == value)

                    return;

                selectedBloodDonationRequestItem = value;

                if (selectedBloodDonationRequestItem is null)
                {
                    dispatcherWrapper.InvokeAsync(() => Parentpage.DontAllowErrors());

                    ClearFields();
                }
                else
                {
                    dispatcherWrapper.InvokeAsync(() => Parentpage.AllowOptionalErrors());

                    PopulateFields();
                }

                OnPropertyChanged();
            }
        }
        public ObservableCollection<BloodDonationRequestItem> BloodDonationRequestItems
        {
            get => bloodDonationRequestItems;
            set
            {
                if (bloodDonationRequestItems == value)

                    return;

                bloodDonationRequestItems = value;

                OnPropertyChanged();

            }

        }

        public bool ActionIsRunning {
            get => actionIsRunning;

            set {
                if (actionIsRunning == value)
                    return;

                actionIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
		/// Gets the error string of a property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Messages))
                    return GetErrorString(propertyName, DCPersonnelValidator.MessagesValidation(Messages));
                return null;
            }
        }
        
        public ICommand ApproveCommand { get; }
        public ICommand DenyCommand { get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BloodDonationRequestsViewModel"/> class with the default values.
        /// </summary>
        public BloodDonationRequestsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<DCPersonnel>>();
            security = IoCContainer.Get<ISecurity>();

            ApproveCommand = new RelayCommand(() => ApproveOrRejectDonation(true));
            DenyCommand = new RelayCommand(() => ApproveOrRejectDonation(false));
            BloodDonationRequestItems = new ObservableCollection<BloodDonationRequestItem>();
            LoadRequestsAsync();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloodDonationRequestsViewModel"/> class with the given values.
        /// </summary>
        /// <param name="unitOfWork">The UoW used to access repositories.</param>
        /// <param name="appViewModel">The viewmodel for the application.</param>
        /// <param name="dispatcherWrapper">The ui thread dispatcher.</param>
        /// <param name="security">The collection of security methods.</param>
        public BloodDonationRequestsViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<DCPersonnel> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.dispatcherWrapper = dispatcherWrapper;
            this.security = security;
            
            BloodDonationRequestItems = new ObservableCollection<BloodDonationRequestItem>();
            ApproveCommand = new RelayCommand(() => ApproveOrRejectDonation(true));
            DenyCommand = new RelayCommand(() => ApproveOrRejectDonation(false));
            LoadRequestsAsync();
        }
        #endregion

        #region Public Methods

        public async void ApproveOrRejectDonation(bool decision)
        {
            await RunCommand(() => ActionIsRunning, async () => {
                await ApproveOrRejectDonationAsync(decision);
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles the selected blood donation request
        /// </summary>
        /// <param name="decision">Boolean representing whether the request has been approved or rejected</param>
        private async Task ApproveOrRejectDonationAsync(bool decision)
        {
            await Task.Run(() =>
            {
                if (SelectedBloodDonationRequestItem is null)
                {
                    Popup("No donation request has been selected. Select one from the list and try again.");
                    return;
                }

                dispatcherWrapper.InvokeAsync(() => Parentpage.AllowErrors());

                if (Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                try
                {
                    DonationForm form = unitOfWork.DonationForms
                        .Entities
                        .First(f => f.DonationFormID == selectedBloodDonationRequestItem.Id);
                    form.DCPersonnelID = appViewModel.User.PersonID;
                    form.DonationStatus = decision;

                    string messageContent = decision ? "Donation request has been approved!" : "Donation request has been rejected!";
                    messageContent += "\n" + Messages;

                    Message message = new Message
                    {
                        RecieverID = form.DonorID,
                        SenderID = appViewModel.User.PersonID,
                        SendDate = DateTime.Now,
                        Content = messageContent
                    };

                    unitOfWork.Persons
                        .Entities
                        .First(p => p.PersonID == form.DonorID)
                        .ReceivedMessages
                        .Add(message);

                    unitOfWork.Complete();

                    LoadRequestsAsync();
                    SelectedBloodDonationRequestItem = null;
                    ClearFields();
                    Popup("Request handled successfully!", PopupType.Successful);
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
            });
        }

        /// <summary>
        /// Loads all pending blood donation requests (not approved or rejected yet) in the table
        /// </summary>
        private async void LoadRequestsAsync()
        {
            await Task.Run(() =>
            {
                BloodDonationRequestItems = new ObservableCollection<BloodDonationRequestItem>();
                unitOfWork.DonationForms
                    .Entities
                    .Where(request => request.DonationStatus is null)
                    .ToList()
                    .ForEach(request =>
                        dispatcherWrapper.InvokeAsync(() => 
                        {
                            BloodDonationRequestItems.Add(new BloodDonationRequestItem
                            {
                                Id = request.DonationFormID,
                                FullName = request.Donor.Person.FirstName + " " + request.Donor.Person.LastName,
                                Age = DateTime.Today.Year - request.Donor.Person.BirthDate.Year,
                                NationalIdentificationNumber = request.Donor.Person.Nin,
                                Weight = request.Weight,
                                HeartRate = request.HeartRate,
                                SystolicBP = request.SystolicBloodPressure,
                                DiastolicBP = request.DiastolicBloodPressure,
                                ApplyDate = request.ApplyDate,
                                PastSurgeries = request.PastSurgeries,
                                TravelStatus = request.TravelStatus
                            });
                        })
                    );
            });
        }

        /// <summary>
        /// Fills the readonly fields of the form with the data of the selected request
        /// </summary>
        private void PopulateFields()
        {
            FullName = selectedBloodDonationRequestItem.FullName;
            NationalIdentificationNumber = selectedBloodDonationRequestItem.NationalIdentificationNumber;
            Age = selectedBloodDonationRequestItem.Age;
            Weight = selectedBloodDonationRequestItem.Weight;
            HeartRate = selectedBloodDonationRequestItem.HeartRate;
            SystolicBP = selectedBloodDonationRequestItem.SystolicBP;
            DiastolicBP = selectedBloodDonationRequestItem.DiastolicBP;
            ApplyDate = selectedBloodDonationRequestItem.ApplyDate;
            PastSurgeries = selectedBloodDonationRequestItem.PastSurgeries;
            TravelStatus = selectedBloodDonationRequestItem.TravelStatus;
        }

        /// <summary>
        /// Clears the fields of the form
        /// </summary>
        private void ClearFields()
        {
            FullName = string.Empty;
            NationalIdentificationNumber = string.Empty;
            Age = null;
            Weight = null;
            HeartRate = null;
            SystolicBP = null;
            DiastolicBP = null;
            ApplyDate = null;
            PastSurgeries = string.Empty;
            TravelStatus = string.Empty;
            Messages = string.Empty;
        }
        #endregion

    }

    /// <summary>
    /// Class used for representing the data of a DonationForm entity
    /// </summary>
    public class BloodDonationRequestItem : BaseViewModel
    {
        #region Private members
        private int id;
        private DateTime applyDate;
        private string nationalIdentificationNumber;
        private string fullName;
        private int age;
        private int weight;
        private int heartRate;
        private int? systolicBP;
        private int? diastolicBP;
        private string pastSurgeries;
        private string travelStatus;
        #endregion

        #region Public Properties
        public int Id
        {
            get => id;
            set
            {
                if (value == id)

                    return;

                id = value;

                OnPropertyChanged();
            }
        }
        public string FullName
        {
            get => fullName;

            set
            {
                if (fullName == value) return;
                fullName = value;
                OnPropertyChanged();
            }
        }
        public string NationalIdentificationNumber
        {
            get => nationalIdentificationNumber;

            set
            {
                if (nationalIdentificationNumber == value)
                    return;

                nationalIdentificationNumber = value;

                OnPropertyChanged();
            }
        }
        public int Age
        {
            get => age;

            set
            {
                if (age == value)
                    return;

                age = value;

                OnPropertyChanged();
            }
        }
        public DateTime ApplyDate
        {
            get => applyDate;

            set
            {
                if (applyDate == value)
                    return;

                applyDate = value;

                OnPropertyChanged();
            }
        }

        public int Weight {
            get => weight;
            set {
                if (weight == value)

                    return;

                weight = value;

                OnPropertyChanged();
            }

        }
        public int HeartRate {
            get => heartRate;

            set {
                if (heartRate == value)

                    return;

                heartRate = value;

                OnPropertyChanged();


            }
        }
        public int? SystolicBP {
            get => systolicBP;

            set {
                if (systolicBP == value)

                    return;

                systolicBP = value;

                OnPropertyChanged();
            }
        }
        public int? DiastolicBP {
            get => diastolicBP;

            set {
                if (diastolicBP == value)

                    return;

                diastolicBP = value;

                OnPropertyChanged();
            }
        }

        public string PastSurgeries {

            get => pastSurgeries;

            set {
                if (pastSurgeries == value)

                    return;

                pastSurgeries = value;

                OnPropertyChanged();
            }
        }
        public string TravelStatus {
            get => travelStatus;

            set {
                if (travelStatus == value)

                    return;

                travelStatus = value;

                OnPropertyChanged();

            }

        }

        //public override string this[string propertyName]
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}
        #endregion
    }
}
