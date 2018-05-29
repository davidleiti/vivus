namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.ObjectModel;
    using Vivus.Core.ViewModels;
    using System.Windows;
    using System;
    using Vivus.Core.DataModels;
    using System.Collections.Generic;
    using System.Windows.Input;
    using VivusConsole = Core.Console.Console;
    using Vivus.Core.Doctor.Validators;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.Security;
    using Vivus.Core.Doctor.IoC;
    using System.Threading.Tasks;
    using System.Linq;
    using Vivus.Core.Helpers;

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
        private IApplicationViewModel<Model.Doctor> appViewModel;
        private ISecurity security;
        List<DistanceMatrixApiHelpers.RouteDetails> routes;


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
            appViewModel = IoCContainer.Get<IApplicationViewModel<Model.Doctor>>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async () =>
            {
                await LoadPatientsAsync();
                await LoadPrioritiesAsync();
                await LoadDonationCentersAsync();
                await LoadRequestsAsync();
                PopulateFields();
            });

            //UpdateRequests();

        }


        public RequestViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<Model.Doctor> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security) : base(new DispatcherWrapper(Application.Current.Dispatcher))
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

        #endregion



        #region Private Methods

        /// <summary>
        /// Updates the table of requests.
        /// </summary>
        /// <returns></returns>
        private async void UpdateRequests()
        {
            while (true)
            {
                Items.Clear();
                await LoadRequestsAsync();
                await Task.Delay(15000);
            }
        }

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
            SelectedPatientName = new BasicEntity<string>(SelectedTableItem.Id, SelectedTableItem.PatientName);
            SelectedPriority = new BasicEntity<string>(SelectedTableItem.Id, SelectedTableItem.Priority);
            List<string> thrombocytesList = SelectedTableItem.Thrombocytes.Split('/').ToList();
            RequestThrombocytes = Int32.Parse(thrombocytesList[0]);
            List<string> redCellsList = SelectedTableItem.RedCells.Split('/').ToList();
            RequestRedCells = Int32.Parse(redCellsList[0]);
            List<string> plasmaList = SelectedTableItem.Plasma.Split('/').ToList();
            RequestPlasma = Int32.Parse(plasmaList[0]);
            List<string> bloodList = SelectedTableItem.Blood.Split('/').ToList();
            RequestBlood = Int32.Parse(plasmaList[0]);
        }

        /// <summary>
        /// Loads all the donation centers asynchronously sorted by their location.
        /// </summary>
        /// <returns></returns>
        private async Task LoadDonationCentersAsync()
        {
            Model.Doctor doctor;
            Model.Address originAddress;
          
            // Get the doctor
            doctor = unitOfWork.Persons[IoCContainer.Get<IApplicationViewModel<Model.Doctor>>().User.PersonID].Doctor;
            // Get doctor's work address
            originAddress = unitOfWork.Addresses.Find(a => a.AddressID == doctor.WorkAddressID).Single();
            
            // Clear the doctor
            doctor = null;
            // Get the routes
            routes = (await DistanceMatrixApiHelpers.GetDistancesAsync(originAddress, unitOfWork.DonationCenters.Entities.Select(dc => dc.Address))).ToList();
            // Sort them by the distance ascending
            routes.Sort((r1, r2) =>
            {
                if (r1.Distance < r2.Distance)
                    return -1;

                if (r1.Distance > r2.Distance)
                    return 1;

                if (r1.Duration < r2.Duration)
                    return -1;

                if (r1.Duration > r2.Duration)
                    return 1;

                return 0;
            });

        }

        

        /// <summary>
        /// Loads all the requests asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadRequestsAsync()
        {
            await Task.Run(() =>
                unitOfWork.BloodRequests
                        .Entities
                        .Where(request => request.Doctor.PersonID == appViewModel.User.PersonID)
                        .ToList()
                        .ForEach(request =>
                            dispatcherWrapper.InvokeAsync(() =>
                            {
                                int Id = request.BloodRequestID;
                                string PatientName = request.Patient.Person.FirstName + " " + request.Patient.Person.LastName;
                                string Priority = request.RequestPriority.Type;
                                List<Model.BloodContainer> bloodContainers = request.BloodContainers.ToList();
                                string Trombocytes;
                                string RedCells;
                                string Plasma;
                                string Blood;
                                string RequestStatus;
                                string Cdc;
                                int? Dcn;

                                if (bloodContainers == null || bloodContainers.Count == 0)
                                {
                                    Trombocytes = "0" + "/" + request.ThrombocytesQuantity;
                                    RedCells = "0" + "/" + request.RedCellsQuantity;
                                    Plasma = "0" + "/" + request.PlasmaQuantity;
                                    Blood = "0" + "/" + request.BloodQuantity;
                                }
                                else
                                {
                                    Trombocytes = bloodContainers.Where(bc => bc.BloodRequestID == request.BloodRequestID).ToList()
                                                        .Where(bc => bc.BloodContainerType.Type == "Trombocytes").Count().ToString()
                                                        + "/" + request.ThrombocytesQuantity;
                                    RedCells = bloodContainers.Where(bc => bc.BloodRequestID == request.BloodRequestID).ToList()
                                                        .Where(bc => bc.BloodContainerType.Type == "Red cells").Count().ToString()
                                                        + "/" + request.RedCellsQuantity;
                                    Plasma = bloodContainers.Where(bc => bc.BloodRequestID == request.BloodRequestID).ToList()
                                                        .Where(bc => bc.BloodContainerType.Type == "Plasma").Count().ToString()
                                                        + "/" + request.PlasmaQuantity;
                                    Blood = bloodContainers.Where(bc => bc.BloodRequestID == request.BloodRequestID).ToList()
                                                    .Where(bc => bc.BloodContainerType.Type == "Blood").Count().ToString()
                                                    + "/" + request.BloodQuantity;
                                }

                                if (request.IsFinished == true)
                                    RequestStatus = "Done";
                                else
                                    RequestStatus = "Pending";

                                if (request.DonationCenters.ToList() == null || request.DonationCenters.ToList().Count == 0)
                                    Cdc = routes[0].DestinationAddress.DonationCenters.ToList()[0].Name;
                                else
                                {
                                    Cdc = request.DonationCenters.ToList().Last().Name;
                                }
                                Dcn = request.DonationCenters.Count;

                                RequestItemViewModel requestItem = new RequestItemViewModel();
                                requestItem.Id = Id;
                                requestItem.PatientName = PatientName;
                                requestItem.Priority = Priority;
                                requestItem.Thrombocytes = Trombocytes;
                                requestItem.RedCells = RedCells;
                                requestItem.Plasma = Plasma;
                                requestItem.Blood = Blood;
                                requestItem.RequestStatus = RequestStatus;
                                requestItem.Cdc = Cdc;
                                requestItem.Dcn = Dcn;
                                Items.Add(requestItem);
                                
                             }
                            )
                        )
            );
        }

        /// <summary>
        /// Fills the fields of a request.(when a new one is added)
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

            Model.Doctor doctor;
            doctor = unitOfWork.Persons[IoCContainer.Get<IApplicationViewModel<Model.Doctor>>().User.PersonID].Doctor;

            // Update the database
            request.Doctor = doctor;
            request.Patient = patient;
            request.RequestPriority = priority;
            request.ThrombocytesQuantity = RequestThrombocytes;
            request.RedCellsQuantity = RequestRedCells;
            request.PlasmaQuantity = RequestPlasma;
            request.BloodQuantity = RequestBlood;
            request.IsFinished = false;
        }

        /// <summary>
        /// Fills the fields of a request viewmodel.(when a new one is added)
        /// </summary>
        /// <param name="request">The request viewmodel instance.</param>
        private void FillViewModelRequest(ref RequestItemViewModel request)
        {
            Model.Patient patient;
            patient = unitOfWork.Patients.Find(p => p.Person.PersonID == SelectedPatientName.Id).Single();
            Model.RequestPriority priority;
            priority = unitOfWork.RequestPriorities.Find(r => r.RequestPriorityID == SelectedPriority.Id).Single();
            Model.Doctor doctor;
            doctor = unitOfWork.Persons[IoCContainer.Get<IApplicationViewModel<Model.Doctor>>().User.PersonID].Doctor;
           

            // If the instance is null, initialize it
            if (request is null)
                request = new RequestItemViewModel();

            request.Id = patient.PersonID;
            request.PatientName = patient.Person.FirstName +" "+ patient.Person.LastName;
            request.Priority = priority.Type.ToString();
            request.Thrombocytes = "0/" + RequestThrombocytes;
            request.RedCells = "0/" + RequestRedCells;
            request.Plasma = "0/" + RequestPlasma;
            request.Blood = "0/" + RequestBlood;
            request.RequestStatus = "Pending";
            request.Cdc = routes[0].DestinationAddress.DonationCenters.ToList()[0].Name;
            request.Dcn = 0;
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
        }


        /// <summary>
        /// Cancel.
        /// </summary>
        private void Cancel()
        {
            try
            {
                // Remove from DB
                List<Model.BloodRequest> bloodRequests = unitOfWork.BloodRequests.Entities.ToList();
                Model.BloodRequest bloodRequest = bloodRequests.Find(br => br.BloodRequestID == SelectedTableItem.Id);
                unitOfWork.BloodRequests.Remove(bloodRequest);

                // Make changes persistent
                unitOfWork.Complete();

                // Remove from table
                Items.Remove(SelectedTableItem);

                VivusConsole.WriteLine("Doctor: Canceled a request!");
                Popup("The request was canceled!", PopupType.Successful);
            }
            catch
            {
                Popup($"An error occured while canceling the request.");
            }
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
        private string thrombocytes;
        private string redCells;
        private string plasma;
        private string blood;
        private string requestStatus;
        private string cdc;
        private int? dcn;

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
        public string Thrombocytes
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
        public string RedCells
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
        public string Plasma
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
        public string Blood
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
        public int? Dcn
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
