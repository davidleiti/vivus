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

        //private List<BasicEntity<string>> containerTypes;
        //private List<BasicEntity<string>> bloodTypes;
        //private List<BasicEntity<string>> rhTypes;

        private string containerCode;
        private string harvestDate;
        private BasicEntity<string> containerType;

        private BasicEntity<string> addContainerBloodType;
        private BasicEntity<string> requestBloodType;

        private BasicEntity<string> addContainerRH;
        private BasicEntity<string> requestRH;

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

        public ContainersStorageItemViewModel SelectedItem { get; set; }

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
            ContainerTypes = new List<BasicEntity<string>>();
            BloodTypes = new List<BasicEntity<string>>();
            RHTypes = new List<BasicEntity<string>>();
            AddCommand = new RelayCommand(Add);
            RequestCommand = new RelayCommand(Request);
            Containers = new ObservableCollection<ContainersStorageItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Model.DCPersonnel>>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async () =>
            {
                await LoadContainerTypesAsync();
                await LoadBloodTypesAsync();
                await LoadRHTypesAsync();
                //PopulateFields();
            });
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Clears all the fields of the viewmodel.
        /// </summary>
        //private void ClearFields()
        //{
        //    Email = string.Empty;
        //    IsActive = true;
        //    IsActiveIsEnabled = true;
        //    Person.FirstName = string.Empty;
        //    Person.LastName = string.Empty;
        //    Person.BirthDate = string.Empty;
        //    Person.NationalIdentificationNumber = string.Empty;
        //    Person.PhoneNumber = string.Empty;
        //    Person.Gender = new BasicEntity<string>(-1, "Not specified");
        //    Address.StreetName = string.Empty;
        //    Address.StreetNumber = string.Empty;
        //    Address.City = string.Empty;
        //    Address.County = Counties[0];
        //    Address.ZipCode = string.Empty;
        //}

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
                    dispatcherWrapper.InvokeAsync(() => BloodTypes.Add(new BasicEntity<string>(rh.RhID, rh.Type)))
                );
            });
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {
            unitOfWork.BloodContainerTypes.Entities.ToList()
                .ForEach(c =>dispatcherWrapper.InvokeAsync(() => ContainerTypes.Add(new BasicEntity<string>(c.ContainerTypeID, c.Type))));
        }

        /// <summary>
        /// Adds a new blood container
        /// </summary>
        private void Add()
        {
            int count;

            ToValidate = Validation.ManageBlood;
            ParentPage.AllowErrors();

            //count = errors.Keys
            //            .Where(key => key != nameof(RequestBloodType) && key != nameof(RequestRH))
            //            .Select(key => errors[key]).Aggregate((l1, l2) => l1.Concat(l2).ToList())
            //            .Count;

            //if (count > 0)
            //{
            //    Popup("Some errors were found. Fix them before going forward.");
            //    return;
            //}

            Vivus.Console.WriteLine(SelectedItem.ContainerType);

            Vivus.Console.WriteLine("DCPersonnel Manage Blood: Container added!");
            Popup("Successfull operation!", PopupType.Successful);
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
        private bool expired;

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
            get => expired;

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
