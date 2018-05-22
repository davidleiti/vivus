namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
    using Vivus.Client.Core.AttachedProperties;
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
            // cbContainerCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            //cbContainerType.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            if (ViewModel.ToValidate == BloodRequestsViewModel.Validation.ManageRequest)
            {
                cbContainerType.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
                cbContainerCode.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            }

            if (ViewModel.ToValidate == BloodRequestsViewModel.Validation.DonationCenterRedirect)
                cbDonationCenter.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);

            ViewModel.ToValidate = BloodRequestsViewModel.Validation.None;
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
