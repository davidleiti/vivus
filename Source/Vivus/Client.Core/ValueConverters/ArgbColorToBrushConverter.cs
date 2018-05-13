namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Globalization;
    using System.Windows.Media;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Represents a converter that takes in an <see cref="ArgbColor"/> and returns a <see cref="SolidColorBrush"/>.
    /// </summary>
    public class ArgbColorToBrushConverter : BaseValueConverter<ArgbColorToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ArgbColor vmColor = (ArgbColor)value;

            return new SolidColorBrush(Color.FromArgb(vmColor.A, vmColor.R, vmColor.G, vmColor.B));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush uiColor = (SolidColorBrush)value;

            return new ArgbColor(uiColor.Color.A, uiColor.Color.R, uiColor.Color.G, uiColor.Color.B);
        }
    }
}
