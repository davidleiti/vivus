namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the blood managing page.
    /// </summary>
    public class ManageBloodViewModel : BaseViewModel
    {
        #region Private fields

        private ButtonType buttonType;
        private bool actionIsRunning;
        private ContainersStorageItemViewModel selectedItem;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Model.Doctor> appViewModel;
        private ISecurity security;
        private bool operationSuccessful;

        #endregion

        #region Public properties

        public IPage ParentPage { get; set; }

        public ObservableCollection<ContainersStorageItemViewModel> Items { get; }

        /// <summary>
        /// Get the dismiss command
        /// </summary>
        public ICommand DismissCommand { get; }

        public ICommand ReturnCommand { get; }

        public ContainersStorageItemViewModel SelectedItem
        {
            get => selectedItem;

            set
            {
                if (value == selectedItem)
                    return;
                selectedItem = value;

                if(selectedItem is null)
                {
                    // todo : is here something related to button type needed?
                    return;
                }
                else
                {
                    // todo same with button type
                    
                    dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());
                    // populate fields not needed
                }
                OnPropertyChanged();
            }
        }


        public bool ActionIsRunning
        {
            get => actionIsRunning;

            set
            {
                if (ActionIsRunning == value)
                    return;

                actionIsRunning = value;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Private methonds

        private async Task DismissBloodContainerAsync()
        {
            if (SelectedItem is null)
            {
                Popup("No container selected, before dismissing please select one.");
                return;
            }

            try
            {
                ContainersStorageItemViewModel selectedContainer = SelectedItem;
                await unitOfWork.CompleteAsync();
                Items.Remove(selectedContainer);
                Popup("Blood container dismissed successfully!", PopupType.Successful);
            } 
            catch
            {
                Popup("Unexpected error occured. Please try again later.");
            }

        }

        private async Task ReturnBloodContainerAsync()
        {
            if (SelectedItem is null)
            {
                Popup("Before returning a blood container, select one.");
                return;
            }
            try
            {
                ContainersStorageItemViewModel selectedContainer = SelectedItem;
                await unitOfWork.CompleteAsync();

                // this container does not belong to any doctor after setting the request to null
                unitOfWork.BloodContainers[selectedContainer.Id].BloodRequestID = null;
                
                Items.Remove(selectedContainer);
                // todo: remove request?
                Popup("Blood container return request sent successfully!", PopupType.Successful);
            }
            catch
            {
                Popup("Operation encountered some errors. Try again later.");
            }
        }

        private bool isExpired(string containerType, DateTime harvestDate)
        {
            if (containerType.Equals("Thrombocytes"))
                return harvestDate.AddDays(6) <= DateTime.Now;
            if (containerType.Equals("Red cells"))
                return harvestDate.AddDays(43) <= DateTime.Now;
            if (containerType.Equals("Plasma"))
                return harvestDate.AddMonths(12).AddDays(1) <= DateTime.Now;
            return harvestDate.AddDays(43) <= DateTime.Now;
        }

        private async Task LoadContainersAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => Items.Clear());
                unitOfWork.BloodContainers
                .Entities
                .ToList()
                .ForEach(bloodContainer =>
                   dispatcherWrapper.InvokeAsync(() =>
                       Items.Add(
                           new ContainersStorageItemViewModel
                           {
                               ContainerType = bloodContainer.BloodContainerType.Type,
                               ContainerCode = bloodContainer.ContainerCode,
                               BloodType = bloodContainer.BloodType.Type,
                               HarvestDate = bloodContainer.HarvestDate,
                               Expired = isExpired(bloodContainer.BloodContainerType.Type, bloodContainer.HarvestDate)           
                           }
                           )
                   )
                );
            });
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageBloodViewModel"/> class with the default values.
        /// </summary>
        public ManageBloodViewModel():base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Items = new ObservableCollection<ContainersStorageItemViewModel>();

            DismissCommand = new RelayCommand(async () => await DismissBloodContainerAsync());
            ReturnCommand = new RelayCommand(async () => await ReturnBloodContainerAsync());

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<Model.Doctor>>();
            security = IoCContainer.Get<ISecurity>();

            // Test whether the binding was done right or not

            /*
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new ContainersStorageItemViewModel
                {
                    Id = 7,
                    ContainerType = "Plasma",
                    BloodType = "0",
                    ContainerCode = "1234",
                    HarvestDate = new DateTime(2018, 2, 17),
                    Expired = false
                });
            });
            */

            Task.Run(async () =>
            {
                await LoadContainersAsync();
            });
        }

        #endregion

        #region Public Methods


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

        public override string this[string propertyName]
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}

        