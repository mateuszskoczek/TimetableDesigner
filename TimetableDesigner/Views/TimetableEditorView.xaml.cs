using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Xml.Linq;
using TimetableDesigner.Controls;
using TimetableDesigner.Services.Project;
using TimetableDesigner.Services.TabNavigation;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.ViewModels.Views;

namespace TimetableDesigner.Views
{
    public partial class TimetableEditorView : UserControl
    {
        #region FIELDS

        private ITabNavigationService _tabNavigationService;
        private IProjectService _projectService;

        #endregion



        #region CONSTRUCTORS

        public TimetableEditorView()
        {
            _tabNavigationService = ServiceProvider.Instance.GetService<ITabNavigationService>();
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            InitializeComponent();
        }

        #endregion

        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is ClassControl control && !control.EditButton.IsChecked.Value)
            {
                DragDrop.DoDragDrop((DependencyObject)sender, new DataObject(DataFormats.Serializable, sender), DragDropEffects.Move);
            }
        }

        private void ItemsControl_Drop(object sender, DragEventArgs e)
        {
            object item = e.Data.GetData(DataFormats.Serializable);
            if (item is UIElement element)
            {
                DynamicGrid.SetRow(element, -1);
                DynamicGrid.SetColumn(element, -1);
            }
        }
    }
}
