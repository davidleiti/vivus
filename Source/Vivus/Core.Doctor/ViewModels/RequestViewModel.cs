namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.ObjectModel;
    using Vivus.Core.ViewModels;
    using System.Windows;
    using System;
    using Vivus.Core.DataModels;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Vivus = Console;
    using Vivus.Core.Doctor.Validators;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.Security;
    using Vivus.Core.Doctor.IoC;
    using System.Threading.Tasks;
    using System.Linq;

    /// <summary>
    /// Represents a view model for the request page.
    /// </summary>

    public class RequestViewModel : BaseViewModel
    {
        #region Private Members

        private BasicEntity<string> selectedPatientName;
        private BasicEntity<string> selectedPriority;
        private int? requestThrombocytes;
        private int? requestRedCells;
        private int? requestPlasma;
        private int? requestBlood;
        private IUnitOfWork unitOfWork;
        private IApllicationViewModel<Model.Doctor> appViewModel;
        private ISecurity security;
        private RequestItemViewModel selectedItem;


        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the collection of the donation history.
        /// </summary>
        public ObservableCollection<RequestItemViewModel> Items { get; }

        public RequestItemViewModel SelectedTableItem { get; set; }

        public List<BasicEntity<string>> PatientNames { get; }

        public List<BasicEntity<string>> Priorities { get; }

        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets the register command.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Gets the register command.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Gets or sets the requestPatientName.
        /// </summary>
        public BasicEntity<string> SelectedPatientName
        {
            get => selectedPatientName;

            set
            {
                if (selectedPatientName == value)
                    return;

                selectedPatientName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestPriority.
        /// </summary>
        public BasicEntity<string> SelectedPriority
        {
            get => selectedPriority;

            set
            {
                if (selectedPriority == value)
                    return;

                selectedPriority = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestThrombocytes.
        /// </summary>
        public int? RequestThrombocytes
        {
            get => requestThrombocytes;

            set
            {
                if (requestThrombocytes == value)
                    return;

                requestThrombocytes = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestRedCells.
        /// </summary>
        public int? RequestRedCells
        {
            get => requestRedCells;

            set
            {
                if (requestRedCells == value)
                    return;

                requestRedCells = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestPlasma.
        /// </summary>
        public int? RequestPlasma
        {
            get => requestPlasma;

            set
            {
                if (requestPlasma == value)
                    return;

                requestPlasma = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requested blood.
        /// </summary>
        public int? RequestBlood
        {
            get => requestBlood;

            set
            {
                if (requestBlood == value)
                    return;

                requestBlood = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
		/// Gets or sets the selected item in the administrators table
		/// </summary>
		public RequestItemViewModel SelectedItem
        {
            get => selectedItem;

            set
            {
                if (value == selectedItem)
                    return;

                selectedItem = value;
                
                OnPropertyChanged();
            }
        }


        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(SelectedPatientName))
                    return GetErrorString(propertyName, DoctorValidator.NotNullStringFieldValidation(SelectedPatientName, "Patient"));

                if (propertyName == nameof(SelectedPriority))
                    return GetErrorString(propertyName, DoctorValidator.NotNullStringFieldValidation(SelectedPriority, "Priority"));

                if (propertyName == nameof(RequestThrombocytes))
                    return GetErrorString(propertyName, DoctorValidator.NotNullIntegerFieldValidation(RequestThrombocytes, "Thrombocytes"));

                if (propertyName == nameof(RequestRedCells))
                    return GetErrorString(propertyName, DoctorValidator.NotNullIntegerFieldValidation(RequestRedCells, "Red cells"));

                if (propertyName == nameof(RequestPlasma))
                    return GetErrorString(propertyName, DoctorValidator.NotNullIntegerFieldValidation(RequestPlasma, "Plasma"));

                if (propertyName == nameof(RequestBlood))
                    return GetErrorString(propertyName, DoctorValidator.NotNullIntegerFieldValidation(RequestPlasma, "Blood"));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestViewModel"/> class with the default values.
        /// </summary>
        public RequestViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {

            Items = new ObservableCollection<RequestItemViewModel>();
            PatientNames = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select patient") };
            Priorities = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select priority") };
            AddCommand = new RelayCommand(async () => await AddAsync());
            CancelCommand = new RelayCommand(Cancel);

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Model.Doctor>>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async () =>
            {
                await LoadPatientsAsync();
                await LoadPrioritiesAsync();
                PopulateFields();
            });

            // Test whether the binding was done right or not
            /*
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new RequestItemViewModel
                {
                    Id = 39,
                    PatientName = "Ionut",
                    Priority = "easy",
                    Thrombocytes = 1,
                    RedCells = 213,
                    Plasma = 555,
                    RequestStatus = "done",
                    Cdc = "???",
                    Dcn = "dcn"
                });

            });*/
        }


        public RequestViewModel(IUnitOfWork unitOfWork, IApllicationViewModel<Model.Doctor> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security) : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {

            Items = new ObservableCollection<RequestItemViewModel>();
            PatientNames = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select patient") };
            Priorities = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select priority") };
            AddCommand = new RelayCommand(async () => await AddAsync());
            CancelCommand = new RelayCommand(Cancel);

            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.security = security;

            Task.Run(async () =>
            {
                await LoadPatientsAsync();
                await LoadPrioritiesAsync();
                PopulateFields();
            });

            // Test whether the binding was done right or not
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new RequestItemViewModel
                {
                    Id = 39,
                    PatientName = "Ionut",
                    Priority = "easy",
                    Thrombocytes = 1,
                    RedCells = 213,
                    Plasma = 555,
                    RequestStatus = "done",
                    Cdc = "???",
                    Dcn = "dcn"
                });

            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all the patients asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadPatientsAsync()
        {
            await Task.Run(() =>
            {
                PatientNames.Clear();
                PatientNames.Add(new BasicEntity<string>(-1, "Select patient"));
                unitOfWork.Patients
                            .Entities
                            .ToList()
                            .ForEach(patient => dispatcherWrapper
                            .InvokeAsync(() => PatientNames.Add(new BasicEntity<string>(patient.PersonID,
                                                                 patient.Person.FirstName + " " + patient.Person.LastName)))
                );
            });
        }

        /// <summary>
        /// Loads all the patients asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadPrioritiesAsync()
        {
            await Task.Run(() =>
            {
                Priorities.Clear();
                Priorities.Add(new BasicEntity<string>(-1, "Select priority"));
                unitOfWork.RequestPriorities.Entities.ToList().ForEach(priority =>
                    dispatcherWrapper.InvokeAsync(() => Priorities.Add(new BasicEntity<string>(priority.RequestPriorityID, priority.Type)))
                );
            });
        }

        /// <summary>
        /// Clears all the fields of the viewmodel.
        /// </summary>
        private void ClearFields()
        {
            SelectedPatientName = new BasicEntity<string>(-1, "Select patient");
            SelectedPriority = new BasicEntity<string>(-1, "Select priority");
            RequestThrombocytes = 0;
            RequestRedCells = 0;
            RequestPlasma = 0;
            RequestBlood = 0;
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {               
            SelectedPatientName = new BasicEntity<string>(selectedItem.Id, selectedItem.PatientName);
            SelectedPriority = new BasicEntity<string>(selectedItem.Id, selectedItem.Priority);
            RequestThrombocytes = selectedItem.Thrombocytes;
            RequestRedCells = selectedItem.RedCells;
            //RequestBlood = selectedItem.Blood;
        }

        /// <summary>
        /// Fills the fields of a request.
        /// </summary>
        /// <param name="request">The request instance.</param>
        /// <param name="fillOptional">Whether to fill the optional field also or not.</param>
        private void FillModelRequest(ref Model.BloodRequest request, bool fillOptional = false)
        {
            Model.Patient patient;
            patient = unitOfWork.Patients.Find(p => p.Person.PersonID == SelectedPatientName.Id).Single();
            Model.RequestPriority priority;
            priority = unitOfWork.RequestPriorities.Find(r => r.RequestPriorityID == SelectedPriority.Id).Single();

            // If the instance is null, initialize it
            if (request is null)
                request = new Model.BloodRequest();


            // Update the database

            //request.Patient = patient;
            //request.RequestPriority = priority;
            //request.ThrombocytesQuantity = RequestThrombocytes;
            //request.RedCellsQuantity = RequestRedCells;
            //request.PlasmaQuantity = RequestPlasma;
            //request.BloodQuantity = RequestBlood;
        }

        /// <summary>
        /// Fills the fields of a request viewmodel.
        /// </summary>
        /// <param name="request">The request viewmodel instance.</param>
        private void FillViewModelRequest(ref RequestItemViewModel request)
        {
            Model.Patient patient;
            patient = unitOfWork.Patients.Find(p => p.Person.PersonID == SelectedPatientName.Id).Single();
            Model.RequestPriority priority;
            priority = unitOfWork.RequestPriorities.Find(r => r.RequestPriorityID == SelectedPriority.Id).Single();

            // If the instance is null, initialize it
            if (request is null)
                request = new RequestItemViewModel();

            request.PatientName = patient.Person.FirstName +" "+ patient.Person.LastName;
            request.Priority = priority.ToString();
            request.Thrombocytes = RequestThrombocytes;
            request.RedCells = RequestRedCells;
            request.Plasma = RequestPlasma;
            request.Blood = RequestBlood;
        }


        /// <summary>
        /// Add.
        /// </summary>
        private async Task AddAsync()
        {

            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                try
                {
                    Model.BloodRequest request;
                    RequestItemViewModel requestViewModel;

                    request = null;
                    requestViewModel = null;

                    FillModelRequest(ref request, true);
                    unitOfWork.BloodRequests.Add(request);
                    // Make changes persistent
                    unitOfWork.Complete();

                    // Update the table
                    FillViewModelRequest(ref requestViewModel);
                    requestViewModel.Id = request.PatientID;

                    dispatcherWrapper.InvokeAsync(() => Items.Add(requestViewModel));

                    Popup($"Request added successfully!", PopupType.Successful);
                }
                catch
                {
                    Popup($"An error occured while adding the request.");
                }
            });
            /*
            ParentPage.AllowErrors();

            if(Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("Doctor: Add works!");
            Popup("Add works!", PopupType.Successful);*/
        }

        /// <summary>
        /// Cancel.
        /// </summary>
        private void Cancel()
        {
            /*ParentPage.AllowErrors();

            if (Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }*/

            Vivus.Console.WriteLine("Doctor: Cancel works!");
            Popup("Cancel works!", PopupType.Successful);
        }

        #endregion
    }

    /// <summary>
    /// Represents an item view model for the requests table.
    /// </summary>

    public class RequestItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private string patientName;
        private string priority;
        private int? thrombocytes;
        private int? redCells;
        private int? plasma;
        private int? blood;
        private string requestStatus;
        private string cdc;
        private string dcn;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identificator.
        /// </summary>
        public int Id
        {
            get => id;

            set
            {
                if (id == value)
                    return;

                id = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the patientName.
        /// </summary>
        public string PatientName
        {
            get => patientName;

            set
            {
                if (patientName == value)
                    return;

                patientName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public string Priority
        {
            get => priority;

            set
            {
                if (priority == value)
                    return;

                priority = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the thrombocytes.
        /// </summary>
        public int? Thrombocytes
        {
            get => thrombocytes;

            set
            {
                if (thrombocytes == value)
                    return;

                thrombocytes = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the red cells.
        /// </summary>
        public int? RedCells
        {
            get => redCells;

            set
            {
                if (redCells == value)
                    return;

                redCells = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the plasma.
        /// </summary>
        public int? Plasma
        {
            get => plasma;

            set
            {
                if (plasma == value)
                    return;

                plasma = value;

                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the blood.
        /// </summary>
        public int? Blood
        {
            get => blood;

            set
            {
                if (blood == value)
                    return;

                blood = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestStatus.
        /// </summary>
        public string RequestStatus
        {
            get => requestStatus;

            set
            {
                if (requestStatus == value)
                    return;

                requestStatus = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cdc.
        /// </summary>
        public string Cdc
        {
            get => cdc;

            set
            {
                if (cdc == value)
                    return;

                cdc = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the patientName.
        /// </summary>
        public string Dcn
        {
            get => dcn;

            set
            {
                if (dcn == value)
                    return;

                dcn = value;

                OnPropertyChanged();
            }
        }

        #endregion

    }

}
