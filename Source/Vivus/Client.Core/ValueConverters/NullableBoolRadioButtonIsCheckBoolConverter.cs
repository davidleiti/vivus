namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a converter that takes in a nullable <see cref="bool"/> value and returns a <see cref="bool"/> value.
    /// </summary>
    public class NullableBoolRadioButtonIsCheckBoolConverter : BaseValueConverter<NullableBoolRadioButtonIsCheckBoolConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return false;
            
            return (value as bool?).Value == bool.Parse(parameter.ToString());
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // True only when the checked radiobutton is the one with the parameter true (The 'yes' radiobutton)
            return (value as bool?).Value && bool.Parse(parameter.ToString());
        }
    }
}
