using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivus.Client.Core.ValueConverters
{
    /// <summary>
    /// Represents a converter that takes in an <see cref="int"/> and returns a <see cref="string"/>.
    /// </summary>
    public class IntToStringConverter : BaseValueConverter<IntToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == string.Empty)
                return null;
            return int.Parse(value.ToString());
        }
    }
}
