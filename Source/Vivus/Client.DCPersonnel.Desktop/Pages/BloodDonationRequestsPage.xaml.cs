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
            Console.WriteLine("IMPLEMENT DON'T ALLOW ERRORS!");
            //throw new System.NotImplementedException();
        }

        public void AllowOptionalErrors()
        {
            //throw new System.NotImplementedException();
            Console.WriteLine("IMPLEMENT ALLOW OPTIONAL ERRORS!");
        }
    }
}
