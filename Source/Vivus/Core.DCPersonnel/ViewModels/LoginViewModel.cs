namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.IoC;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;
    using BCrypt.Net;
    using Vivus.Core.Security;
    using Vivus.Core.ViewModels.Base;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Represents a view model for the login page.
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Private Members

        private bool loginIsRunning;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent page of the current <see cref="LoginViewModel"/>.
        /// </summary>
        public IContainPassword ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = "moldovan.dani@yahoo.com";

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

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the login command.
        /// </summary>
        public ICommand LoginCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class with the default values.
        /// </summary>
        public LoginViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync());
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
                        Model.DCPersonnel personnel = IoCContainer.Get<IUnitOfWork>().DCPersonnel.Entities.First(p => p.Account.Email == Email && p.Active);

                        if (!BCrypt.Verify(ParentPage.SecurePasword.Unsecure(), personnel.Account.Password))
                        {
                            Popup("Invalid email or password.");
                            VivusConsole.WriteLine("Invalid username of password.");
                            return;
                        }

                        IoCContainer.Get<IApplicationViewModel<Model.DCPersonnel>>().User = personnel;

                        IoCContainer.Get<WindowViewModel>().OnUnloadAnimateLoginPage = false;
                        IoCContainer.Get<WindowViewModel>().CurrentPage = DataModels.ApplicationPage.ManageBlood;
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

        #endregion
    }
}
