﻿namespace Vivus.Core.DCPersonnel.ViewModels
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

                    dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());

                    //ClearFieldsAddAndModify();
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
            RequestCommand = new RelayCommand(Request);
            Containers = new ObservableCollection<ContainersStorageItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Model.DCPersonnel>>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async () =>
            {
                await Task.Run(() =>
                {
                    LoadContainerTypesAsync();
                    LoadBloodTypesAsync();
                    LoadRHTypesAsync();
                    LoadContainersAsync();
                });
                //PopulateFields();
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
                if (ButtonType == ButtonType.Add)
                    await AddBloodContainerAsync();
                //else
                //    await ModifyAdministratorAsync();

                ClearFieldsAddAndModify();
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

        /// <summary>
        /// Loads all the ContainerTypes asynchronously.
        /// </summary>
        /// <returns></returns>
        private async void LoadContainerTypesAsync()
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
        private async void LoadBloodTypesAsync()
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
        private async void LoadRHTypesAsync()
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
                Containers.Clear();
                unitOfWork.BloodContainers
                .Entities
                .ToList()
                .ForEach(bloodContainer =>
                    dispatcherWrapper.InvokeAsync(() => 
                        Containers.Add(new ContainersStorageItemViewModel
                        {
                            ContainerType = bloodContainer.BloodContainerType.Type,
                            ContainerCode = bloodContainer.ContainerCode,
                            BloodType = bloodContainer.BloodType.Type,
                            HarvestDate = bloodContainer.HarvestDate,
                            Id = bloodContainer.BloodContainerID
                })));

            });
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {
            ContainerType = new BasicEntity<string>(1, selectedItem.ContainerType);
            ContainerCode = selectedItem.ContainerCode;
            AddContainerBloodType = new BasicEntity<string>(1, selectedItem.BloodType);
            AddContainerRH = new BasicEntity<string>(1, "nada");
            HarvestDate = selectedItem.HarvestDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds a new blood container
        /// </summary>
        private async Task AddBloodContainerAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                int count = errors.Keys
                        .Where(key => key != nameof(RequestBloodType) && key != nameof(RequestRH))
                        .ToList()
                        .Count;

                if (count > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
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
            });

        }

        /// <summary>
        /// Request blood.
        /// </summary>
        private void Request()
        {
            int count;

            ToValidate = Validation.RequestDonation;
            ParentPage.AllowErrors();

            count = errors.Keys
                        .Where(key => key == nameof(RequestBloodType) || key == nameof(RequestRH))
                        .Select(key => errors[key]).Aggregate((l1, l2) => l1.Concat(l2).ToList())
                        .Count;

            if (count > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("DC Personnel: Blood requested!");
            Popup("Successfull operation!", PopupType.Successful);
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

            storageItemViewModel.ContainerType = unitOfWork.BloodContainerTypes[ContainerType.Id].Type;
            storageItemViewModel.ContainerCode = ContainerCode;
            storageItemViewModel.BloodType = unitOfWork.BloodTypes[AddContainerBloodType.Id].Type;
            storageItemViewModel.HarvestDate = DateTime.Parse(HarvestDate);
        }

        

        #endregion
    }

    public class ContainersStorageItemViewModel : BaseViewModel
    {
        #region Private members

        private int id;
        private string containerCode;
        private string containerType;
        private string bloodType;
        private DateTime harvestDate;

        #endregion

        #region Public properties

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

        public string ContainerType
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
                
                    if (ContainerType.Equals("Thrombocytes"))
                        return HarvestDate.AddDays(6) <= DateTime.Now;
                    if (ContainerType.Equals("Red cells"))
                        return HarvestDate.AddDays(43) <= DateTime.Now;
                    if (ContainerType.Equals("Plasma"))
                        return HarvestDate.AddMonths(12).AddDays(1) <= DateTime.Now;
                    return HarvestDate.AddDays(43) <= DateTime.Now;
            }

        }

        #endregion
    }

}