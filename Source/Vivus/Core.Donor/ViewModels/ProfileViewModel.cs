namespace Vivus.Core.Donor.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.Validators;
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
        private bool updateIsRunning;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent page of the current <see cref="SignUpViewModel"/>.
        /// </summary>
        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the email address of the donor.
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
        public AddressViewModel ResidenceAddress { get; }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets or sets the flag that indicates whether the upadte command is running or not.
        /// </summary>
        public bool UpdateIsRunning
        {
            get => updateIsRunning;

            set
            {
                if (updateIsRunning == value)
                    return;

                updateIsRunning = value;

                OnPropertyChanged();
            }
        }

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
                    return GetErrorString(propertyName, DonorValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetErrorString(propertyName, DonorValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the update command.
        /// </summary>
        public ICommand UpdateCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class with the default values.
        /// </summary>
        public ProfileViewModel()
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            ResidenceAddress = new AddressViewModel(false);
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };

            UpdateCommand = new RelayCommand(UpdateAsync);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers a donor.
        /// </summary>
        private async void UpdateAsync()
        {
            await RunCommand(() => UpdateIsRunning, async () =>
            {
                ParentPage.AllowErrors();

                if (Errors + Person.Errors + IdentificationCardAddress.Errors + ResidenceAddress.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                await Task.Delay(3000);

                VivusConsole.WriteLine("Profile update done!");
                Popup("Successfull operation!", PopupType.Successful);
            });
        }

        #endregion
    }
}
