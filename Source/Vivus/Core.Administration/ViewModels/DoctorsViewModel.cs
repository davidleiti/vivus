namespace Vivus.Core.Administration.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.Administration.Validators;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the doctors page.
    /// </summary>
    public class DoctorsViewModel : BaseViewModel 
    {
        #region Private Members

        private string email;
        private object password;
        private bool status;

        #endregion

        #region Public Properties

        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the email address of the doctor
        /// </summary>
        public string Email
        {
            get => email;

            set
            {
                if (email == value)
                    return;

                email = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the binding for the password
        /// </summary>
        public object Password
        {
            get => password;

            set
            {
                if (password == value)
                    return;

                password = value;

                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the binding for the status
        /// </summary>
        public bool Status
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
        /// Gets the person view model for a doctor
        /// </summary>
        public PersonViewModel Person { get;  }

        /// <summary>
        /// Gets the home address view model
        /// </summary>
        public AddressViewModel HomeAddress { get; }

        /// <summary>
        /// Gets the work address view model
        /// </summary>
        public AddressViewModel WorkAddress { get; }

        /// <summary>
        /// Gets the list of counties
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets the add doctor command
        /// </summary>
        public ICommand AddDoctorCommand { get; }

        /// <summary>
        /// Gets the collection of doctors
        /// </summary>
        public ObservableCollection<DoctorItemViewModel> Doctors { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Email))
                    return GetErrorString(propertyName, AdministrationValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorsViewModel"/> class with the default values
        /// </summary>
        public DoctorsViewModel()
        {
            Person = new PersonViewModel();
            HomeAddress = new AddressViewModel();
            WorkAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            AddDoctorCommand = new RelayCommand(AddDoctor);
            Doctors = new ObservableCollection<DoctorItemViewModel>();

            // Test whether the binding was done right or not
            Application.Current.Dispatcher.Invoke(() =>
            {
                Doctors.Add(new DoctorItemViewModel
                {
                    Id = 39,
                    Name = "Doctorul Smith",
                    NIN = "1234567890123",
                    HomeAddress = "Tara, Orasul, Strada strazilor, Nr 9999",
                    WorkAddress = "Tara, Orasul, Strada strazilor, Nr 9999"
                });
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add a doctor
        /// </summary>
        private void AddDoctor()
        {
            ParentPage.AllowErrors();

            if(Errors + Person.Errors + HomeAddress.Errors + WorkAddress.Errors > 0)
            {
                Popup("Some errors were found! Quick! Catch them! ");
                return;
            }

            Vivus.Console.WriteLine("Adminstration: Add Doctor works!");
            Popup("Successful operation!", PopupType.Successful);
        }

        #endregion
    }

    public class DoctorItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private string name;
        private string nin;
        private string homeAddress;
        private string workAddress;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id
        /// </summary>
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

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get => name;

            set
            {
                if (name == value)
                    return;

                name = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number
        /// </summary>
        public string NIN
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
        /// Gets or sets the home address
        /// </summary>
        public string HomeAddress
        {
            get => homeAddress;

            set
            {
                if (homeAddress == value)
                    return;

                homeAddress = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the work address
        /// </summary>
        public string WorkAddress
        {
            get => workAddress;

            set
            {
                if (workAddress == value)
                    return;

                workAddress = value;

                OnPropertyChanged();
            }
        }

        #endregion
    }
}