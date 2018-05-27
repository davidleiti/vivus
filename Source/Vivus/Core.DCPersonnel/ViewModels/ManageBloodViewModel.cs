namespace Vivus.Core.DCPersonnel.ViewModels
{
    using Vivus.Core.ViewModels;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Vivus.Core.DataModels;
    using System.Windows.Input;
    using System.Windows;
    using Vivus = Console;
    using Vivus.Core.DCPersonnel.Validators;
    using System;
    using System.Linq;
    using System.Globalization;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.Security;
    using Vivus.Core.DCPersonnel.IoC;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a view model for the manage blood page.
    /// </summary>
    public class ManageBloodViewModel : BaseViewModel
    {
        #region Private fields

        private string containerCode;
        private string harvestDate;
        private BasicEntity<string> containerType;

        private BasicEntity<string> addContainerBloodType;
        private BasicEntity<string> requestBloodType;

        private BasicEntity<string> addContainerRH;
        private BasicEntity<string> requestRH;

        private ButtonType buttonType;
        private bool actionIsRunning;
        private ContainersStorageItemViewModel selectedItem;

        private IUnitOfWork unitOfWork;
        private IApllicationViewModel<Model.DCPersonnel> appViewModel;
        private ISecurity security;

        private bool operationSuccessful;

        #endregion

        #region Public Enums

        /// <summary>
        /// Represents an enumaration of possbile parts of the page that can be validated.
        /// </summary>
        public enum Validation
        {
            None,
            ManageBlood,
            RequestDonation
        }

        #endregion

        #region Public properties

        public IPage ParentPage { get; set; }

        public ObservableCollection<ContainersStorageItemViewModel> Containers { get; }

        public List<BasicEntity<string>> ContainerTypes { get; }
        public List<BasicEntity<string>> BloodTypes { get; }
        public List<BasicEntity<string>> RHTypes { get; }

        public ContainersStorageItemViewModel SelectedItem
        {
            get => selectedItem;

            set
            {
                if (value == selectedItem)
                    return;

                selectedItem = value;

                if (selectedItem is null)
                {
                    ButtonType = ButtonType.Add;
                }
                else
                {
                    ButtonType = ButtonType.Modify;

                    dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());
                    PopulateFields();
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the type of the table button.
        /// </summary>
        public ButtonType ButtonType
        {
            get => buttonType;

            set
            {
                if (buttonType == value)
                    return;

                buttonType = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether an action is running or not.
        /// </summary>
        public bool ActionIsRunning
        {
            get => actionIsRunning;

            set
            {
                if (actionIsRunning == value)
                    return;

                actionIsRunning = value;

                OnPropertyChanged();
            }
        }

        public BasicEntity<string> ContainerType
        {
            get => containerType;

            set
            {
                if (containerType == value)
                    return;

                containerType = value;

                OnPropertyChanged();
            }
        }

        public BasicEntity<string> AddContainerBloodType
        {
            get => addContainerBloodType;

            set
            {
                if (addContainerBloodType == value)
                    return;

                addContainerBloodType = value;

                OnPropertyChanged();
            }
        }

        public BasicEntity<string> RequestBloodType
        {
            get => requestBloodType;

            set
            {
                if (requestBloodType == value)
                    return;

                requestBloodType = value;

                OnPropertyChanged();
            }
        }

        public BasicEntity<string> AddContainerRH
        {
            get => addContainerRH;

            set
            {
                if (addContainerRH == value)
                    return;

                addContainerRH = value;

                OnPropertyChanged();
            }
        }

        public BasicEntity<string> RequestRH
        {
            get => requestRH;

            set
            {
                if (requestRH == value)
                    return;

                requestRH = value;

                OnPropertyChanged();
            }
        }

        public string ContainerCode
        {
            get => containerCode;

            set
            {
                if (containerCode == value)
                    return;

                containerCode = value;

                OnPropertyChanged();
            }
        }

        public string HarvestDate
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

        /// <summary>
        /// Gets or sets the which portion of the page should be validated.
        /// </summary>
        public Validation ToValidate { get; set; }

        public ICommand AddCommand { get; }
        public ICommand RequestCommand { get; }

        // Gets the error string of a property
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(HarvestDate))
                    return GetErrorString(propertyName, ContainerInfoValidator.HarvestDateValidation(HarvestDate));
                if (propertyName == nameof(ContainerType))
                    return GetErrorString(propertyName, ContainerInfoValidator.ContainerTypeValidation(ContainerType));
                if (propertyName == nameof(ContainerCode))
                    return GetErrorString(propertyName, ContainerInfoValidator.ContainerCodeValidation(ContainerCode));
                if (propertyName == nameof(AddContainerBloodType))
                    return GetErrorString(propertyName, ContainerInfoValidator.BloodTypeValidation(AddContainerBloodType));
                if (propertyName == nameof(RequestBloodType))
                    return GetErrorString(propertyName, ContainerInfoValidator.BloodTypeValidation(RequestBloodType));
                if (propertyName == nameof(AddContainerRH))
                    return GetErrorString(propertyName, ContainerInfoValidator.RHValidation(AddContainerRH));
                if (propertyName == nameof(RequestRH))
                    return GetErrorString(propertyName, ContainerInfoValidator.RHValidation(RequestRH));

                return null;
            }
        }

        #endregion

        #region Constructors

        public ManageBloodViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            ContainerTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select container type") };
            BloodTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select blood type") };
            RHTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select rh") };
            AddCommand = new RelayCommand(async () => await AddModifyAsync());
            RequestCommand = new RelayCommand(async () => await SendReqAsync());
            Containers = new ObservableCollection<ContainersStorageItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Model.DCPersonnel>>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async () =>
            {
                await LoadContainerTypesAsync();
                await LoadBloodTypesAsync();
                await LoadRHTypesAsync();
                LoadContainersAsync();
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an Blood container.
        /// </summary>
        public async Task AddModifyAsync()
        {
            await RunCommand(() => ActionIsRunning, async () =>
            {
                bool result = false;
                ToValidate = Validation.ManageBlood;
                if (ButtonType == ButtonType.Add)
                {
                    await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());
                    result = await AddBloodContainerAsync();
                    if (result)
                    {
                        await dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());
                        ClearFieldsAddAndModify();
                    }
                }
                else
                {
                    await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());
                    result = await ModifyBloodContainerAsync();
                    if (result)
                    {
                        await dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());
                        ClearFieldsAddAndModify();
                    }
                }
                
                ToValidate = Validation.None;
            });
        }

        /// <summary>
        /// Sends a request.
        /// </summary>
        public async Task SendReqAsync()
        {
            await RunCommand(() => ActionIsRunning, async () =>
            {
                bool result = false;
                ToValidate = Validation.RequestDonation;

                await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                result = await SendRequestAsync();

                if (result)
                {
                    await dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());
                    // Add clear function
                    ClearFieldsRequest();
                }

                ToValidate = Validation.None;
            });
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Clears all the fields of the viewmodel.
        /// </summary>
        private void ClearFieldsAddAndModify()
        {
            ContainerType = new BasicEntity<string>(-1, "Select container type");
            ContainerCode = String.Empty;
            AddContainerBloodType = new BasicEntity<string>(-1, "Select blood type");
            AddContainerRH = new BasicEntity<string>(-1, "Select rh");
            HarvestDate = String.Empty;
        }

        private void ClearFieldsRequest()
        {
            RequestBloodType = new BasicEntity<string>(-1, "Select blood type");
            RequestRH = new BasicEntity<string>(-1, "Select rh");
        }

        /// <summary>
        /// Loads all the ContainerTypes asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadContainerTypesAsync()
        {
            await Task.Run(() =>
            {
                ContainerTypes.Clear();
                ContainerTypes.Add(new BasicEntity<string>(-1, "Select container type"));
                unitOfWork.BloodContainerTypes.Entities.ToList().ForEach(c =>
                    dispatcherWrapper.InvokeAsync(() => ContainerTypes.Add(new BasicEntity<string>(c.ContainerTypeID, c.Type)))
                );
            });
        }

        /// <summary>
        /// Loads all the ContainerTypes asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadBloodTypesAsync()
        {
            await Task.Run(() =>
            {
                BloodTypes.Clear();
                BloodTypes.Add(new BasicEntity<string>(-1, "Select blood type"));
                unitOfWork.BloodTypes.Entities.ToList().ForEach(b =>
                    dispatcherWrapper.InvokeAsync(() => BloodTypes.Add(new BasicEntity<string>(b.BloodTypeID, b.Type)))
                );
            });
        }

        /// <summary>
        /// Loads all the ContainerTypes asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadRHTypesAsync()
        {
            await Task.Run(() =>
            {
                RHTypes.Clear();
                RHTypes.Add(new BasicEntity<string>(-1, "Select rh"));
                unitOfWork.RHs.Entities.ToList().ForEach(rh =>
                    dispatcherWrapper.InvokeAsync(() => RHTypes.Add(new BasicEntity<string>(rh.RhID, rh.Type)))
                );
            });
        }

        /// <summary>
        /// Loads all the Containers asynchronously.
        /// </summary>
        private async void LoadContainersAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => Containers.Clear());
                unitOfWork.BloodContainers
                .Entities
                .ToList()
                .ForEach(bloodContainer =>
                    dispatcherWrapper.InvokeAsync(() => 
                        Containers.Add(new ContainersStorageItemViewModel
                        {
                            ContainerType = new BasicEntity<string>(bloodContainer.BloodContainerType.ContainerTypeID, bloodContainer.BloodContainerType.Type),
                            ContainerCode = bloodContainer.ContainerCode,
                            BloodType = new BasicEntity<string>(bloodContainer.BloodType.BloodTypeID, bloodContainer.BloodType.Type),
                            HarvestDate = bloodContainer.HarvestDate,
                            Rh = new BasicEntity<string>(bloodContainer.RH.RhID, bloodContainer.RH.Type),
                            Id = bloodContainer.BloodContainerID,
                            Expired = isExpired(ContainerType, bloodContainer.HarvestDate)
                            
                            
                })));

            });
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {
            ContainerType = selectedItem.ContainerType;
            ContainerCode = selectedItem.ContainerCode;
            AddContainerBloodType = selectedItem.BloodType;
            AddContainerRH = selectedItem.Rh;
            HarvestDate = selectedItem.HarvestDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds a new blood container
        /// </summary>
        private async Task<bool> AddBloodContainerAsync()
        {
            return await Task.Run(() =>
            {

                int count = errors.Keys
                        .Where(key => key != nameof(RequestBloodType) && key != nameof(RequestRH))
                        .ToList()
                        .Count;

                if (count > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return false;
                }

                try
                {
                    Model.BloodContainer bloodContainer;
                    bloodContainer = null;

                    ContainersStorageItemViewModel storageItemViewModel;
                    storageItemViewModel = null;

                    FillModelBloodContainer(ref bloodContainer);
                    unitOfWork.BloodContainers.Add(bloodContainer);
                    // Make changes persistent
                    //unitOfWork.Complete();

                    //Update the table
                    FillContainersStorageItemViewModel(ref storageItemViewModel);
                    storageItemViewModel.Id = bloodContainer.BloodContainerID;

                    dispatcherWrapper.InvokeAsync(() => Containers.Add(storageItemViewModel));

                    Popup($"Blood container added successfully!", PopupType.Successful);
                }
                catch
                {
                    
                    Popup($"An error occured while adding the blood container.");
                }

                return true;
            });

        }
        
        private async Task<bool> ModifyBloodContainerAsync()
        {
            return await Task.Run(() =>
            {
                if (selectedItem is null)
                    return false;

                int count = errors.Keys
                        .Where(key => key != nameof(RequestBloodType) && key != nameof(RequestRH))
                        .ToList()
                        .Count;

                if (count > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return false;
                }

                try
                {
                    Model.BloodContainer bloodContainer;

                    bloodContainer = unitOfWork.BloodContainers[SelectedItem.Id];

                    FillModelBloodContainer(ref bloodContainer);
                    // Make changes persistent
                    //unitOfWork.Complete();

                    FillContainersStorageItemViewModel(ref selectedItem);

                    Popup("Blood container modified successfully!", PopupType.Successful);

                }
                catch
                {
                    Popup("An error occured while modifing the blood container.");
                }

                ButtonType = ButtonType.Add;
                SelectedItem = null;
                return true;
            });
        }

        /// <summary>
        /// Request blood.
        /// </summary>
        private async Task<bool> SendRequestAsync()
        {
            return await Task.Run(() =>
            {
                int count = errors.Keys
                            .Where(key => key == nameof(RequestBloodType) || key == nameof(RequestRH))
                            .ToList()
                            .Count;

                if (count > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return false;
                }

                try
                {
                    int dcId = unitOfWork.DCPersonnel[appViewModel.User.AccountID].DonationCenterID;
                    
                    unitOfWork.Donors
                        .Entities.ToList()
                        .Where(donor => donor.DonationCenterID == dcId)
                        .ToList()
                            .ForEach(donor =>
                                unitOfWork.Messages.Add(new Model.Message
                                {
                                    SenderID = dcId,
                                    SendDate = DateTime.Now,
                                    Content = "We lack in blood of your type, we are already waiting for you at the donation center you are registered at, come and donate to save a life!",
                                    RecieverID = donor.AccountID
                                })
                            );
                    // Make changes persistent
                    //unitOfWork.Complete();

                }
                catch
                {
                    Popup("An error occured while sending requests.");
                }

                Vivus.Console.WriteLine("DC Personnel: Blood requested!");
                Popup("Successfull operation!", PopupType.Successful);
                return true;
            });
        }

        /// <summary>
        /// Fills the fields of a BloodContainer
        /// </summary>
        /// <param name="bloodContainer"></param>
        private void FillModelBloodContainer(ref Model.BloodContainer bloodContainer)
        {

            if (bloodContainer is null)
                bloodContainer = new Model.BloodContainer();

            //update BloodContainer
            bloodContainer.BloodContainerType = unitOfWork.BloodContainerTypes[ContainerType.Id];
            bloodContainer.ContainerCode = ContainerCode;
            bloodContainer.BloodType = unitOfWork.BloodTypes[AddContainerBloodType.Id];
            bloodContainer.RH = unitOfWork.RHs[AddContainerRH.Id];
            bloodContainer.HarvestDate = DateTime.Parse(HarvestDate);
        }

        /// <summary>
        /// Fills the fields of a ContainersStorageItemViewModel
        /// </summary>
        /// <param name="storageItemViewModel"></param>
        private void FillContainersStorageItemViewModel(ref ContainersStorageItemViewModel storageItemViewModel)
        {
            if (storageItemViewModel is null)
                storageItemViewModel = new ContainersStorageItemViewModel();

            storageItemViewModel.ContainerType =
                new BasicEntity<string>(unitOfWork.BloodContainerTypes[ContainerType.Id].ContainerTypeID,
                unitOfWork.BloodContainerTypes[ContainerType.Id].Type);
            storageItemViewModel.ContainerCode = ContainerCode;
            storageItemViewModel.BloodType =
                new BasicEntity<string>(unitOfWork.BloodTypes[AddContainerBloodType.Id].BloodTypeID,
                unitOfWork.BloodTypes[AddContainerBloodType.Id].Type);
            storageItemViewModel.Rh =
                new BasicEntity<string>(unitOfWork.RHs[AddContainerRH.Id].RhID,
                unitOfWork.RHs[AddContainerRH.Id].Type);
            storageItemViewModel.HarvestDate = DateTime.Parse(HarvestDate);
            storageItemViewModel.Expired = isExpired(storageItemViewModel.ContainerType, storageItemViewModel.HarvestDate);
        }

        private bool isExpired(BasicEntity<string> containerType, DateTime harvestDate)
        {
            if (containerType.Value.Equals("Thrombocytes"))
                return harvestDate.AddDays(6) <= DateTime.Now;
            if (containerType.Value.Equals("Red cells"))
                return harvestDate.AddDays(43) <= DateTime.Now;
            if (containerType.Value.Equals("Plasma"))
                return harvestDate.AddMonths(12).AddDays(1) <= DateTime.Now;
            return harvestDate.AddDays(43) <= DateTime.Now;
        }
        
        #endregion
    }

    public class ContainersStorageItemViewModel : BaseViewModel
    {
        #region Private members

        private int id;
        private string containerCode;
        private BasicEntity<string> containerType;
        private BasicEntity<string> bloodType;
        private BasicEntity<string> rh;
        private DateTime harvestDate;
        private bool expired;

        #endregion

        #region Public properties

        public BasicEntity<string> Rh
        {
            get => rh;

            set
            {
                if (rh == value)
                    return;

                rh = value;

                OnPropertyChanged();
            }
        }

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

        public string ContainerCode
        {
            get => containerCode;

            set
            {
                if (containerCode == value)
                    return;

                containerCode = value;

                OnPropertyChanged();
            }
        }

        public BasicEntity<string> ContainerType
        {
            get => containerType;

            set
            {
                if (containerType == value)
                    return;

                containerType = value;

                OnPropertyChanged();
            }
        }

        public BasicEntity<string> BloodType
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

        public DateTime HarvestDate
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

        public bool Expired
        {
            get
            {
                if (containerType.Value.Equals("Thrombocytes"))
                    return harvestDate.AddDays(6) <= DateTime.Now;
                if (containerType.Value.Equals("Red cells"))
                    return harvestDate.AddDays(43) <= DateTime.Now;
                if (containerType.Value.Equals("Plasma"))
                    return harvestDate.AddMonths(12).AddDays(1) <= DateTime.Now;
                return harvestDate.AddDays(43) <= DateTime.Now;
            }

            set
            {
                if (expired == value)
                    return;

                expired = value;

                OnPropertyChanged();
            }

        }

        #endregion
    }

}