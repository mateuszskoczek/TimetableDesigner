using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.ViewModels.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public class ProjectViewModel : BaseViewModel
    {
        #region FIELDS

        private Project _project;

        #endregion



        #region PROPERTIES

        public Project Project => _project;

        public string Name
        {
            get => Project.Name;
            set
            {
                if (Project.Name != value)
                {
                    Project.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Author
        {
            get => Project.Author;
            set
            {
                if (Project.Author != value)
                {
                    Project.Author = value;
                    NotifyPropertyChanged(nameof(Author));
                }
            }
        }
        public string Description
        {
            get => Project.Description;
            set
            {
                if (Project.Description != value)
                {
                    Project.Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }
        public TimetableTemplateViewModel TimetableTemplate { get; set; }
        public ObservableCollection<ClassroomViewModel> Classrooms { get; set; }
        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        #endregion



        #region CONSTRUCTORS

        public ProjectViewModel(Project project)
        {
            _project = project;

            TimetableTemplate = new TimetableTemplateViewModel(_project.TimetableTemplate);

            Classrooms = new ObservableCollection<ClassroomViewModel>();
            Classrooms.CollectionChanged += Classrooms_CollectionChanged;

            Teachers = new ObservableCollection<TeacherViewModel>();
            Teachers.CollectionChanged += Teachers_CollectionChanged;
        }

        #endregion



        #region PRIVATE METHODS

        private void Classrooms_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IList<ClassroomViewModel>? added = e.NewItems as IList<ClassroomViewModel>;
            IList<ClassroomViewModel>? removed = e.OldItems as IList<ClassroomViewModel>;
            
            if (removed is not null)
            {
                foreach (ClassroomViewModel vm in removed)
                {
                    _project.Classrooms.Remove(vm.Classroom);
                }
            }

            if (added is not null)
            {
                foreach (ClassroomViewModel vm in added)
                {
                    _project.Classrooms.Add(vm.Classroom);
                }
            }
        }

        private void Teachers_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IList<TeacherViewModel>? added = e.NewItems as IList<TeacherViewModel>;
            IList<TeacherViewModel>? removed = e.OldItems as IList<TeacherViewModel>;

            if (removed is not null)
            {
                foreach (TeacherViewModel vm in removed)
                {
                    _project.Teachers.Remove(vm.Teacher);
                }
            }

            if (added is not null)
            {
                foreach (TeacherViewModel vm in added)
                {
                    _project.Teachers.Add(vm.Teacher);
                }
            }
        }

        #endregion
    }
}
