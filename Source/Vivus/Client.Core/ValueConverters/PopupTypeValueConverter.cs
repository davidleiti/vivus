namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Media;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Converts the <see cref="PopupType"/> to an actual color.
    /// </summary>
    public class PopupTypeValueConverter : BaseValueConverter<PopupTypeValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate type
            switch ((PopupType)value)
            {
                case PopupType.Error:
                    return new SolidColorBrush(Color.FromRgb(246, 0, 40));

                case PopupType.Warning:
                    return new SolidColorBrush(Color.FromRgb(255, 198, 0));

                case PopupType.Successful:
                    return new SolidColorBrush(Color.FromRgb(20, 210, 20));

                default:
                    Console.WriteLine($"Popup type convesion impossible. Value: { value }");
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
