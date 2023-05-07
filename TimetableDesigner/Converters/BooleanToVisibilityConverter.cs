using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TimetableDesigner.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        // (bool)parameter - reverse output
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value.GetType() != typeof(bool))
            {
                return Visibility.Visible;
            }
            bool isVisible = (bool)value;

            bool reverse = false;
            if (parameter is not null && parameter.GetType() == typeof(bool))
            {
                reverse = (bool)parameter;
            }

            return isVisible ^ reverse ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value.GetType() != typeof(Visibility))
            {
                return true;
            }
            Visibility visibility = (Visibility)value;

            bool reverse = false;
            if (parameter is not null && parameter.GetType() == typeof(bool))
            {
                reverse = (bool)parameter;
            }

            return visibility == Visibility.Visible ^ reverse;
        }
    }
}
