namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Vivus.Core.DataModels;
    using VivusConsole = Vivus.Core.Console.Console;

    /// <summary>
    /// Represents a converter that takes in a <see cref="ButtonType"/> and returns a <see cref="string"/>.
    /// </summary>
    public class ButtonTypeToStringConverter : BaseValueConverter<ButtonTypeToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ButtonType)value)
            {
                case ButtonType.Add:
                    return "Add";

                case ButtonType.Modify:
                    return "Modify";

                default:
                    VivusConsole.WriteLine($"Button type conversion impossible. Value: { value }");
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
