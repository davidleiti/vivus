namespace Vivus.Client.Doctor.Desktop.Pages
{
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.Doctor.ViewModels;

    /// <summary>
    /// Interaction logic for PatientDetailsPage.xaml
    /// </summary>
    public partial class PatientDetailsPage : BasePage<PatientDetailsViewModel>
    {
        public PatientDetailsPage()
        {
            InitializeComponent();
        }

        public void AllowErrors()
        {
            // Patient details
            tbFirstName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbLastName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbBirthDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbNid.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbPhoneNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

            // National Identification card address
            tbStreet.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbCountry.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

            // medical blood data
            cbBloodType.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbRh.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

        }
    }
}
