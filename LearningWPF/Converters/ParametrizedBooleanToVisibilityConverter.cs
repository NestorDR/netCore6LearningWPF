using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LearningWPF.Converters
{
    /// <summary>
    /// This converter that can accept a parameter to invert the boolean when needed
    /// Visit: https://code.4noobz.net/booleantovisibilityconverter/
    /// </summary>
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class ParametrizedBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool flag = false;

            if (value is bool boolValue)
                flag = boolValue;
            else
            {
                if (value is bool?)
                {
                    bool? flag2 = (bool?)value;
                    flag = flag2.HasValue && flag2.Value;
                }
            }

            //If false is passed as a converter parameter then reverse the value of input value
            if (parameter != null && bool.TryParse(parameter.ToString(), out var boolParameter) && !boolParameter)
                flag = !flag;

            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return visibility == Visibility.Visible;

            return false;
        }
    }
}
