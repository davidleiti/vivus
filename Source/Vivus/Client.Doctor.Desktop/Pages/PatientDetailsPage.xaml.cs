namespace Vivus.Client.Doctor.Desktop.Pages
{
    using System.Windows;
    using Vivus.Client.Core.AttachedProperties;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.ViewModels;

    /// <summary>
    /// Interaction logic for PatientDetailsPage.xaml
    /// </summary>
    public partial class PatientDetailsPage : BasePage<PatientDetailsViewModel>, IPage
    {
        public PatientDetailsPage()
        {
            InitializeComponent();

            ViewModel.ParentPage = this;
        }

        /// <summary>
        /// Allows the errors to be displayed.
        /// </summary>
        public void AllowErrors()
        {
            // Patient details
            tbFirstName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbLastName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbBirthDate.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbNin.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbPhoneNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

            // Address
            tbStreetName.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbStreetNumber.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            tbCity.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);
            cbCounty.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            tbZipCode.SetValue(TextBoxExtensions.ShowErrorTemplateProperty, true);

            // Medical data
            cbBloodType.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
            cbRh.SetValue(ComboBoxExtensions.ShowErrorTemplateProperty, true);
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
        /// Closes the window the page is part of.
        /// </summary>
        public void Close()
        {
            Window.GetWindow(this).Close();
        }
    }
}
