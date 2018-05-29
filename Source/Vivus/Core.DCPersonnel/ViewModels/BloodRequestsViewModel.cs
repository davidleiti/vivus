namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.IoC;
    using Vivus.Core.DCPersonnel.Validators;
    using Vivus.Core.Helpers;
    using Vivus.Core.Model;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using static Vivus.Core.Helpers.DistanceMatrixApiHelpers;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the blood requests page.
    /// </summary>
    public class BloodRequestsViewModel : BaseViewModel
    {
        #region Private members
        private int? thrombocytes;
        private int? plasma;
        private int? redCells;
        private int? blood;

        private string currentThrombocytes;
        private string currentPlasma;
        private string currentRedCells;
        private string currentBlood;
        private string priority;
        private string bloodType;
        private DateTime? harvestDate;

        private BasicEntity<string> containerType;
        private BasicEntity<string> containerCode;
        private BasicEntity<string> donationCenter;

        private List<BasicEntity<string>> containerTypes;
        private List<BasicEntity<string>> containerCodes;
        private List<BasicEntity<string>> donationCenters;

        private RequestDetailsItem selectedRequestDetailsItem;
        private AllRequestsItem selectedAllRequestsItem;
        private object allRequestsLockObj;
        private object requestDetailsLockObj;
        private ObservableCollection<RequestDetailsItem> requestDetailsItems;
        private ObservableCollection<AllRequestsItem> allRequestsItems;

        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<DCPersonnel> appViewModel;

        #endregion

        #region Public Enums

        /// <summary>
        /// Represents an enumaration of possbile parts of the page that can be validated.
        /// </summary>
        public enum Validation
        {
            None,
            ManageRequest,
            DonationCenterRedirect
        }

        #endregion

        #region Public properties

        public ButtonType ButtonType { get; set; }

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
        /// Gets or sets the stored blood.
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
        /// Gets or sets the which portion of the page should be validated.
        /// </summary>
        public Validation ToValidate { get; set; }

        public string CurrentThrombocytes
        {

            get => currentThrombocytes;

            set
            {
                if (currentThrombocytes == value)

                    return;

                currentThrombocytes = value;

                OnPropertyChanged();
            }
        }
        public string CurrentPlasma
        {
            get => currentPlasma;

            set
            {
                if (currentPlasma == value)

                    return;

                currentPlasma = value;

                OnPropertyChanged();
            }
        }
        public string CurrentRedCells
        {
            get => currentRedCells;

            set
            {
                if (currentRedCells == value)
                    return;

                currentRedCells = value;

                OnPropertyChanged();
            }

        }

        /// <summary>
        /// Gets or sets the current number of blood containers for the current request.
        /// </summary>
        public string CurrentBlood
        {
            get => currentBlood;

            set
            {
                if (currentBlood == value)
                    return;

                currentBlood = value;

                OnPropertyChanged();
            }
        }


        public string Priority
        {
            get => priority;
            set
            {
                if (value == priority)

                    return;

                priority = value;

                OnPropertyChanged();
            }
        }
        public string BloodType
        {
            get => bloodType;

            set
            {
                if (bloodType == value)

                    return;

                bloodType = value;

                OnPropertyChanged();
            }
        }
        public BasicEntity<string> ContainerType
        {
            get => containerType;
            set
            {
                if (value == containerType) return;

                containerType = value;
                LoadContainerCodesAsync();

                OnPropertyChanged();
            }
        }
        public BasicEntity<string> ContainerCode
        {
            get => containerCode;
            set
            {
                if (value == containerCode)

                    return;

                containerCode = value;
                PopulateCaseInfoAsync();

                OnPropertyChanged();
            }
        }
        private DateTime? HarvestDate
        {

            get => harvestDate;

            set
            {
                if (harvestDate == value)

                    return;

                harvestDate = value;

                OnPropertyChanged();
            }

        }
        public BasicEntity<string> DonationCenter
        {
            get => donationCenter;
            set
            {
                if (value == donationCenter)

                    return;

                donationCenter = value;

                OnPropertyChanged();
            }
        }
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(ContainerType))

                    return GetErrorString(propertyName, DCPersonnelValidator.ContainerTypeValidation(ContainerType));

                if (propertyName == nameof(ContainerCode))

                    return GetErrorString(propertyName, DCPersonnelValidator.ContainerCodeValidation(containerCode));

                if (propertyName == nameof(DonationCenter))

                    return GetErrorString(propertyName, DCPersonnelValidator.DonationCenterValidation(donationCenter));

                return null;
            }
        }
        public IPage ParentPage { get; set; }

        public List<BasicEntity<string>> ContainerTypes
        {
            get => containerTypes;

            set
            {
                if (containerTypes == value)

                    return;
                containerTypes = value;

                OnPropertyChanged();
            }
        }
        public List<BasicEntity<string>> ContainerCodes
        {
            get => containerCodes;

            set
            {
                if (containerCodes == value)

                    return;

                containerCodes = value;
                    

                OnPropertyChanged();

            }
        }
        public List<BasicEntity<string>> DonationCenters
        {
            get => donationCenters;

            set
            {
                if (donationCenters == value)

                    return;

                donationCenters = value;

                OnPropertyChanged();
            }
        }
        public ObservableCollection<RequestDetailsItem> RequestDetailsItems
        {
            get => requestDetailsItems;
            set
            {
                if (requestDetailsItems == value)

                    return;

                requestDetailsItems = value;

                OnPropertyChanged();
            }
        }
        public ObservableCollection<AllRequestsItem> AllRequestsItems
        {
            get => allRequestsItems;

            set
            {
                if (allRequestsItems == value)

                    return;

                allRequestsItems = value;

                OnPropertyChanged();
            }
        }

        public RequestDetailsItem SelectedRequestDetailsItem
        {
            get => selectedRequestDetailsItem;

            set
            {
                if (selectedRequestDetailsItem == value)

                    return;

                selectedRequestDetailsItem = value;

                OnPropertyChanged();
            }
        }
        public AllRequestsItem SelectedAllRequestsItem
        {
            get => selectedAllRequestsItem;
            set
            {
                if (selectedAllRequestsItem == value)

                    return;

                selectedAllRequestsItem = value;
                if(selectedAllRequestsItem is null)
                {
                    ClearFields();
                }
                else
                {
                    Task.Run(async () =>
                    {
                        await LoadContainerTypesAsync();
                        await LoadDonationCentersAsync();
                    });
                    PopulateFields();
                }
                Vivus.Console.WriteLine(selectedAllRequestsItem.Doctor + "'s request was selected");

                OnPropertyChanged();
            }

        }
        public ICommand AddCommand { get; }

        public ICommand RemoveCommand { get; }

        public ICommand RedirectCommand { get; }

        public ICommand FinishCommand { get; }

        #endregion

        #region Constructors
        public BloodRequestsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            ContainerTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select container type") };
            // ContainerTypes.Add(new BasicEntity<string>(11, "value"));
            //ContainerTypes = new List<BasicEntity<string>> { };
            ContainerCodes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select container code") };
            //ContainerCodes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select container code") };
            DonationCenters = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select donation center") };
            AddCommand = new RelayCommand(AddRequestAsync);
            RemoveCommand = new RelayCommand(RemoveRequestAsync);
            RedirectCommand = new RelayCommand(RedirectRequestAsync);
            FinishCommand = new RelayCommand(FinishRequestAsync);

            // lock objects for the 2 Observable Collections
            requestDetailsLockObj = new object();
            allRequestsLockObj = new object();

            RequestDetailsItems = new ObservableCollection<RequestDetailsItem> { new RequestDetailsItem() };
            AllRequestsItems = new ObservableCollection<AllRequestsItem> { new AllRequestsItem() };

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<DCPersonnel>>();

            BindingOperations.EnableCollectionSynchronization(RequestDetailsItems, requestDetailsLockObj);
            BindingOperations.EnableCollectionSynchronization(AllRequestsItems, allRequestsLockObj);

            dispatcherWrapper.InvokeAsync(() => {
                AllRequestsItems.Add(new AllRequestsItem
                {
                    Id = 1,
                    Doctor = "Paul Alfredovici",
                    Priority = "High",
                    Thrombocytes = "0/3",
                    RedCells = "0/2",
                    Blood = "0/2",
                    Plasma = "0/0",
                    BloodType = "A2"
                });
            });

            Task.Run(async () =>
            {
                await LoadRequestsAsync();
                await LoadStashContainersAsync();
            });
            

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the reauest table
        /// </summary>
        private async Task LoadRequestsAsync()
        {
            lock (allRequestsLockObj)
                AllRequestsItems.Clear();

            lock (requestDetailsLockObj)
                RequestDetailsItems.Clear();

            IEnumerable<BloodRequest> bloodRequests = await unitOfWork.BloodRequests.GetAllAsync();
            DCPersonnel dcPersonnel = appViewModel.User;
            AllRequestsItem allRequestsItem;
            int currentDonationCenterId = dcPersonnel.DonationCenterID;

            foreach (var bloodRequest in bloodRequests.Where(bloodReq => bloodReq.IsFinished))
            {
                List<DonationCenter> associatedDonationCenters = bloodRequest.DonationCenters.ToList();
                if (associatedDonationCenters.FindIndex(donationCenter => donationCenter.DonationCenterID == currentDonationCenterId) != -1)
                {
                    Dictionary<String, String> bloodContainersStrings = await generateContainersQuantityStrings(bloodRequest);
                    allRequestsItem = new AllRequestsItem
                    {
                        Id = bloodRequest.BloodRequestID,
                        Doctor = bloodRequest.Doctor.Person.FirstName + " " + bloodRequest.Doctor.Person.LastName,
                        Priority = bloodRequest.RequestPriority.Type,
                        BloodType = bloodRequest.Patient.BloodType.Type,
                        Plasma = bloodContainersStrings["Plasma"],
                        RedCells = bloodContainersStrings["Red cells"],
                        Thrombocytes = bloodContainersStrings["Thrombocytes"],
                        Blood = bloodContainersStrings["Blood"]
                    };

                    lock (allRequestsLockObj)
                        AllRequestsItems.Add(allRequestsItem);
                }
            }
        }

        /// <summary>
        /// Loads the stash containers
        /// </summary>
        private async Task LoadStashContainersAsync()
        {
            DCPersonnel dcPersonnel = appViewModel.User;
            int currentDonationCenterId = dcPersonnel.DonationCenterID;

            IEnumerable<BloodContainer> bloodContainers = await unitOfWork.BloodContainers.GetAllAsync();
            Dictionary<String, int> stashBloodContainers = new Dictionary<String, int>();

            foreach(BloodContainer bloodContainer in bloodContainers)
            {
                if(bloodContainer.DonationCenterID == currentDonationCenterId)
                {
                    string type = bloodContainer.BloodContainerType.Type;
                    if (!stashBloodContainers.ContainsKey(type)){
                        stashBloodContainers[type] = 0;
                    }
                    stashBloodContainers[type] += 1;
                }
            }

            Thrombocytes = stashBloodContainers.ContainsKey("Thrombocytes") ? stashBloodContainers["Thrombocytes"] : 0;
            RedCells = stashBloodContainers.ContainsKey("Red cells") ? stashBloodContainers["Red cells"] : 0;
            Plasma = stashBloodContainers.ContainsKey("Plasma") ? stashBloodContainers["Plasma"] : 0;
            Blood = stashBloodContainers.ContainsKey("Blood") ? stashBloodContainers["Blood"] : 0;
        }

        /// <summary>
        /// Loads the donation centers based on the provided blood request
        /// </summary>
        /// <param name="bloodRequest">the provided blood request</param>
        private async Task LoadDonationCentersAsync()
        {
            DCPersonnel dcPersonnel = appViewModel.User;
            int currentDonationCenterId = dcPersonnel.DonationCenterID;

            
            IEnumerable<BloodRequest> bloodRequests = await unitOfWork.BloodRequests.GetAllAsync();
            BloodRequest bloodRequest = null;
            foreach(BloodRequest bloodReq in bloodRequests)
            {
                DonationCenter lastDonationCenter = bloodReq.DonationCenters.Last();

                if(lastDonationCenter.DonationCenterID == currentDonationCenterId)
                {
                    bloodRequest = bloodReq;
                }
            }

            DonationCenter currentDonationCenter = await unitOfWork.DonationCenters.SingleAsync(donationCenter => donationCenter.DonationCenterID == currentDonationCenterId);
            Address currentDonationCenterAddress = await unitOfWork.Addresses.SingleAsync(address => address.AddressID == currentDonationCenter.AddressID);

            IEnumerable<DonationCenter> allDonationCenters = await unitOfWork.DonationCenters.GetAllAsync();

            IEnumerable<DonationCenter> notVisitedCenters = allDonationCenters.Where(donationCenter => !donationCenter.BloodRequests.Contains(bloodRequest));
            IEnumerable<DonationCenter> visitedCenters = allDonationCenters.Where(donationCenter => donationCenter.BloodRequests.Contains(bloodRequest));

            List<Address> donationCentersAddresses = new List<Address>();
            Address donationCenterAddress;
            foreach (var donationCenter in notVisitedCenters)
            {
                donationCenterAddress = await unitOfWork.Addresses.SingleAsync(singleAddress => singleAddress.AddressID == donationCenter.AddressID);

                donationCentersAddresses.Add(donationCenterAddress);
            }
            IEnumerable<RouteDetails> routes = (await DistanceMatrixApiHelpers.GetDistancesAsync(currentDonationCenterAddress, donationCentersAddresses)).OrderBy(RouteDetails => RouteDetails.Distance);
            donationCentersAddresses.Clear();
            foreach(var donationCenter in notVisitedCenters)
            {
                donationCenterAddress = await unitOfWork.Addresses.SingleAsync(singleAddress => singleAddress.AddressID == donationCenter.AddressID);

                donationCentersAddresses.Add(donationCenterAddress);
            }
            DonationCenters.Clear();
            DonationCenters.Add(new BasicEntity<string>(-1, "select donation center"));
            foreach (RouteDetails route in routes)
            {
                DonationCenter donationCenter = unitOfWork.DonationCenters.Entities.Single(donCenter => donCenter.AddressID == route.DestinationAddress.AddressID);
                DonationCenters.Add(new BasicEntity<String>(donationCenter.DonationCenterID, donationCenter.Name));
            }
            foreach(DonationCenter donationCenter in visitedCenters)
            {
                DonationCenters.Add(new BasicEntity<String>(donationCenter.DonationCenterID, donationCenter.Name));
            }
        }
        
        /// <summary>
        /// Generates strings for quantities in a blood request
        /// </summary>
        /// <param name="bloodRequest">the provided blood request</param>
        /// <returns></returns>
        private async Task<Dictionary<String, String>> generateContainersQuantityStrings(BloodRequest bloodRequest)
        {
            Dictionary<String, String> containersString = new Dictionary<String, String>();
            int plasmaContainers = unitOfWork.BloodContainers.Entities.ToList().FindAll(bloodContainer => bloodContainer.BloodRequestID == bloodRequest.BloodRequestID && bloodContainer.BloodContainerType.Type == "Plasma").Count();
            int redCellsContainers = unitOfWork.BloodContainers.Entities.ToList().FindAll(bloodContainer => bloodContainer.BloodRequestID == bloodRequest.BloodRequestID && bloodContainer.BloodContainerType.Type == "Red cells").Count();
            int thrombocytesContainers = unitOfWork.BloodContainers.Entities.ToList().FindAll(bloodContainer => bloodContainer.BloodRequestID == bloodRequest.BloodRequestID && bloodContainer.BloodContainerType.Type == "Thrombocytes").Count();
            int bloodContainers = unitOfWork.BloodContainers.Entities.ToList().FindAll(bloodContainer => bloodContainer.BloodRequestID == bloodRequest.BloodRequestID && bloodContainer.BloodContainerType.Type == "Blood").Count();

            containersString.Add("Plasma", plasmaContainers + "/" + bloodRequest.PlasmaQuantity);
            containersString.Add("Red cells", redCellsContainers + "/" + bloodRequest.RedCellsQuantity);
            containersString.Add("Thrombocytes", thrombocytesContainers + "/" + bloodRequest.ThrombocytesQuantity);
            containersString.Add("Blood",  bloodContainers + "/" + bloodRequest.BloodQuantity);

            return containersString;
        }

        /// <summary>
        /// Loads the container types
        /// </summary>
        private async Task LoadContainerTypesAsync()
        {
            if(selectedAllRequestsItem == null)
            {
                return;
            }

            DCPersonnel dcPersonnel = appViewModel.User;
            int currentDonationCenterId = dcPersonnel.DonationCenterID;

            IEnumerable<BloodContainer> bloodContainers = await unitOfWork.BloodContainers.GetAllAsync();
            Dictionary<String, int> stashBloodContainers = new Dictionary<String, int>();

            foreach (BloodContainer bloodContainer in bloodContainers)
            {
                if (bloodContainer.DonationCenterID == currentDonationCenterId)
                {
                    string type = bloodContainer.BloodContainerType.Type;
                    if (!stashBloodContainers.ContainsKey(type))
                    {
                        stashBloodContainers[type] = 0;
                    }
                    stashBloodContainers[type] += 1;
                }
            }

           this.ContainerTypes.Clear();
           this.ContainerTypes.Add(new BasicEntity<string>(-1, "Select container code"));
           foreach (String bloodConType in stashBloodContainers.Keys)
           {
                BloodContainerType containerType = unitOfWork.BloodContainerTypes.Entities.Single(bloodContainerType => bloodContainerType.Type == bloodConType);
                this.ContainerTypes.Add(new BasicEntity<String>(containerType.ContainerTypeID, containerType.Type));
           }
        }


        /// <summary>
        /// Loads conatiner cases
        /// </summary>
        private async void LoadContainerCodesAsync()
        {
            DCPersonnel dcPersonnel = appViewModel.User;
            int currentDonationCenterId = dcPersonnel.DonationCenterID;

            if(containerType is null || containerType.Id == -1)
            {
                this.ContainerCodes.Clear();
                this.ContainerCodes.Add(new BasicEntity<string>(-1, "Select container type"));
                return;
            }

            List<BasicEntity<String>> contCodes = new List<BasicEntity<string>>();  
            IEnumerable<BloodContainer> bloodContainers = await unitOfWork.BloodContainers.GetAllAsync();
            foreach(BloodContainer bloodContainer in bloodContainers)
            {
                if (bloodContainer.BloodContainerType.ContainerTypeID == containerType.Id && bloodContainer.DonationCenterID == currentDonationCenterId) 
                {
                    contCodes.Add(new BasicEntity<string>(bloodContainer.BloodContainerID, bloodContainer.ContainerCode));
                }
            }
            this.ContainerCodes.Clear();
            this.ContainerCodes.Add(new BasicEntity<string>(-1, "Select container type"));
            foreach (BasicEntity<String> contCode in contCodes)
            {
                this.ContainerCodes.Add(contCode);
            }
        }

        /// <summary>
        /// Populates the info regarding the selected blood case
        /// </summary>
        private async void PopulateCaseInfoAsync()
        {
            if(containerCode is null || containerCode.Id == -1)
            {
                this.bloodType = string.Empty;
                this.harvestDate = null;
                return;
            }

            BloodContainer bloodContainer = await unitOfWork.BloodContainers.SingleAsync(bloodCon => bloodCon.BloodContainerID == containerCode.Id);
            BloodType bloodTy = await unitOfWork.BloodTypes.SingleAsync(bloodT => bloodT.BloodTypeID == bloodContainer.BloodTypeID);
            this.BloodType = bloodTy.Type;
            this.HarvestDate = bloodContainer.HarvestDate;
        }

        private void PopulateFields()
        {
            Priority = selectedAllRequestsItem.Priority;
            CurrentThrombocytes = selectedAllRequestsItem.Thrombocytes;
            CurrentRedCells = selectedAllRequestsItem.RedCells;
            CurrentPlasma = selectedAllRequestsItem.Plasma;
            CurrentBlood = selectedAllRequestsItem.Blood;
        }

        private void ClearFields()
        {
            Priority = string.Empty;
            CurrentThrombocytes = string.Empty;
            CurrentRedCells = string.Empty;
            CurrentPlasma = string.Empty;
            CurrentBlood = string.Empty;

            ContainerTypes.Clear();
            ContainerTypes.Add(new BasicEntity<string>(-1, "Select container code"));
            ContainerCodes.Clear();
            ContainerCodes.Add(new BasicEntity<string>(-1, "Select container type"));
            DonationCenters.Clear();
            DonationCenters.Add(new BasicEntity<string>(-1, "Select donation center"));
        }

        private void FillModelBloodContainer(ref BloodContainer container)
        {

        }

        private void FillRequestDetailsItem(ref RequestDetailsItem requestDetail, int id)
        {
            requestDetail.BloodType = this.BloodType;
            requestDetail.ContainerCode = this.ContainerCode.Value;
            requestDetail.ContainerType = this.ContainerType.Value;
            requestDetail.HarvestDate = this.HarvestDate.GetValueOrDefault();
            requestDetail.Id = id;
        }

        public async void AddRequestAsync()
        {
            int count;

            ToValidate = Validation.ManageRequest;
            ParentPage.AllowErrors();

            count = errors.Keys
                        .Where(key => key != nameof(DonationCenter))
                        .Select(key => errors[key]).Aggregate((l1, l2) => l1.Concat(l2).ToList())
                        .Count;

            if (count > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");

                return;
            }

            if(this.containerCode == null)
            {
                Popup("No container is selected!");

                return;
            }

            await dispatcherWrapper.InvokeAsync(async () =>
            {
                DCPersonnel dcPersonnel = appViewModel.User;
                int currentDonationCenterId = dcPersonnel.DonationCenterID;

                BloodContainer bloodContainer = await unitOfWork.BloodContainers.SingleAsync(bloodCont => bloodCont.BloodContainerID == this.containerCode.Id);
                IEnumerable<BloodRequest> bloodRequests = await unitOfWork.BloodRequests.GetAllAsync();
                BloodRequest bloodRequest = null;
                foreach (BloodRequest bloodReq in bloodRequests)
                {
                    DonationCenter lastDonationCenter = bloodReq.DonationCenters.Last();

                    if (lastDonationCenter.DonationCenterID == currentDonationCenterId)
                    {
                        bloodRequest = bloodReq;
                    }
                }
                bloodContainer.BloodRequestID = bloodRequest.BloodRequestID;
                RequestDetailsItem bloodContainerVM = new RequestDetailsItem();
                FillRequestDetailsItem(ref bloodContainerVM, bloodContainer.BloodContainerID);
                this.RequestDetailsItems.Add(bloodContainerVM);
            });

            Vivus.Console.WriteLine("DCPersonnel BloodRequest: Request added!");
            Popup("Successfull operation!", PopupType.Successful);
        }
        public void RemoveRequestAsync()
        {
            int count;

            ToValidate = Validation.ManageRequest;
            ParentPage.AllowErrors();

            count = errors.Keys
                        .Where(key => key != nameof(DonationCenter))
                        .Select(key => errors[key]).Aggregate((l1, l2) => l1.Concat(l2).ToList())
                        .Count;

            if (count > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("DCPersonnel BloodRequest: Request removed!");
            Popup("Successfull operation!", PopupType.Successful);
        }
        public void RedirectRequestAsync()
        {
            int count;

            ToValidate = Validation.DonationCenterRedirect;
            ParentPage.AllowErrors();

            count = errors[nameof(DonationCenter)].Count;

            if (count > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("DCPersonnel BloodRequest: Request redirected!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        private void FinishRequestAsync()
        {
            Vivus.Console.WriteLine("DCPersonnel BloodRequest: Request finished!");
            Popup("Successfull operation!", PopupType.Successful);
        }
        #endregion

    }

    public class RequestDetailsItem : BaseViewModel
    {
        #region Private members
        private int id;
        private string containerType;
        private string containerCode;
        private string bloodType;
        private DateTime harvestDate;
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
        public string ContainerType
        {
            get => containerType;
            set
            {
                if (value == containerType)
                    return;
                containerType = value;
                OnPropertyChanged();

            }
        }
        public string ContainerCode
        {
            get => containerCode;
            set
            {
                if (value == containerCode)
                    return;
                containerCode = value;
                OnPropertyChanged();

            }
        }
        public string BloodType
        {
            get => bloodType;
            set
            {
                if (value == bloodType)
                    return;
                bloodType = value;
                OnPropertyChanged();
            }
        }
        public DateTime HarvestDate
        {
            get => harvestDate;
            set
            {
                if (value == harvestDate)
                    return;
                harvestDate = value;
                OnPropertyChanged();
            }
        }
        public override string this[string propertyName]
        {
            get { return null; }
        }
        #endregion
        #region Contrucors
        public RequestDetailsItem()
        {
            Id = 11;
            ContainerCode = "lol";
            ContainerType = "typeee";
            bloodType = "rhpoz";
            harvestDate = new DateTime(2012, 11, 1);
        }
        #endregion
    }
    public class AllRequestsItem : BaseViewModel
    {
        #region Private memebers
        private int id;
        private string doctor;

        private string priority;
        private string thrombocytes;
        private string redCells;
        private string plasma;
        private string blood;
        private string bloodType;

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
        public string Doctor
        {
            get => doctor;
            set
            {
                if (value == doctor)

                    return;

                doctor = value;

                OnPropertyChanged();
            }
        }
        public string Priority
        {
            get => priority;
            set
            {
                if (value == priority)

                    return;

                priority = value;

                OnPropertyChanged();
            }
        }
        public string Thrombocytes
        {
            get => thrombocytes;
            set
            {
                if (value == thrombocytes)

                    return;

                thrombocytes = value;

                OnPropertyChanged();

            }

        }
        public string Plasma
        {
            get => plasma;
            set
            {
                if (value == plasma)

                    return;

                plasma = value;

                OnPropertyChanged();

            }
        }
        public string RedCells
        {
            get => redCells;
            set
            {
                if (value == redCells)
                    return;

                redCells = value;

                OnPropertyChanged();

            }

        }

        /// <summary>
        /// Gets or sets the current blood containers number.
        /// </summary>
        public string Blood
        {
            get => blood;

            set
            {
                if (value == blood)

                    return;

                blood = value;

                OnPropertyChanged();
            }
        }

        public string BloodType
        {
            get => bloodType;
            set
            {
                if (value == bloodType)

                    return;

                bloodType = value;

                OnPropertyChanged();
            }
        }
        public override string this[string propertyName]
        {
            get { return null; }
        }
        #endregion
        #region Constructors
        public AllRequestsItem()
        {
            Id = 10;
            Doctor = "doc";
            Thrombocytes = "0/10";
            Priority = "high";
            redCells = "0/11";
            Plasma = "0/12";
            Blood = "0/13";
            BloodType = "rhneg";
        }
       
        #endregion
    }

}
