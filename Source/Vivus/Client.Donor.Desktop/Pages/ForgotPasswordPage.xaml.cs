namespace Vivus.Client.Donor.Desktop.Pages
{
    using System.Security;
    using System.Windows;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.ViewModels;

    /// <summary>
    /// Interaction logic for ForgotPasswordPage.xaml
    /// </summary>
    public partial class ForgotPasswordPage : BasePage<ForgotPasswordViewModel>, IPage, IContainPassword
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordPage"/> class with the default values.
        /// </summary>
        public ForgotPasswordPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Stores a reference to the secured memory location of the password.
        /// </summary>
        public SecureString SecurePasword => pbPassword.SecurePassword;

        #endregion

        #region Public Methods

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void AllowErrors()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Resets the errors allow status.
        /// </summary>
        public void DontAllowErrors()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Allows only the optional errors to be displayed.
        /// </summary>
        public void AllowOptionalErrors()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Closes the window the page is part of.
        /// </summary>
        public void Close()
        {
            Window.GetWindow(this).Close();
        }

        #endregion
    }
}
