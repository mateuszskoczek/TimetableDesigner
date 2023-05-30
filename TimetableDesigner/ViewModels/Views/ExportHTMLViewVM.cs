using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using TimetableDesigner.Commands;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Export;
using TimetableDesigner.Services.FileDialog;
using TimetableDesigner.Services.MessageBox;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.ViewModels.Views
{
    public class ExportHTMLViewVM : ObservableObject, IViewVM
    {
        #region FIELDS

        private readonly IProjectService _projectService;
        private readonly IFileDialogService _fileDialogService;
        private readonly IMessageBoxService _messageBoxSercvice;

        #endregion



        #region PROPERTIES

        public ObservableDictionary<GroupVM, bool> Groups { get; set; }
        public ObservableDictionary<SubgroupVM, bool> Subgroups { get; set; }
        public ObservableDictionary<TeacherVM, bool> Teachers { get; set; }
        public ObservableDictionary<ClassroomVM, bool> Classrooms { get; set; }

        public ICommand ExportCommand { get; set; }

        #endregion



        #region CONSTRUCTORS

        public ExportHTMLViewVM()
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            _fileDialogService = ServiceProvider.Instance.GetService<IFileDialogService>();
            _messageBoxSercvice = ServiceProvider.Instance.GetService<IMessageBoxService>();

            ObservableCollection<GroupVM> groups = _projectService.ProjectViewModel.Groups;
            groups.CollectionChanged += Groups_CollectionChanged;
            Groups = new ObservableDictionary<GroupVM, bool>(groups.ToDictionary(x => x, x => true));

            ObservableCollection<SubgroupVM> subgroups = _projectService.ProjectViewModel.Subgroups;
            subgroups.CollectionChanged += Subgroups_CollectionChanged;
            Subgroups = new ObservableDictionary<SubgroupVM, bool>(subgroups.ToDictionary(x => x, x => true));

            ObservableCollection<TeacherVM> teachers = _projectService.ProjectViewModel.Teachers;
            teachers.CollectionChanged += Teachers_CollectionChanged;
            Teachers = new ObservableDictionary<TeacherVM, bool>(teachers.ToDictionary(x => x, x => true));

            ObservableCollection<ClassroomVM> classrooms = _projectService.ProjectViewModel.Classrooms;
            classrooms.CollectionChanged += Classrooms_CollectionChanged;
            Classrooms = new ObservableDictionary<ClassroomVM, bool>(classrooms.ToDictionary(x => x, x => true));

            ExportCommand = new RelayCommand<object>(a => Export());
        }

        #endregion



        #region PRIVATE METHODS

        private void Export()
        {
            Dictionary<string, IEnumerable<string>> types = new Dictionary<string, IEnumerable<string>>()
            {
                { "HTML", new List<string> { "html" } }
            };
            string? path = _fileDialogService.SaveFile(types);
            if (path is null)
            {
                return;
            }

            Project project = _projectService.Project;
            IEnumerable<Group> groups = Groups.Cast<ObservableKeyValuePair<GroupVM, bool>>().Where(x => x.Value).Select(x => x.Key.Group);
            IEnumerable<Subgroup> subgroups = Subgroups.Cast<ObservableKeyValuePair<SubgroupVM, bool>>().Where(x => x.Value).Select(x => x.Key.Subgroup);
            IEnumerable<Teacher> teachers = Teachers.Cast<ObservableKeyValuePair<TeacherVM, bool>>().Where(x => x.Value).Select(x => x.Key.Teacher);
            IEnumerable<Classroom> classrooms = Classrooms.Cast<ObservableKeyValuePair<ClassroomVM, bool>>().Where(x => x.Value).Select(x => x.Key.Classroom);

            Exporter exporter = new ExporterHTML(project);
            foreach (Group group in groups)
            {
                exporter.Groups.Add(group);
            }
            foreach (Subgroup subgroup in subgroups)
            {
                exporter.Subgroups.Add(subgroup);
            }
            foreach (Teacher teacher in teachers)
            {
                exporter.Teachers.Add(teacher);
            }
            foreach (Classroom classroom in classrooms)
            {
                exporter.Classrooms.Add(classroom);
            }
            exporter.Export(path);

            _messageBoxSercvice.ShowInformation("Data was exported successfully");
        }

        private void Groups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (GroupVM vm in e.OldItems)
                {
                    Groups.Remove(vm);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (GroupVM vm in e.NewItems)
                {
                    Groups.Add(vm, true);
                }
            }
        }

        private void Subgroups_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (SubgroupVM vm in e.OldItems)
                {
                    Subgroups.Remove(vm);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (SubgroupVM vm in e.NewItems)
                {
                    Subgroups.Add(vm, true);
                }
            }
        }

        private void Teachers_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (TeacherVM vm in e.OldItems)
                {
                    Teachers.Remove(vm);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (TeacherVM vm in e.NewItems)
                {
                    Teachers.Add(vm, true);
                }
            }
        }

        private void Classrooms_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is not null)
            {
                foreach (ClassroomVM vm in e.OldItems)
                {
                    Classrooms.Remove(vm);
                }
            }

            if (e.NewItems is not null)
            {
                foreach (ClassroomVM vm in e.NewItems)
                {
                    Classrooms.Add(vm, true);
                }
            }
        }

        #endregion
    }
}
