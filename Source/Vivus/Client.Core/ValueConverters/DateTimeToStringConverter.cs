namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a converter that takes in a <see cref="DateTime"/> and returns a <see cref="string"/>.
    /// </summary>
    public class DateTimeToStringConverter : BaseValueConverter<DateTimeToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("dd/MM/yyyy");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse(value.ToString());
        }
    }
}
