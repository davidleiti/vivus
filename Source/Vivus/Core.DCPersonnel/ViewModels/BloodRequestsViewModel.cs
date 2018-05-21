﻿namespace Vivus.Core.DCPersonnel.ViewModels
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


        private List<BasicEntity<string>> containerTypes;
        private List<BasicEntity<string>> containerCodes;
        private List<BasicEntity<string>> donationCenters;
        private int? currentThrombocytes;
        private int? currentPlasma;
        private int? currentRedCells;
        private string priority;
        private string bloodType;
        private BasicEntity<string> containerType;
        private BasicEntity<string> containerCode;
        private DateTime harvestDate;
        private string donationCenter;
        #endregion

        #region Public properties
        public int? Thrombocytes { get; }
        public int? Plasma { get; }
        public int? RedCells { get; }

        public List<BasicEntity<string>> ContainerTypes { get; }
        public List<BasicEntity<string>> ContainerCodes { get; }
        public List<BasicEntity<string>> DonationCenters { get; }
        public int? CurrentThrombocytes { get; }
        public int? CurrentPlasma { get; }
        public int? CurrentRedCells { get; }
        public string Priority { get; }
        public string BloodType { get; }
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
        private DateTime HarvestDate { get; }
        public string DonationCenter
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
                    return GetErrorString(propertyName,DCPersonnelValidator.ContainerCodeValidation(ContainerCode));
                return null;
            }
        }
        public IPage ParentPage { get; set; }


        public ObservableCollection<RequestDetailsItem> RequestDetailsItems { get; }
        public ObservableCollection<AllRequestsItem> AllRequestsItems { get; }
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
        private string containerType;
        private string containerCode;
        private string bloodType;
        private DateTime harvestDate;
        #endregion
        #region Public Properties
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
        private string doctor;
        private int? thrombocytes;
        private int? redCells;
        private int? plasma;
        private string bloodType;

        #endregion
        #region Public Properties
        public string Doctor {
            get => doctor;
            set
            {
                if (value == doctor)
                    return;
                doctor = value;
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
            Doctor = "doc";
            Thrombocytes = 10;
            redCells = 11;
            Plasma = 12;
            BloodType = "rhneg";
        }
        #endregion

    }

}
