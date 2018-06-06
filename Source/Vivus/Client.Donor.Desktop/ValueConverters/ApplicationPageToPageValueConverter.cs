namespace Vivus.Client.Donor.Desktop.ValueConverters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Vivus.Client.Core.ValueConverters;
    using Vivus.Client.Donor.Desktop.Pages;
    using VivusConsole = Vivus.Core.Console.Console;
    using Vivus.Core.Donor.DataModels;

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

                case ApplicationPage.Register:
                    return new SignUpPage();

                case ApplicationPage.ReasonsToDonate:
                    return new ReasonsToDonatePage();

                case ApplicationPage.Apply:
                    return new ApplyPage();

                case ApplicationPage.History:
                    return new HistoryPage();

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
