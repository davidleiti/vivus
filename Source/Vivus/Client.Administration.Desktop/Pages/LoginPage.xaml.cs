namespace Vivus.Client.Administration.Desktop.Pages
{
    using System.Security;
    using System.Threading.Tasks;
    using Vivus.Client.Core.Animations;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.Administration.ViewModels;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : BasePage<LoginViewModel>, IContainPassword
    {
        public LoginPage()
        {
            // Add page animations
            if (IoCContainer.Get<WindowViewModel>().OnLoadAnimateLoginPage)
                PageLoadAnimation = PageAnimation.SlideAndFadeInFromRight;
            PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;

            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        /// <summary>
        /// Gets the password of the doctor.
        /// </summary>
        public SecureString SecurePasword => pbPassword.SecurePassword;

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
    }
}
