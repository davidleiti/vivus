namespace Vivus.Core.DCPersonnel.ViewModels
{
    using Vivus.Core.ViewModels;
    using Vivus.Core.DataModels;
    using System.Windows.Input;
    using Vivus.Core.DCPersonnel.Validators;
    using Vivus = Console;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a view model for the profile page.
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
        #region Private members

        private string email;
        private object password;

        #endregion

        #region Public Properties

        public IPage ParentPage { get; set; }

        /*
         * Gets or sets the email address of the dc's employee
         */
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

        /*
         * Gets or sets the password of the dc's employee
         */
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

        // Gets the person view model
        public PersonViewModel Person { get; set; }

        // Gets the identification card adress view model
        public AddressViewModel IdentificationCardAddress { get; }

        // Gets the list of counties
        public List<BasicEntity<string>> Counties { get; }

        // Gets the update command
        public ICommand UpdateCommand { get; }

        /*
         * Gets the error string of a property
         * param Name: The name of the property
         */
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Email))
                    return GetErrorString(propertyName, DCPersonnelValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetErrorString(propertyName, DCPersonnelValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpViewModel"/> class with the default values.
        /// </summary>
        public ProfileViewModel()
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            UpdateCommand = new RelayCommand(Update);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the profile of a dc's employee
        /// </summary>
        private void Update()
        {
            ParentPage.AllowErrors();

            if (Errors + Person.Errors + IdentificationCardAddress.Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("DC Personnel Profile: Update worked!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        #endregion
    }
}
