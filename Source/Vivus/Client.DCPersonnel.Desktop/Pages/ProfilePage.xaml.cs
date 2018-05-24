namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DCPersonnel.ViewModels;
    using System.Security;
    using System.Windows.Controls;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : BasePage<ProfileViewModel>, IPage, IContainPassword
    {
        public ProfilePage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        public SecureString SecurePasword => pbPassword.SecurePassword;

        public void AllowErrors() {
            // Account details
            tbEmail.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            pbPassword.GetBindingExpression(CacheModeProperty).UpdateTarget();
            pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, true);

            // Person details
            tbFirstName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbLastName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbBirthDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbNin.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbPhoneNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

            // National identification card address
            tbIdentifCardStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbIdentifCardStreetNo.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbIdentifCardCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbIdentifCardCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbIdentifCardZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
        }

        public void DontAllowErrors()
        {
            throw new System.NotImplementedException();
        }

        public void AllowOptionalErrors()
        {
            pbPassword.GetBindingExpression(CacheModeProperty).UpdateTarget();
            if (SecurePasword.Length == 0)
                pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, false);
            else
                pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, true);
        }
    }
}
