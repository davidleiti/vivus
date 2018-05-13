namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// A base value converter that allows direct XAML usage.
    /// </summary>
    /// <typeparam name="T">The type of the value converter.</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        /// <summary>
        /// A single static instance of this value converter.
        /// </summary>
        private static T converter = null;

        /// <summary>
        /// Provides a static instance of the value converter.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new T());
        }

        /// <summary>
        /// Converts a type to another.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType">The target type of the convertion.</param>
        /// <param name="parameter">Parameter of the convertion.</param>
        /// <param name="culture">Information about culture.</param>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value back to its source type.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType">The target type of the convertion.</param>
        /// <param name="parameter">Parameter of the convertion.</param>
        /// <param name="culture">Information about culture.</param>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
