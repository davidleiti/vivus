namespace Vivus.Client.Administration.Desktop.Pages
{
    using System.Security;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.Administration.ViewModels;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Interaction logic for DCPersonnelPage.xaml
    /// </summary>
    public partial class DCPersonnelPage : BasePage<DCPersonnelViewModel>, IPage, IContainPassword
    {
        public DCPersonnelPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;


        }

        /// <summary>
        /// Stores a reference to the secured memory location of the password.
        /// </summary>
        public SecureString SecurePasword => pbPassword.SecurePassword;

        public void AllowErrors()
        {
            // Account details
            tbEmail.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            pbPassword.GetBindingExpression(CacheModeProperty).UpdateTarget();
            pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, true);

            cbDonationCenter.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);

            // Person details
            tbFirstName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbLastName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbBirthDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbNin.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbPhoneNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

            // Address details
            tbAddressStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbAddressStreetNo.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbAddressCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbAddressCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
        }
    }
}
