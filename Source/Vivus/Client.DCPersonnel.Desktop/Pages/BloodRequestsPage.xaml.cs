namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.ViewModels;

    /// <summary>
    /// Interaction logic for BloodRequestsPage.xaml
    /// </summary>
    public partial class BloodRequestsPage : BasePage<BloodRequestsViewModel>,IPage
    {
        public BloodRequestsPage()
        {
            InitializeComponent();
            ViewModel.ParentPage = this;
        }
        public void AllowErrors()
        {
            //todo
        }
    }
}
