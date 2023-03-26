using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;

namespace TimetableDesigner.ViewModels.Models
{
    public class ClassViewModel : BaseModelViewModel
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
        public TeacherViewModel? Teacher
        {
            get => _projectService.ProjectViewModel?.Teachers.Where(vm => vm.Teacher == _class.Teacher).FirstOrDefault();
            set
            {
                if (_class.Teacher != value?.Teacher)
                {
                    _class.Teacher = value?.Teacher;
                    NotifyPropertyChanged(nameof(Teacher));
                }
            }
        }
        public IGroupViewModel? Group
        {
            get
            {
                if (_class.Group?.GetType() == typeof(GroupViewModel))
                {
                    return _projectService.ProjectViewModel?.Groups.Where(vm => vm.Group == _class.Group).FirstOrDefault();
                }
                else if (_class.Group?.GetType() == typeof(SubgroupViewModel))
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
                if (_class.Group != value?.Group)
                {
                    _class.Group = value?.Group;
                    NotifyPropertyChanged(nameof(Group));
                }
            }
        }
        public ClassroomViewModel? Classroom
        {
            get => _projectService.ProjectViewModel?.Classrooms.Where(vm => vm.Classroom == _class.Classroom).FirstOrDefault();
            set
            {
                if (_class.Classroom != value?.Classroom)
                {
                    _class.Classroom = value?.Classroom;
                    NotifyPropertyChanged(nameof(Classroom));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ClassViewModel(Class @class) 
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();

            _class = @class;
        }

        #endregion
    }
}
