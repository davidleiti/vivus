namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the blood managing page.
    /// </summary>
    public class ManageBloodViewModel : BaseViewModel
    {
        #region Public properties
        public ObservableCollection<ContainersStorageItemViewModel> Items { get; }

        /// <summary>
        /// Get the dismiss command
        /// </summary>
        public ICommand DismissCommand { get; }

        public ICommand ReturnCommand { get; }

        #endregion

        #region Private methonds

        private void DismissBloodContainer()
        {
            Vivus.Console.WriteLine("Doctor: Dismiss a blood container!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        private void ReturnBloodContainer()
        {
            Vivus.Console.WriteLine("Doctor: Return a blood container!");
            Popup("Successfull operation!", PopupType.Successful);
        }


        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageBloodViewModel"/> class with the default values.
        /// </summary>
        public ManageBloodViewModel()
        {
            Items = new ObservableCollection<ContainersStorageItemViewModel>();

            DismissCommand = new RelayCommand(DismissBloodContainer);
            ReturnCommand = new RelayCommand(ReturnBloodContainer);

            // Test whether the binding was done right or not
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

        