namespace Vivus.Core.Administration.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the donation centers page.
    /// </summary>
    public class DonationCentersViewModel : BaseViewModel
    {
        #region Private Members

        private string donationCenterName;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of donation center
        /// </summary>
        public string DonationCenterName
        {
            get => donationCenterName;

            set
            {
                if (donationCenterName == value)
                    return;

                donationCenterName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the donation center address view model
        /// </summary>
        public AddressViewModel ResidencyAddress { get; }


        /// <summary>
        /// Gets the list of counties
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }


        /// <summary>
        /// Command for adding the new donation center
        /// </summary>
        public ICommand Add { get;  }

        /// <summary>
        /// Command forgetting donation centers
        /// </summary>
        public ObservableCollection<DonationCenterItemViewModel> DonationCenters { get; }

        #endregion

        #region Constructors

        public DonationCentersViewModel()
        {
            ResidencyAddress = new AddressViewModel();
            DonationCenters = new ObservableCollection<DonationCenterItemViewModel>();

            // TODO add entity for test

        }

        #endregion

        #region Private methods

        private void AddDonationCenter()
        {
            if (Errors + ResidencyAddress.Errors > 0)
            {
                Popup("Some errors here ");
                return;
            }

            Vivus.Console.WriteLine("Adminstration: Add Dodation Center works!");
            Popup("Successful operation!", PopupType);
        }

        #endregion


        public class DonationCenterItemViewModel: BaseViewModel
        {
            #region Private members

            private string donationCenterName;
            private string residencyAddress;

            #endregion

            #region Public Properties

            public string DonationCenterNAme
            {
                get => donationCenterName;

                set
                {
                    if (donationCenterName == value)
                        return;

                    donationCenterName = value;

                    OnPropertyChanged();
                }
            }

            public string ResidencyAddress
            {
                get => residencyAddress;

                set
                {
                    if (residencyAddress == value)
                        return;

                    residencyAddress = value;

                    OnPropertyChanged();
                }
            }


            #endregion
        }

    }
}
