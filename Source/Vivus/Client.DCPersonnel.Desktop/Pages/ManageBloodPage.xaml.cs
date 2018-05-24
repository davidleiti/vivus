namespace Vivus.Client.DCPersonnel.Desktop.Pages
{
    using Vivus.Client.Core.Pages;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Core.DCPersonnel.ViewModels;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Interaction logic for ManageBloodPage.xaml
    /// </summary>
    public partial class ManageBloodPage : BasePage<ManageBloodViewModel>, IPage
    {
        public ManageBloodPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        public void AllowErrors()
        {
            // Adding new container details
            if (ViewModel.ToValidate == ManageBloodViewModel.Validation.ManageBlood)
            {
                cbContainerType.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
                tbContainerCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
                cbBloodTypeManage.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
                cbRHManage.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
                tbHarvestDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            }

            // Request blood details
            if (ViewModel.ToValidate == ManageBloodViewModel.Validation.RequestDonation)
            {
                cbBloodTypeReq.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
                cbRHReq.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            }
        }

        public void DontAllowErrors()
        {
            if (ViewModel.ToValidate == ManageBloodViewModel.Validation.ManageBlood)
            {
                cbContainerType.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
                tbContainerCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
                cbBloodTypeManage.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
                cbRHManage.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
                tbHarvestDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, false);
            }

            // Request blood details
            if (ViewModel.ToValidate == ManageBloodViewModel.Validation.RequestDonation)
            {
                cbBloodTypeReq.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
                cbRHReq.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, false);
            }

            ViewModel.ToValidate = ManageBloodViewModel.Validation.None;
        }

        public void AllowOptionalErrors()
        {
            throw new System.NotImplementedException();
        }
    }
}
