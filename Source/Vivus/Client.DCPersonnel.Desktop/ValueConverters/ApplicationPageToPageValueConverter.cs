namespace Vivus.Client.DCPersonnel.Desktop.ValueConverters
{
    using System;
    using System.Globalization;
    using Vivus.Client.Core.ValueConverters;
    using VivusConsole = Vivus.Core.Console.Console;
    using Vivus.Core.DCPersonnel.DataModels;
    using System.Diagnostics;
    using Vivus.Client.DCPersonnel.Desktop.Pages;

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

                case ApplicationPage.ForgotPassword:
                    return new ForgotPasswordPage();

                case ApplicationPage.ManageBlood:
                    return new ManageBloodPage();

                case ApplicationPage.BloodRequests:
                    return new BloodRequestsPage();

                case ApplicationPage.BloodDonationRequests:
                    return new BloodDonationRequestsPage();

                case ApplicationPage.DonationResults:
                    return new DonationsResultsPage();

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
