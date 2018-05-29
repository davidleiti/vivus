namespace Vivus.Client.Doctor.Desktop.Pages
{
    using System.Security;
    using System.Windows.Controls;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.ViewModels;

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

        /// <summary>
        /// Stores a reference to the secured memory location of the password.
        /// </summary>
        public SecureString SecurePasword => pbPassword.SecurePassword;

        public void AllowErrors()
        {
            //Account details
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

            //Work address
            tbWorkAddressStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbWorkAddressStreetNo.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbWorkAddressCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbWorkAddressCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbWorkAddressZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
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

        public void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
