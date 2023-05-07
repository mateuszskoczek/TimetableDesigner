using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimetableDesigner.Core;
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;
using TimetableDesigner.Services.TabNavigation;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.ViewModels.Models.Base;
using TimetableDesigner.ViewModels.Views;

namespace TimetableDesigner.Controls
{
    public partial class ClassControl : UserControl
    {
        #region FIELDS

        private IProjectService _projectService;
        private ITabNavigationService _tabNavigationService;

        #endregion



        #region PROPERTIES

        private static DependencyProperty RemoveButtonCommandProperty = DependencyProperty.Register(
            "RemoveButtonCommand",
            typeof(ICommand),
            typeof(ClassControl),
            new PropertyMetadata(null)
        );
        public ICommand RemoveButtonCommand
        {
            get => (ICommand)GetValue(RemoveButtonCommandProperty);
            set => SetValue(RemoveButtonCommandProperty, value);
        }

        private static DependencyProperty RemoveButtonCommandParameterProperty = DependencyProperty.Register(
            "RemoveButtonCommandParameter",
            typeof(object),
            typeof(ClassControl),
            new PropertyMetadata(null)
        );
        public object RemoveButtonCommandParameter
        {
            get => GetValue(RemoveButtonCommandParameterProperty);
            set => SetValue(RemoveButtonCommandParameterProperty, value);
        }

        private static DependencyProperty CloneButtonCommandProperty = DependencyProperty.Register(
            "CloneButtonCommand",
            typeof(ICommand),
            typeof(ClassControl),
            new PropertyMetadata(null)
        );
        public ICommand CloneButtonCommand
        {
            get => (ICommand)GetValue(CloneButtonCommandProperty);
            set => SetValue(CloneButtonCommandProperty, value);
        }

        private static DependencyProperty CloneButtonCommandParameterProperty = DependencyProperty.Register(
            "CloneButtonCommandParameter",
            typeof(object),
            typeof(ClassControl),
            new PropertyMetadata(null)
        );
        public object CloneButtonCommandParameter
        {
            get => GetValue(CloneButtonCommandParameterProperty);
            set => SetValue(CloneButtonCommandParameterProperty, value);
        }


        public IProjectService ProjectService => _projectService;

        #endregion



        #region CONSTRUCTORS

        public ClassControl()
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            _tabNavigationService = ServiceProvider.Instance.GetService<ITabNavigationService>();

            InitializeComponent();
        }

        #endregion



        #region PRIVATE METHODS

        private void RemoveButton_Click(object sender, RoutedEventArgs e) => RemoveButtonCommand?.Execute(RemoveButtonCommandParameter);

        private void CloneButton_Click(object sender, RoutedEventArgs e) => CloneButtonCommand?.Execute(CloneButtonCommandParameter);

        #endregion

    }
}
