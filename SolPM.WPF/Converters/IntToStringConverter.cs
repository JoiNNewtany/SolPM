using System;
using System.Globalization;
using System.Windows.Data;

namespace SolPM.WPF.Converters
{
    // For those cases when WPF just doesn't want to work
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return ((double)value).ToString();
            }
            if (value is int)
            {
                return ((int)value).ToString();
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                if (int.TryParse(text, out var number))
                {
                    return number;
                }
                return default(int);
            }
            return default(int);
        }
    }
}