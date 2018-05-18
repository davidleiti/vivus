namespace Vivus.Client.Doctor.Desktop.ValueConverters
{
    using System;
    using System.Globalization;
    using Vivus.Client.Core.ValueConverters;
    using VivusConsole = Vivus.Core.Console.Console;
    using Vivus.Core.Doctor.DataModels;
    using System.Diagnostics;
    using Vivus.Client.Doctor.Desktop.Pages;

    /// <summary>
    /// Represents a converter that takes in an <see cref="ApplicationPage"/> and returns a <see cref="BasePage"/>.
    /// </summary>
    internal class ApplicationPageToPageValueConverter : BaseValueConverter<ApplicationPageToPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Login:
                    return new LoginPage();

                case ApplicationPage.Patients:
                    return new PatientsPage();

                case ApplicationPage.PatientDetails:
                    return new PatientDetailsPage();

                case ApplicationPage.Requests:
                    return new RequestPage();

                case ApplicationPage.ManageBlood:
                    return new ManageBloodPage();

                case ApplicationPage.Profile:
                    return new ProfilePage();

                case ApplicationPage.Notifications:
                    return new NotificationsPage();

                default:
                    VivusConsole.WriteLine($"Application page conversion impossible. Value: { value }");
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // No need to convert back
            throw new NotImplementedException();
        }
    }
}
