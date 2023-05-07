using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TimetableDesigner.Core;
using TimetableDesigner.Properties;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.Converters
{
    internal class IUnitVMToUnitNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IUnitVM unit)
            {
                switch (unit)
                {
                    case ClassroomVM classroom: return Resources.Global_Classroom;
                    case TeacherVM teacher: return Resources.Global_Teacher;
                    case GroupVM group: return Resources.Global_Group;
                    case SubgroupVM subgroup: return Resources.Global_Subgroup;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
