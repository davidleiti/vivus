namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;

    /// <summary>
    /// Represents a view model for the donation results page.
    /// </summary>
    public class DonationsResultsViewModel : BaseViewModel
    {
        #region Private Members

        private string donor;
        private string nin;
        private string donationDate;
        private string donationResults;
        private string filter;
        DonationFormItemViewModel selectedDonationForm;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the full name of the donor.
        /// </summary>
        public string Donor
        {
            get => donor;

            set
            {
                if (donor == value)
                    return;

                donor = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number of the donor.
        /// </summary>
        public string NationalIdentificationNumber
        {
            get => nin;

            set
            {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the donation date of the donor.
        /// </summary>
        public string DonationDate
        {
            get => donationDate;

            set
            {
                if (donationDate == value)
                    return;

                donationDate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the results of the donation.
        /// </summary>
        public string DonationResults
        {
            get => donationResults;

            set
            {
                if (donationResults == value)
                    return;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the filter value for the donation forms.
        /// </summary>
        public string Filter
        {
            get => filter;

            set
            {
                if (filter == value)
                    return;

                filter = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected donation form.
        /// </summary>
        public DonationFormItemViewModel SelectedDonationForm
        {
            get => selectedDonationForm;

            set
            {
                if (selectedDonationForm == value)
                    return;

                selectedDonationForm = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of the donation forms.
        /// </summary>
        public ObservableCollection<DonationFormItemViewModel> DonationForms { get; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the send command of the donation results.
        /// </summary>
        public ICommand SendCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new intance of the <see cref="DonationsResultsViewModel"/> class with the default values.
        /// </summary>
        public DonationsResultsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            DonationForms = new ObservableCollection<DonationFormItemViewModel>();

            dispatcherWrapper.InvokeAsync(() => DonationForms.Add(new DonationFormItemViewModel
            {
                PersonId = 10,
                ApplyDate = new DateTime(2018, 5, 24),
                Donor = "Bara Gabriela",
                NationalIdentificationNumber = "2840812094291",
                BloodType = "O-"
            }));
        }

        #endregion
    }

    /// <summary>
    /// Represents an item view model for the donation forms table.
    /// </summary>
    public class DonationFormItemViewModel : BaseViewModel
    {
        #region Private Members

        private int personId;
        private DateTime applyDate;
        private string donor;
        private string nin;
        private string bloodType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identificatior of the person.
        /// </summary>
        public int PersonId
        {
            get => personId;

            set
            {
                if (personId == value)
                    return;

                personId = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the apply date of the donor.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the full name of the donor.
        /// </summary>
        public string Donor
        {
            get => donor;

            set
            {
                if (donor == value)
                    return;

                donor = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number of the donor.
        /// </summary>
        public string NationalIdentificationNumber
        {
            get => nin;

            set
            {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the blood type and the rh of the donor.
        /// </summary>
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

        #endregion
    }
}
