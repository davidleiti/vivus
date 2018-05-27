namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
    using System;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.ViewModels;

    /// <summary>
    /// Interaction logic for DonationResultsPage.xaml
    /// </summary>
    public partial class DonationsResultsPage : BasePage<DonationsResultsViewModel>, IPage
    {
        public DonationsResultsPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void AllowErrors()
        {
            tbDonationDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbDonationResults.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
        }

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void DontAllowErrors()
        {
            tbDonationDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbDonationResults.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
        }

        /// <summary>
        /// Allows only the optional errors to be displayed.
        /// </summary>
        public void AllowOptionalErrors()
        {
        }
    }
}
