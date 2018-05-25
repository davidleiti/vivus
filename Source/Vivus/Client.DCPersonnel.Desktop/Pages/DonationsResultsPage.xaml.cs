namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
    using System;
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
        }

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void AllowErrors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void DontAllowErrors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows only the optional errors to be displayed.
        /// </summary>
        public void AllowOptionalErrors()
        {
            throw new NotImplementedException();
        }
    }
}
