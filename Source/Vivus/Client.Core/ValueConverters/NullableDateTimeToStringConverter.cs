namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a converter that takes in a <see cref="DateTime?"/> and returns a <see cref="string"/>.
    /// </summary>
    public class NullableDateTimeToStringConverter : BaseValueConverter<NullableDateTimeToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime?)value)?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (DateTime?)DateTime.Parse(value.ToString());
        }
    }
}
