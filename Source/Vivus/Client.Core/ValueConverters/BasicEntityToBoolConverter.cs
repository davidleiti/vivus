﻿namespace Vivus.Client.Core.ValueConverters
{
    using System;
    using System.Globalization;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Represents a converter that takes in a <see cref="BasicEntity{Type}"/> and returns a <see cref="bool"/> based on the parameter value.
    /// </summary>
    public class BasicEntityToBoolConverter : BaseValueConverter<BasicEntityToBoolConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((BasicEntity<string>)value).Value == parameter.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BasicEntity<string>(-1, parameter.ToString());
        }
    }
}
