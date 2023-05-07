using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Properties;
using TimetableDesigner.Services;
using TimetableDesigner.Services.TabNavigation;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels.Views
{
    public class ClassroomEditorViewVM : ObservableObject, IViewVM, IUnitEditorViewVM
    {
        #region FIELDS

        private ClassroomVM _classroom;

        #endregion



        #region PROPERTIES

        IUnitVM IUnitEditorViewVM.Unit => Classroom;
        public ClassroomVM Classroom
        {
            get => _classroom;
            set
            {
                if (_classroom != value)
                {
                    _classroom = value;
                    NotifyPropertyChanged(nameof(Classroom));
                }
            }
        }

        public string Name
        {
            get => _classroom.Name;
            set
            {
                if (_classroom.Name != value)
                {
                    _classroom.Name = value;
                    NotifyPropertyChanged(nameof(Name));

                    TabItem? tab = ServiceProvider.Instance.GetService<ITabNavigationService>().Tabs.Where(tab => tab.ViewModel == this).FirstOrDefault();
                    if (tab != null)
                    {
                        tab.Title = $"{Resources.Tabs_ClassroomEdit}: {_classroom.Name}";
                    }
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ClassroomEditorViewVM() : this(new ClassroomVM(new Classroom()))
        { }

        public ClassroomEditorViewVM(ClassroomVM classroom)
        {
            _classroom = classroom;
        }

        #endregion
    }
}
