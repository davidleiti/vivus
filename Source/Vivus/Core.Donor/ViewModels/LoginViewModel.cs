namespace Vivus.Core.Donor.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;
    using BCrypt.Net;
    using System.Windows;
    using Vivus.Core.Security;
    using Vivus.Core.ViewModels.Base;

    /// <summary>
    /// Represents a view model for the login page.
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private bool loginIsRunning;
        private bool registerIsRunning;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent page of the current <see cref="LoginViewModel"/>.
        /// </summary>
        public IContainPassword ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = "hutanu.adina@hotmail.com";

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

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the login command.
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Gets or sets the register command.
        /// </summary>
        public ICommand RegisterCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class with the default values.
        /// </summary>
        public LoginViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            RegisterCommand = new RelayCommand(async () => await RegisterAsync());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Attempts to log the user in.
        /// </summary>
        /// <param name="password">The <see cref="SecureString"/> passwd in from the view for the user's password.</param>
        /// <returns></returns>
        private async Task LoginAsync()
        {
            await RunCommand(() => LoginIsRunning, async () =>
            {
                if (string.IsNullOrEmpty(Email) && ParentPage.SecurePasword.Length == 0)
                {
                    Popup("Both fields are mandatory.");
                    return;
                }

                if (string.IsNullOrEmpty(Email))
                {
                    Popup("Email address field is mandatory.");
                    return;
                }

                if (ParentPage.SecurePasword.Length == 0)
                {
                    Popup("Password field is mandatory.");
                    return;
                }

                await Task.Run(() =>
                {
                    try
                    {
                        Model.Donor donor = IoCContainer.Get<IUnitOfWork>().Donors.Entities.First(d => d.Account.Email == Email);

                        if (!BCrypt.Verify(ParentPage.SecurePasword.Unsecure(), donor.Account.Password))
                        {
                            Popup("Invalid email or password.");
                            VivusConsole.WriteLine("Invalid username of password.");
                            return;
                        }

                        IoCContainer.Get<IApllicationViewModel<Model.Donor>>().User = new Model.Donor
                        {
                            PersonID = donor.PersonID,
                            AccountID = donor.AccountID,
                            DonationCenterID = donor.DonationCenterID,
                            ResidenceID = donor.ResidenceID
                        };

                        IoCContainer.Get<WindowViewModel>().OnUnloadAnimateLoginPage = false;
                        IoCContainer.Get<WindowViewModel>().CurrentPage = DataModels.ApplicationPage.Dashboard;
                        IoCContainer.Get<WindowViewModel>().ShowMenu();

                        IoCContainer.Get<WindowViewModel>().SideMenuVisibility = Visibility.Visible;

                        VivusConsole.WriteLine($"Welcome, { Email }!");
                    }
                    catch
                    {
                        Popup("Invalid email or password.");
                        VivusConsole.WriteLine("No user found.");
                    }
                });
            });
        }

        /// <summary>
        /// Moves on to the register page.
        /// </summary>
        /// <returns></returns>
        private async Task RegisterAsync()
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                await Task.Run(() =>
                {
                    IoCContainer.Get<WindowViewModel>().CurrentPage = DataModels.ApplicationPage.Register;
                    VivusConsole.WriteLine("Moved to the register page.");
                });
            });
        }

        #endregion
    }
}
