namespace Vivus.Core.Donor.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.Donor.Validators;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a view model for the sign up page.
    /// </summary>
    public class SignUpViewModel : BaseViewModel
    {
        #region Private Members

        private string email;
        private object password;
        private bool registerIsRunning;
        private bool loginIsRunning;

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
        /// Gets or sets the flag that indicates whether the register command is running or not.
        /// </summary>
        public bool RegisterIsRunning
        {
            get => registerIsRunning;

            set
            {
                if (registerIsRunning == value)
                    return;

                registerIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether the login command is running or not.
        /// </summary>
        public bool LoginIsRunning
        {
            get => loginIsRunning;

            set
            {
                if (loginIsRunning == value)
                    return;

                loginIsRunning = value;

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
        /// Gets the register command.
        /// </summary>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// Gets the login command.
        /// </summary>
        public ICommand LoginCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpViewModel"/> class with the default values.
        /// </summary>
        public SignUpViewModel()
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            ResidenceAddress = new AddressViewModel(false);
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };

            RegisterCommand = new RelayCommand(RegisterAsync);
            LoginCommand = new RelayCommand(LoginAsync);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers a donor.
        /// </summary>
        private async void RegisterAsync()
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                ParentPage.AllowErrors();

                if (Errors + Person.Errors + IdentificationCardAddress.Errors + ResidenceAddress.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                await Task.Delay(3000);

                VivusConsole.WriteLine("Registration done!");
                Popup("Successfull operation!", PopupType.Successful);
            });
        }

        /// <summary>
        /// Goes back to the login page.
        /// </summary>
        private async void LoginAsync()
        {
            await RunCommand(() => LoginIsRunning, async () =>
            {
                await Task.Run(() =>
                {
                    // This time, on load, animate the login page
                    IoCContainer.Get<WindowViewModel>().OnLoadAnimateLoginPage = true;

                    IoCContainer.Get<WindowViewModel>().CurrentPage = DataModels.ApplicationPage.Login;
                    IoCContainer.Get<WindowViewModel>().HideMenu();
                    VivusConsole.WriteLine("Moved to the login page.");
                });
            });
        }

        #endregion
    }
}
