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
    /// Represents a view model for the donation center personnel page.
    /// </summary>
    public class DCPersonnelViewModel : BaseViewModel
    {
        #region Private Members

        private string email;
        private object password;

        #endregion

        #region Public Properties

        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the email address of the DCPersonnel.
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
        /// Gets or sets the binding property for the password.
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
        /// Gets the list of donation centers.
        /// </summary>
        public List<BasicEntity<string>> DonationCenters { get; }

        public BasicEntity<string> SelectedDonationCenter { get; set; }

        public bool Active { get; set; }


        /// <summary>
        /// Gets the person view model.
        /// </summary>
        public PersonViewModel Person { get; }

        /// <summary>
        /// Gets the address
        /// </summary>
        public AddressViewModel Address { get; }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets the add command.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Gets the collection of DCPersonnel.
        /// </summary>
        public ObservableCollection<DCPItemViewModel> Items { get; }


        /// <summary>
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
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
        /// Initializes a new instance of the <see cref="DCPersonnelViewModel"/> class with the default values.
        /// </summary>
        public DCPersonnelViewModel()
        {
            //email = "ie";

            DonationCenters = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select donation center") };
            Active = true;

            Person = new PersonViewModel();
            Address = new AddressViewModel();
            AddCommand = new RelayCommand(Add);

            Items = new ObservableCollection<DCPItemViewModel>();

            // Test whether the binding was done right or not
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new DCPItemViewModel
                {
                    Id = 1,
                    Name = "Alex",
                    Nin = "12345",
                    DonationCenter = "Fsega"
                });
            });
            
           
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add a DCPersonnel.
        /// </summary>
        private void Add()
        {
            ParentPage.AllowErrors();

            if (Errors + Person.Errors + Address.Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("DCPersonnel: Registration worked!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        #endregion
    }

    public class DCPItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private string name;
        private string nin;
        private string donation_center;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identificator.
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
        /// Gets or sets the name.
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
        /// Gets or sets the nin.
        /// </summary>
        public string Nin
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
        /// Gets or sets the donation center.
        /// </summary>
        public string DonationCenter
        {
            get => donation_center;

            set
            {
                if (donation_center == value)
                    return;

                donation_center = value;

                OnPropertyChanged();
            }
        }

        #endregion

    }
}