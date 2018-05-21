namespace Vivus.Client.Administration.Desktop.Pages
{
	using System.Security;
	using Vivus.Client.Core.AttachedProperties;
	using Vivus.Client.Core.Pages;
    using Vivus.Core.Administration.ViewModels;
	using Vivus.Core.DataModels;

	/// <summary>
	/// Interaction logic for AdministratorsPage.xaml
	/// </summary>
	public partial class AdministratorsPage : BasePage<AdministratorsViewModel>, IPage, IContainPassword
    {
        public AdministratorsPage()
        {
            InitializeComponent();

			ViewModel.ParentPage = this;
		}

		public SecureString SecurePasword => pbPassword.SecurePassword;

		public void AllowErrors() {
			tbEmail.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			pbPassword.GetBindingExpression(CacheModeProperty).UpdateTarget();
			pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, true);

			tbFirstName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			tbLastName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			tbBirthDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			tbNin.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			tbPhoneNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

			tbStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			tbStreetNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			tbCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			cbCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
			tbZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
		}

        /// <summary>
        /// Resets the errors allow status.
        /// </summary>
        public void DontAllowErrors()
        {
            tbEmail.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            pbPassword.GetBindingExpression(CacheModeProperty).UpdateTarget();
            pbPassword.SetValue(PasswordBoxExtensions.ShowErrorTemplateProperty, false);

            tbFirstName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbLastName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbBirthDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbNin.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbPhoneNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);

            tbStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbStreetNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            cbCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
            tbZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
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
    }
}
