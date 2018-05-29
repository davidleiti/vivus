namespace Vivus.Client.Donor.Desktop.Pages
{
    using System.Security;
    using System.Windows.Controls;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.ViewModels;

    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : BasePage<ProfileViewModel>, IPage, IContainPassword
    {
        public ProfilePage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;

            // Set the optional address
            //  Street name
            tbResidenceStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbResidenceStreetName.LostFocus += delegate { tbResidenceStreetName.GetBindingExpression(TextBox.TextProperty).UpdateTarget(); };
            //  Street number
            tbResidenceStreetNo.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbResidenceStreetNo.LostFocus += delegate { tbResidenceStreetNo.GetBindingExpression(TextBox.TextProperty).UpdateTarget(); };
            //  City
            tbResidenceCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbResidenceCity.LostFocus += delegate { tbResidenceCity.GetBindingExpression(TextBox.TextProperty).UpdateTarget(); };
            //  County
            cbResidenceCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            cbResidenceCounty.LostFocus += delegate { cbResidenceCounty.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateTarget(); };
            //  Zip code
            tbResidenceZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbResidenceZipCode.LostFocus += delegate { tbResidenceZipCode.GetBindingExpression(TextBox.TextProperty).UpdateTarget(); };
        }

        /// <summary>
        /// Stores a reference to the secured memory location of the password.
        /// </summary>
        public SecureString SecurePasword => pbPassword.SecurePassword;

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void AllowErrors()
        {
            // Account details
            tbEmail.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            pbPassword.GetBindingExpression(CacheModeProperty).UpdateTarget();
            pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, true);
            tbEmail.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbFavourieDonationCenter.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);

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

            // Residence address
            tbResidenceStreetName.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            tbResidenceStreetNo.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            tbResidenceCity.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            cbResidenceCounty.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateTarget();
            tbResidenceZipCode.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
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
            pbPassword.GetBindingExpression(CacheModeProperty).UpdateTarget();
            if (SecurePasword.Length == 0)
                pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, false);
            else
                pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, true);
        }

        /// <summary>
        /// Closes the window the page is part of.
        /// </summary>
        public void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
