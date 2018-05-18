namespace Vivus.Client.Donor.Desktop.Pages
{
    using System.Security;
    using System.Threading.Tasks;
    using Vivus.Client.Core.Animations;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.Donor.ViewModels;

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
        /// Gets the password of the donor.
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
    }
}
