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
            { typeof(MainWindowVM), typeof(MainWindow) },
            { typeof(WelcomeViewVM), typeof(WelcomeView) },
            { typeof(ProjectSettingsViewVM), typeof(ProjectSettingsView) },
            { typeof(ClassroomEditorViewVM), typeof(ClassroomEditorView) },
            { typeof(TeacherEditorViewVM), typeof(TeacherEditorView) },
            { typeof(GroupEditorViewVM), typeof(GroupEditorView) },
            { typeof(TimetableEditorViewVM), typeof(TimetableEditorView) },
        };

        #endregion



        #region PUBLIC METHODS

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IViewVM? viewModel = value as IViewVM;
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
