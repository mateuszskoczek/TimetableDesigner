using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TimetableDesigner.Converters
{
    class BooleanToGridColumnRowVisibilityConverter : IValueConverter
    {
        private static GridLength _notVisible = new GridLength(0, GridUnitType.Pixel);
        private static GridLength _visible = new GridLength(1, GridUnitType.Star);

        // (bool)parameter - reverse output
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value.GetType() != typeof(bool))
            {
                return new GridLength(1, GridUnitType.Star);
            }
            bool isVisible = (bool)value;

            bool reverse = false;
            if (parameter is not null && parameter.GetType() == typeof(bool))
            {
                reverse = (bool)parameter;
            }

            return isVisible ^ reverse ? _visible : _notVisible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value.GetType() != typeof(GridLength))
            {
                return true;
            }
            GridLength visibility = (GridLength)value;

            bool reverse = false;
            if (parameter is not null && parameter.GetType() == typeof(bool))
            {
                reverse = (bool)parameter;
            }

            return visibility != _notVisible ^ reverse;
        }
    }
}
