namespace Vivus.Client.Core.ValueConverters
{
    using CustomVisibility = Vivus.Core.DataModels;
    using System;
    using System.Globalization;
    using System.Windows;

    class CustomVisibilityToWindowsVisibilityConverter : BaseValueConverter<CustomVisibilityToWindowsVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((CustomVisibility.Visibility)value)
            {
                case CustomVisibility.Visibility.Hidden:
                    return Visibility.Hidden;

                case CustomVisibility.Visibility.Visible:
                    return Visibility.Visible;

                default:
                    return Visibility.Collapsed;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Visibility)value)
            {
                case Visibility.Hidden:
                    return CustomVisibility.Visibility.Hidden;

                case Visibility.Visible:
                    return CustomVisibility.Visibility.Visible;

                default:
                    return CustomVisibility.Visibility.Collapsed;
            }
        }
    }
}
