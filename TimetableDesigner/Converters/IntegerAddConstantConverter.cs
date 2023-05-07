using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TimetableDesigner.Converters
{
    public class IntegerAddConstantConverter : IValueConverter
    {
        #region PUBLIC METHODS

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value + int.Parse((string)parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (int)value - (int)parameter;

        #endregion
    }
}
