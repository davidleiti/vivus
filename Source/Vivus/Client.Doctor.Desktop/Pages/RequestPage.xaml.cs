namespace Vivus.Client.Doctor.Desktop.Pages
{
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.ViewModels;

    /// <summary>
    /// Interaction logic for RequestPage.xaml
    /// </summary>
    public partial class RequestPage : BasePage<RequestViewModel>, IPage
    {
        public RequestPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        public void AllowErrors()
        {
            cbSelectedPatientName.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            cbSelectedPriority.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbRequestPlasma.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbRequestRedCells.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbRequestThrombocytes.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
        }
    }
}
