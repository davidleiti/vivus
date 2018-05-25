namespace Vivus.Core.DCPersonnel.ViewModels
{
    using Vivus.Core.ViewModels;
    using Vivus.Core.DataModels;
    using System;
    using System.Windows.Input;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Vivus = Console;
    using Vivus.Core.DCPersonnel.Validators;

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
        private bool approved;
        private BloodDonationRequestItem selectedBloodDonationRequestItem;
        private ObservableCollection<BloodDonationRequestItem> bloodDonationRequestItems;

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
        public bool Approved
        {
            get => approved;

            set
            {
                if (approved == value)

                    return;

                approved = value;

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
        public BloodDonationRequestsViewModel()
        {
            /*fullName = "yay";
            nin="yoooooooooo";
            age=10;
            weight=10;
            heartRate=10;
            systolicBP=10;
            diastolicBP=10;
            applyDate=new DateTime(2018,11,11);
            pastSurgeries="none";
            messages="noooo";
            approved=true;*/
            ApproveCommand = new RelayCommand(() => { approved = true; CheckMessages(); });
            DenyCommand = new RelayCommand(() => { approved = false; CheckMessages(); });
            SelectedBloodDonationRequestItem = new BloodDonationRequestItem();
            BloodDonationRequestItems = new ObservableCollection<BloodDonationRequestItem>();
            BloodDonationRequestItems.Add(new BloodDonationRequestItem());


        }
        #endregion
        #region Private Methods
        private void CheckMessages()
        {
            Parentpage.AllowErrors();
            if (Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }
            Vivus.Console.WriteLine("DCPersonnel approval/denial successful");
            Popup("Successfull operation!", PopupType.Successful);

        }
        #endregion
    }
    public class BloodDonationRequestItem : BaseViewModel
    {
        #region Private members
        private int id;
        private DateTime applyDate;
        private string nationalIdentificationNumber;
        private string fullName;
        private int age;
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
        public override string this[string propertyName]
        {
            get
            {
                return null;
            }
        }
        #endregion

        public BloodDonationRequestItem()
        {
            Id = 10;
            ApplyDate = new DateTime(2012, 11, 1);
            NationalIdentificationNumber = "123";
            FullName = "Sinklars Draptinen";
            Age = 11;

        }
    }
}
