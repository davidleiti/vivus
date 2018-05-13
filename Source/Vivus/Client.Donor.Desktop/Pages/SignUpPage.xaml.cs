﻿namespace Vivus.Client.Donor.Desktop.Pages
{
    using System.Security;
    using System.Windows.Controls;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.ViewModels;

    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : BasePage<SignUpViewModel>, IPage, IContainPassword
    {
        public SignUpPage()
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
    }
}