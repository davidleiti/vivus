namespace Vivus.Client.Administration.Desktop.Pages
{
    using System.Security;
    using System.Windows.Controls;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Administration.ViewModels;
    using Vivus.Client.Core.AttachedProperties;

    /// <summary>
    /// Interaction logic for DonationCentersPage.xaml
    /// </summary>
    public partial class DonationCentersPage : BasePage<DonationCentersViewModel>
    {
        public DonationCentersPage()
        {
            InitializeComponent();
        }

        public void AllowErrors()
        {
            tbDonationCenterName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbStreet.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbCounty.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
        }
    }
}
