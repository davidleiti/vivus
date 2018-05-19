namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.Validators;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a view model for the profile page.
    /// </summary>

    public class ProfileViewModel : BaseViewModel
    {
        #region Private Members

        private string email;
        private object password;

        #endregion

        #region Public Properties

        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the email address of the doctor.
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
        /// Gets the person view model.
        /// </summary>
        public PersonViewModel Person { get; }

        /// <summary>
        /// Gets the identification card address view model.
        /// </summary>
        public AddressViewModel IdentificationCardAddress { get; }

        /// <summary>
        /// Gets the residence address view model.
        /// </summary>
        public AddressViewModel WorkAddress { get; }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets the register command.
        /// </summary>
        public ICommand UpdateCommand { get; }

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
                    return GetErrorString(propertyName, DoctorValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetErrorString(propertyName, DoctorValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class with the default values.
        /// </summary>
        public ProfileViewModel()
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            WorkAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            UpdateCommand = new RelayCommand(Update);

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates a doctor.
        /// </summary>
        private void Update()
        {
            ParentPage.AllowErrors();

            if (Errors + Person.Errors + IdentificationCardAddress.Errors + WorkAddress.Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            VivusConsole.WriteLine("Doctor: Update worked!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        #endregion
    }
}
