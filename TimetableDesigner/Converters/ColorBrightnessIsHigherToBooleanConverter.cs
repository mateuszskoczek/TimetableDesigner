using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TimetableDesigner.Converters
{
    public class ColorBrightnessIsHigherToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] color && (parameter is int min || (parameter is string minStr && int.TryParse(minStr, out min))))
            {
                int start = 0;
                if (color.Length == 4)
                {
                    start = 1;
                }
                int result = (int)Math.Sqrt(color[start] * color[start] * .241 +
                                            color[start + 1] * color[start + 1] * .691 +
                                            color[start + 2] * color[start + 2] * .068);
                return min < result;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
