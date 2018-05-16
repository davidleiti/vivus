namespace Vivus.Client.Administration.Desktop.Pages
{
    using System.Security;
    using System.Windows.Controls;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.Administration.ViewModels;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Interaction logic for DoctorsPage.xaml
    /// </summary>
    public partial class DoctorsPage : BasePage<DoctorsViewModel>, IPage, IContainPassword
    {
        public DoctorsPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        /// <summary>
        /// Stores a reference to the secured memory location of the password
        /// </summary>
        public SecureString SecurePasword => pbPassword.SecurePassword;

        /// <summary>
        /// Allows the errors to be displayed
        /// </summary>
        public void AllowErrors()
        {
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

            // Home address
            tbHomeStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbHomeStreetNo.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbHomeCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbHomeCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbHomeZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

            // Work address
            tbWorkStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbWorkStreetNo.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbWorkCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbWorkCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbWorkZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
        }
    }
}
