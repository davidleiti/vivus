﻿namespace Vivus.Client.Administration.Desktop.Pages
{
    using System;
    using System.Security;
    using System.Threading.Tasks;
    using Vivus.Client.Core.Animations;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.Administration.DataModels;
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.Administration.ViewModels;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : BasePage<LoginViewModel>, IContainPassword
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class with the default values.
        /// </summary>
        public LoginPage()
        {
            // Add page animations
            if (IoCContainer.Get<WindowViewModel>().OnLoadAnimateLoginPage)
                PageLoadAnimation = PageAnimation.SlideAndFadeInFromRight;
            PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;

            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the password of the doctor.
        /// </summary>
        public SecureString SecurePasword => pbPassword.SecurePassword;

        #endregion

        #region Public Methods

        /// <summary>
        /// Animates the current page out.
        /// </summary>
        /// <returns></returns>
        public override async Task AnimateOut()
        {
            // Make sure there is something to do
            if (!IoCContainer.Get<WindowViewModel>().OnUnloadAnimateLoginPage)
                return;

            await base.AnimateOut();
        }

        #endregion

        #region Private Handlers

        /// <summary>
        /// Handles the KeyDown event for a textbox.
        /// </summary>
        /// <param name="sender">The caller.</param>
        /// <param name="e">The event arguments.</param>
        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                ViewModel.LoginCommand.Execute(null);
        }

        /// <summary>
        /// Raised when the user clicks on the forgot password button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ForgotPasswordButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.ForgotPasswordCommand.Execute(new Action(LoadPopup));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the popup.
        /// </summary>
        private void LoadPopup()
        {
            PopupWindow popupWindow;

            popupWindow = new PopupWindow(new WindowViewModel { CurrentPage = ApplicationPage.ForgotPassword });
            (popupWindow as IPopup).Owner = IoCContainer.Get<WindowViewModel>().Owner;
            ViewModel.ForgotPasswordPopup = popupWindow;
        }

        #endregion
    }
}
