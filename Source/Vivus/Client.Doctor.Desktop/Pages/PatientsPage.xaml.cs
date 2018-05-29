namespace Vivus.Client.Doctor.Desktop.Pages
{
    using System;
    using System.Windows.Controls;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.Doctor.ViewModels;

    /// <summary>
    /// Interaction logic for PatientsPage.xaml
    /// </summary>
    public partial class PatientsPage : BasePage<PatientsViewModel>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsPage"/> class with the given value.
        /// </summary>
        /// <param name="patientDetails">The popup window that contains the patient details.</param>
        public PatientsPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Handlers

        /// <summary>
        /// Raised when the user click on the new patient button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void NewPatientButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.NewPatientCommand.Execute(new Action(LoadPopup));
        }

        /// <summary>
        /// Raised when the user double clicks on a record inside the all patients table.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ModifyPatientPreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.LastSelectedPatient = (sender as DataGrid).SelectedItem as PatientItemViewModel;
            ViewModel.ModifyPatientCommand.Execute(new Action(LoadPopup));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the popup.
        /// </summary>
        private void LoadPopup()
        {
            PopupWindow popupWindow;

            popupWindow = new PopupWindow(new WindowViewModel { CurrentPage = ApplicationPage.PatientDetails });
            (popupWindow as IPopup).Owner = IoCContainer.Get<WindowViewModel>().Owner;
            ViewModel.PatientDetailsPopup = popupWindow;
        }

        #endregion
    }
}
