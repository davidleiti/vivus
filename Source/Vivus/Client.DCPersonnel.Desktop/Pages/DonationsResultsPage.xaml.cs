namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
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
            cbBloodType.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            cbRH.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbDonationDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbDonationResults.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
        }

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void DontAllowErrors()
        {
            cbBloodType.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
            cbRH.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
            tbDonationDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            tbDonationResults.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
        }

        /// <summary>
        /// Allows only the optional errors to be displayed.
        /// </summary>
        public void AllowOptionalErrors()
        {
        }

        /// <summary>
        /// Closes the window the page is part of.
        /// </summary>
        public void Close()
        {
        }
    }
}
