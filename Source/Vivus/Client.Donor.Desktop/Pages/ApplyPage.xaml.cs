namespace Vivus.Client.Donor.Desktop.Pages
{
    using System.Windows.Controls;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.ViewModels;

    /// <summary>
    /// Interaction logic for ApplyPage.xaml
    /// </summary>
    public partial class ApplyPage : BasePage<ApplyViewModel>, IPage
    {
        public ApplyPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void AllowErrors()
        {
            tbWeight.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbHeartRate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbSystolicBP.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbDiastolicBP.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbPastSurgeries.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbTravelStatus.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
        }

        /// <summary>
        /// Resets the errors allow status.
        /// </summary>
        public void DontAllowErrors()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Allows only the optional errors to be displayed.
        /// </summary>
        public void AllowOptionalErrors()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Raisen when a disease checkbox is checked.
        /// </summary>
        /// <param name="sender">A disease checkbox.</param>
        /// <param name="e">The event arguments.</param>
        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Diseases.Add((sender as CheckBox).Content.ToString());
        }

        /// <summary>
        /// Raisen when a disease checkbox is unchecked.
        /// </summary>
        /// <param name="sender">A disease checkbox.</param>
        /// <param name="e">The event arguments.</param>
        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Diseases.Remove((sender as CheckBox).Content.ToString());
        }
    }
}
