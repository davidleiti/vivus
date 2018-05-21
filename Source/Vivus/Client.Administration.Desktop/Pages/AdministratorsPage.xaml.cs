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
		}
	}
}
