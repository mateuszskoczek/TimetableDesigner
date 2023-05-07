using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Diagnostics;

namespace TimetableDesigner.Converters
{
    public class ByteArrayToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = (byte[])value;
            Color color;
            switch (bytes.Length)
            {
                case < 3: color = Colors.Red; break;
                case 3: color = Color.FromRgb(bytes[0], bytes[1], bytes[2]); break;
                case > 4: color = Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]); break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            List<byte> bytes = new List<byte>();

            if (color.A != 0xFF) bytes.Add(color.A);
            bytes.Add(color.R);
            bytes.Add(color.G);
            bytes.Add(color.B);

            return bytes.ToArray();
        }
    }
}
