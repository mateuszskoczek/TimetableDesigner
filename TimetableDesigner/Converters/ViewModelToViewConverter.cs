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
using TimetableDesigner.ViewModels.Base;
using TimetableDesigner.Views;

namespace TimetableDesigner.Converters
{
    public class ViewModelToViewConverter : IValueConverter
    {
        #region FIELDS

        private static readonly Dictionary<Type, Type> _viewModelViewPairs = new Dictionary<Type, Type>
        {
            { typeof(MainViewModel), typeof(MainWindow) },
            { typeof(WelcomeTabViewModel), typeof(WelcomeTabView) },
            { typeof(ProjectSettingsTabViewModel), typeof(ProjectSettingsTabView) },
            { typeof(ClassroomEditTabViewModel), typeof(ClassroomEditTabView) },
        };

        #endregion



        #region PUBLIC METHODS

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BaseViewModel? viewModel = value as BaseViewModel;
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
