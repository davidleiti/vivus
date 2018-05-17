namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;
    /// <summary>
    /// Represents a view model for the patient details page.
    /// </summary>
    public class PatientDetailsViewModel : BaseViewModel
    {
        #region Private properties
        private string bloodType;
        private string rhType;
        private string status;
        #endregion

        #region Public properties

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

        public string RhType
        {
            get => rhType;

            set
            {
                if (rhType == value)
                    return;
                rhType = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status
        {
            get => status;

            set
            {
                if (status == value)
                    return;

                status = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the person view model.
        /// </summary>
        public PersonViewModel Person { get;  }

        /// <summary>
        /// Gets the identification card address view model.
        /// </summary>
        public AddressViewModel IdentificationCardAddress { get;  }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets the list of blood types.
        /// </summary>
        public List<BasicEntity<string>> BloodTypes { get;  }

        /// <summary>
        /// Gets the list of RHs.
        /// </summary>
        public List<BasicEntity<string>> RhTypes { get;  }

        public ICommand AddCommand { get;  }

        #endregion

        #region Constructors

        public PatientDetailsViewModel()
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            BloodTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select blood type") };
            RhTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select RH type") };
            AddCommand = new RelayCommand(Add);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Counties.Add(new BasicEntity<string>(1, "C1"));
                Counties.Add(new BasicEntity<string>(2, "C2"));
                Counties.Add(new BasicEntity<string>(3, "C3"));
                
                Person.Gender = new BasicEntity<string>(1, "Male");
                Person.NationalIdentificationNumber = "1234";
                Person.BirthDate = "bd";

                IdentificationCardAddress.City = "Bucuresti";
                RhType = "1";
            });
        }

        #endregion

        #region Private Methods

        private void Add() {
            if (Errors + Person.Errors + IdentificationCardAddress.Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            VivusConsole.WriteLine("Doctor: Add successfull");
            Popup("Successfull operation!", PopupType.Successful);
        }

        #endregion

    }
}
