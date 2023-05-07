using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TimetableDesigner.Core;
using TimetableDesigner.Services.Project;
using TimetableDesigner.Services;
using System.Diagnostics;

namespace TimetableDesigner.Converters
{
    internal class TimetableSlotToRowNumberConverter : IValueConverter
    {
        #region FIELDS

        private readonly IProjectService _projectService;

        #endregion



        #region CONSTRUCTORS

        public TimetableSlotToRowNumberConverter()
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
        }

        #endregion



        #region PUBLIC METHODS

        //value - TimetableSlot
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimetableSpan slot)
            {
                return _projectService.ProjectViewModel.TimetableTemplate.Slots.IndexOf(slot);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && index >= 0)
            {
                return _projectService.ProjectViewModel.TimetableTemplate.Slots[index];
            }
            return null;
        }

        #endregion
    }
}
