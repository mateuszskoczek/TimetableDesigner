using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;

namespace TimetableDesigner.ViewModels.Models
{
    public class ProjectViewModel : BaseModelViewModel
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
        public ObservableCollection<GroupViewModel> Groups { get; set; }
        public ObservableCollection<SubgroupViewModel> Subgroups { get; set; }

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

            Groups = new ObservableCollection<GroupViewModel>();
            Groups.CollectionChanged += Groups_CollectionChanged;

            Subgroups = new ObservableCollection<SubgroupViewModel>();
            Subgroups.CollectionChanged += Subgroups_CollectionChanged;
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

        private void Groups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IList<GroupViewModel>? added = e.NewItems as IList<GroupViewModel>;
            IList<GroupViewModel>? removed = e.OldItems as IList<GroupViewModel>;

            if (removed is not null)
            {
                foreach (GroupViewModel vm in removed)
                {
                    _project.Groups.Remove(vm.Group);
                }
            }

            if (added is not null)
            {
                foreach (GroupViewModel vm in added)
                {
                    _project.Groups.Add(vm.Group);
                }
            }
        }

        private void Subgroups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IList<SubgroupViewModel>? added = e.NewItems as IList<SubgroupViewModel>;
            IList<SubgroupViewModel>? removed = e.OldItems as IList<SubgroupViewModel>;

            if (removed is not null)
            {
                foreach (SubgroupViewModel vm in removed)
                {
                    _project.Subgroups.Remove(vm.Subgroup);
                }
            }

            if (added is not null)
            {
                foreach (SubgroupViewModel vm in added)
                {
                    _project.Subgroups.Add(vm.Subgroup);
                }
            }
        }

        #endregion
    }
}
