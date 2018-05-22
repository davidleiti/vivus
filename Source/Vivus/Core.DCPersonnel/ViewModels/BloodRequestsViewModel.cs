namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.Validators;
    using Vivus.Core.ViewModels;
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

        private int? currentThrombocytes;
        private int? currentPlasma;
        private int? currentRedCells;
        private int? currentBlood;
        private string priority;
        private string bloodType;
        private BasicEntity<string> containerType;
        private BasicEntity<string> containerCode;
        private DateTime harvestDate;
        private BasicEntity<string> donationCenter;

        private List<BasicEntity<string>> containerTypes;
        private List<BasicEntity<string>> containerCodes;
        private List<BasicEntity<string>> donationCenters;

        private ObservableCollection<RequestDetailsItem> requestDetailsItems;
        private ObservableCollection<AllRequestsItem> allRequestsItems;
        #endregion

        #region Public properties
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


        public int? CurrentThrombocytes
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
        public int? CurrentPlasma
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
        public int? CurrentRedCells
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
        public int? CurrentBlood
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

                OnPropertyChanged();
            }
        }
        private DateTime HarvestDate
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

                allRequestsItems= value;

                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }

        public ICommand RemoveCommand { get; }

        public ICommand RedirectCommand { get; }

        #endregion

        #region Constructors
        public BloodRequestsViewModel()
        {
            ContainerTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select container type") };
            // ContainerTypes.Add(new BasicEntity<string>(11, "value"));
            //ContainerTypes = new List<BasicEntity<string>> { };
            ContainerCodes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select container code") };
            //ContainerCodes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select container code") };
            DonationCenters = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select donation center") };
            AddCommand = new RelayCommand(addRequest);
            RemoveCommand = new RelayCommand(removeRequest);
            RedirectCommand = new RelayCommand(redirectRequest);
            RequestDetailsItems = new ObservableCollection<RequestDetailsItem> { new RequestDetailsItem() };
            AllRequestsItems = new ObservableCollection<AllRequestsItem> { new AllRequestsItem() };


        }
        #endregion

        #region Public Methods
        public void addRequest()
        {
            ParentPage.AllowErrors();
            if (Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");

                return;
            }

            Vivus.Console.WriteLine("DCPersonnel BloodRequest: Request added!");
            Popup("Successfull operation!", PopupType.Successful);
        }
        public void removeRequest()
        {
            ParentPage.AllowErrors();
            if (Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("DCPersonnel BloodRequest: Request removed!");
            Popup("Successfull operation!", PopupType.Successful);
        }
        public void redirectRequest()
        {
            ParentPage.AllowErrors();
            if (Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("DCPersonnel BloodRequest: Request redirected!");
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
        private int? thrombocytes;
        private int? redCells;
        private int? plasma;
        private int? blood;
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
        public int? Thrombocytes
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
        public int? Plasma
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
        public int? RedCells
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
        public int? Blood
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
            Thrombocytes = 10;
            Priority = "high";
            redCells = 11;
            Plasma = 12;
            Blood = 13;
            BloodType = "rhneg";
        }
       
        #endregion
    }

}
