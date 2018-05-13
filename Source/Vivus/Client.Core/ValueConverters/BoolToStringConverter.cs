namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Converts a <see cref="bool"/> value to a <see cref="string"/> value (yes/no).
    /// </summary>
    public class BoolToStringConverter : BaseValueConverter<BoolToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Yes";

            return "No";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Yes")
                return true;

            return false;
        }
    }
}
