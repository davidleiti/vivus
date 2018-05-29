namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
    using System.Windows.Controls;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.ViewModels;
    using Vivus.Core.Console;

    /// <summary>
    /// Interaction logic for BloodDonationRequestsPage.xaml
    /// </summary>
    public partial class BloodDonationRequestsPage : BasePage<BloodDonationRequestsViewModel>,IPage
    {
        public BloodDonationRequestsPage()
        {
            InitializeComponent();
            ViewModel.Parentpage = this;
        }
        public void AllowErrors()
        {
            dcMessages.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

        }

        public void DontAllowErrors()
        {
            dcMessages.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
        }

        public void AllowOptionalErrors()
        {
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
