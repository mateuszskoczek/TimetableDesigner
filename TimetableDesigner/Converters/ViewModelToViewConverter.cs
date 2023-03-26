using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TimetableDesigner.ViewModels;
using TimetableDesigner.ViewModels.Views;
using TimetableDesigner.Views;

namespace TimetableDesigner.Converters
{
    public class ViewModelToViewConverter : IValueConverter
    {
        #region FIELDS

        private static readonly Dictionary<Type, Type> _viewModelViewPairs = new Dictionary<Type, Type>
        {
            { typeof(MainViewModel), typeof(MainWindow) },
            { typeof(WelcomeViewModel), typeof(WelcomeView) },
            { typeof(ProjectSettingsViewModel), typeof(ProjectSettingsView) },
            { typeof(ClassroomEditViewModel), typeof(ClassroomEditView) },
            { typeof(TeacherEditViewModel), typeof(TeacherEditView) },
            { typeof(GroupEditViewModel), typeof(GroupEditView) },
        };

        #endregion



        #region PUBLIC METHODS

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BaseViewViewModel? viewModel = value as BaseViewViewModel;
            if (viewModel is not null)
            {
                Type view = _viewModelViewPairs[viewModel.GetType()];
                FrameworkElement? viewInstance = Activator.CreateInstance(view) as FrameworkElement;
                if (viewInstance is not null ) 
                {
                    viewInstance.DataContext = viewModel;
                    return viewInstance;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
