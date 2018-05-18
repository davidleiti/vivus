namespace Vivus.Client.Administration.Desktop.ValueConverters
{
    using System;
    using System.Globalization;
    using Vivus.Client.Core.ValueConverters;
    using VivusConsole = Vivus.Core.Console.Console;
    using Vivus.Core.Administration.DataModels;
    using System.Diagnostics;
    using Vivus.Client.Administration.Desktop.Pages;

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

                case ApplicationPage.Doctors:
                    return new DoctorsPage();

                case ApplicationPage.DonationCenters:
                    return new DonationCentersPage();

                case ApplicationPage.DCPersonnel:
                    return new DCPersonnelPage();

                case ApplicationPage.Administrators:
                    return new AdministratorsPage();

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
