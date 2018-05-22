namespace Vivus.Client.Doctor.Desktop.Pages
{
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.ViewModels;

    /// <summary>
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : BasePage<NotificationsViewModel>, IPage
    {
        public NotificationsPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;

        }

        public void AllowErrors()
        {
            cbPersonTypes.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            cbPersonNames.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbMessage.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

        }

        public void DontAllowErrors()
        {
            throw new System.NotImplementedException();
        }

        public void AllowOptionalErrors()
        {
            throw new System.NotImplementedException();
        }
    }
}
