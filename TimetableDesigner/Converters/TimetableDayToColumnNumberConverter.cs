using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TimetableDesigner.Core;
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.Converters
{
    internal class TimetableDayToColumnNumberConverter : IValueConverter
    {
        #region FIELDS

        private readonly IProjectService _projectService;

        #endregion



        #region CONSTRUCTORS

        public TimetableDayToColumnNumberConverter()
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
        }

        #endregion



        #region PUBLIC METHODS

        //value - TimetableDay
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimetableDay day)
            {
                return _projectService.ProjectViewModel.TimetableTemplate.Days.IndexOf(day);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && index >= 0)
            {
                return _projectService.ProjectViewModel.TimetableTemplate.Days[index];
            }
            return null;
        }

        #endregion
    }
}
