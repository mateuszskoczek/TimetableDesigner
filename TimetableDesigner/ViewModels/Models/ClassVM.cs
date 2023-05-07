using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public class ClassVM : ObservableObject, IModelVM
    {
        #region FIELDS

        private IProjectService _projectService;

        private Class _class;

        #endregion



        #region PROPERTIES

        public Class Class => _class;

        public string Name
        {
            get => _class.Name;
            set
            {
                if (_class.Name != value)
                {
                    _class.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public TeacherVM? Teacher
        {
            get => _projectService.ProjectViewModel?.Teachers.Where(vm => vm.Teacher == _class.Teacher).FirstOrDefault();
            set
            {
                if (_class.Teacher != value?.Teacher)
                {
                    _class.Teacher = value?.Teacher;
                    NotifyPropertyChanged(nameof(Teacher));

                    //REFRESH: Errors
                    _projectService.RefreshErrors();
                }
            }
        }
        public BaseGroupVM? Group
        {
            get
            {

                if (_class.Group?.GetType() == typeof(Group))
                {
                    return _projectService.ProjectViewModel?.Groups.Where(vm => vm.Group == _class.Group).FirstOrDefault();
                }
                else if (_class.Group?.GetType() == typeof(Subgroup))
                {
                    return _projectService.ProjectViewModel?.Subgroups.Where(vm => vm.Subgroup == _class.Group).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_class.Group != value?.BaseGroup)
                {
                    _class.Group = value?.BaseGroup;
                    NotifyPropertyChanged(nameof(Group));

                    //REFRESH: Errors
                    _projectService.RefreshErrors();
                }
            }
        }
        public ClassroomVM? Classroom
        {
            get => _projectService.ProjectViewModel?.Classrooms.Where(vm => vm.Classroom == _class.Classroom).FirstOrDefault();
            set
            {
                if (_class.Classroom != value?.Classroom)
                {
                    _class.Classroom = value?.Classroom;
                    NotifyPropertyChanged(nameof(Classroom));

                    //REFRESH: Errors
                    _projectService.RefreshErrors();
                }
            }
        }
        public TimetableDay? Day
        {
            get => _class.Day;
            set
            {
                if (value != _class.Day) 
                {
                    _class.Day = value;
                    NotifyPropertyChanged(nameof(Day));
                }
            }
        }
        public TimetableSpan? Slot
        {
            get => _class.Slot;
            set
            {
                if (value != _class.Slot)
                {
                    _class.Slot = value;
                    NotifyPropertyChanged(nameof(Slot));
                }
            }
        }
        public byte[] Color
        {
            get => _class.Color;
            set
            {
                if (value != _class.Color)
                {
                    _class.Color = value;
                    NotifyPropertyChanged(nameof(Color));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ClassVM(Class @class) 
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();

            _class = @class;
        }

        #endregion
    }
}
