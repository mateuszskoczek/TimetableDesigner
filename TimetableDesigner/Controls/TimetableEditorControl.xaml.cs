using System;
using System.Collections.Generic;
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
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.Controls
{
    public partial class TimetableEditorControl : UserControl
    {
        #region FIELDS

        private IProjectService _projectService;

        #endregion



        #region PROPERTIES

        public IGroupViewModel Group 
        {
            get => (IGroupViewModel)GetValue(GroupProperty);
            set => SetValue(GroupProperty, value);
        }
        public static readonly DependencyProperty GroupProperty = DependencyProperty.Register("Group", typeof(IGroupViewModel), typeof(TimetableEditorControl));

        #endregion



        #region CONSTRUCTORS

        public TimetableEditorControl()
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();

            InitializeComponent();
        }

        #endregion
    }
}
