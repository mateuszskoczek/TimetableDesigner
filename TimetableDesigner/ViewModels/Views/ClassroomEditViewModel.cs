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

namespace TimetableDesigner.ViewModels.Views
{
    public class ClassroomEditViewModel : BaseViewViewModel
    {
        #region FIELDS

        private ClassroomViewModel _classroom;

        #endregion



        #region PROPERTIES

        public ClassroomViewModel Classroom
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

        public ClassroomEditViewModel() : this(new ClassroomViewModel(new Classroom()))
        { }

        public ClassroomEditViewModel(ClassroomViewModel classroom)
        {
            _classroom = classroom;
        }

        #endregion
    }
}
