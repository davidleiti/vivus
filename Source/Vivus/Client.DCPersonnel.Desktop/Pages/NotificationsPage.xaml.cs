namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
	using System.Windows.Controls;
	using Vivus.Client.Core.AttachedProperties;
	using Vivus.Client.Core.Pages;
	using Vivus.Core.DataModels;
	using Vivus.Core.DCPersonnel.ViewModels;

	/// <summary>
	/// Interaction logic for NotificationsPage.xaml
	/// </summary>
	public partial class NotificationsPage : BasePage<NotificationsViewModel>, IPage {
		public NotificationsPage() {
			InitializeComponent();

			ViewModel.ParentPage = this;

		
		}

		public void AllowErrors() {
			cbPersonTypes.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
			cbPersonName.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
			tbNin.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			tbMessage.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
			//cbPersonTypes.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
			//cbPersonName.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
			//tbNin.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
			//tbMessage.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateTarget();
		}

        public void DontAllowErrors()
        {
            throw new System.NotImplementedException();
        }

        public void AllowOptionalErrors()
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
