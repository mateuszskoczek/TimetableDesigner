using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Services.Project;

namespace TimetableDesigner.ViewModels.Models
{
    public class ProjectVM : ObservableObject, IModelVM
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
        public TimetableTemplateVM TimetableTemplate { get; set; }
        public ObservableCollection<ClassroomVM> Classrooms { get; set; }
        public ObservableCollection<TeacherVM> Teachers { get; set; }
        public ObservableCollection<GroupVM> Groups { get; set; }
        public ObservableCollection<SubgroupVM> Subgroups { get; set; }
        public ObservableCollection<ClassVM> Classes { get; set; }

        #endregion



        #region CONSTRUCTORS

        public ProjectVM(Project project)
        {
            _project = project;

            TimetableTemplate = new TimetableTemplateVM(_project.TimetableTemplate);

            Classrooms = new ObservableCollection<ClassroomVM>(_project.Classrooms.Select(item => new ClassroomVM(item)));
            Classrooms.CollectionChanged += Classrooms_CollectionChanged;

            Teachers = new ObservableCollection<TeacherVM>(_project.Teachers.Select(item => new TeacherVM(item)));
            Teachers.CollectionChanged += Teachers_CollectionChanged;

            Groups = new ObservableCollection<GroupVM>(_project.Groups.Select(item => new GroupVM(item)));
            Groups.CollectionChanged += Groups_CollectionChanged;

            Subgroups = new ObservableCollection<SubgroupVM>(_project.Subgroups.Select(item => new SubgroupVM(item)));
            Subgroups.CollectionChanged += Subgroups_CollectionChanged;

            Classes = new ObservableCollection<ClassVM>(_project.Classes.Select(item => new ClassVM(item)));
            Classes.CollectionChanged += Classes_CollectionChanged;
        }

        #endregion



        #region PRIVATE METHODS

        private void Classrooms_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (ClassroomVM vm in e.OldItems)
                {
                    foreach (ClassVM cvm in Classes.Where(x => x.Classroom == vm))
                    {
                        cvm.Classroom = null;
                    }
                    _project.Classrooms.Remove(vm.Classroom);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (ClassroomVM vm in e.NewItems)
                {
                    _project.Classrooms.Add(vm.Classroom);
                }
            }
        }

        private void Teachers_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (TeacherVM vm in e.OldItems)
                {
                    foreach (ClassVM cvm in Classes.Where(x => x.Teacher == vm))
                    {
                        cvm.Teacher = null;
                    }
                    _project.Teachers.Remove(vm.Teacher);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (TeacherVM vm in e.NewItems)
                {
                    _project.Teachers.Add(vm.Teacher);
                }
            }
        }

        private void Groups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (GroupVM vm in e.OldItems)
                {
                    foreach (ClassVM cvm in Classes.Where(x => x.Group == vm))
                    {
                        cvm.Group = null;
                    }
                    _project.Groups.Remove(vm.Group);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (GroupVM vm in e.NewItems)
                {
                    _project.Groups.Add(vm.Group);
                }
            }
        }

        private void Subgroups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (SubgroupVM vm in e.OldItems)
                {
                    foreach (ClassVM cvm in Classes.Where(x => x.Group == vm))
                    {
                        cvm.Group = null;
                    }
                    foreach (GroupVM gvm in Groups.Where(x => x.AssignedSubgroups.Contains(vm)))
                    {
                        gvm.RemoveSubgroup(vm);
                    }
                    _project.Subgroups.Remove(vm.Subgroup);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (SubgroupVM vm in e.NewItems)
                {
                    _project.Subgroups.Add(vm.Subgroup);
                }
            }
        }

        private void Classes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (ClassVM vm in e.OldItems)
                {
                    _project.Classes.Remove(vm.Class);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (ClassVM vm in e.NewItems)
                {
                    _project.Classes.Add(vm.Class);
                }
            }
        }

        #endregion
    }
}
