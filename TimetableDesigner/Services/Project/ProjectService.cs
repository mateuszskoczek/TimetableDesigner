using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Properties;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.ViewModels.Models.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TimetableDesigner.Services.Project
{
    public class ProjectService : ObservableObject, IProjectService
    {
        #region FIELDS

        private Core.Project? _project;
        private ProjectVM? _projectViewModel;

        private string? _savePath;
        private ObservableDictionary<Guid, RecentProjectEntry> _recentProjects;

        private ObservableCollection<ProjectError> _errors;

        private static readonly JsonSerializerSettings _serializationSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            NullValueHandling = NullValueHandling.Include,
        };

        #endregion



        #region PROPERTIES

        public Core.Project? Project
        {
            get => _project;
            private set
            {
                if (_project != value)
                {
                    _project = value;
                    NotifyPropertyChanged(nameof(Project));
                }
            }
        }
        public ProjectVM? ProjectViewModel
        {
            get => _projectViewModel;
            private set
            {
                if (_projectViewModel != value)
                {
                    _projectViewModel = value;
                    NotifyPropertyChanged(nameof(ProjectViewModel));
                }
            } 
        }

        public string? SavePath
        {
            get => _savePath;
            private set
            {
                if (_savePath != value)
                {
                    _savePath = value;
                    NotifyPropertyChanged(nameof(SavePath));
                }
            }
        }
        public ObservableDictionary<Guid, RecentProjectEntry> RecentProjects
        {
            get => _recentProjects;
            private set
            {
                if (_recentProjects != value)
                {
                    _recentProjects = value;
                    NotifyPropertyChanged(nameof(RecentProjects));
                }
            }
        }

        public ObservableCollection<ProjectError> Errors
        {
            get => _errors;
            set
            {
                if (_errors != value)
                {
                    _errors = value;
                    NotifyPropertyChanged(nameof(Errors));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ProjectService()
        {
            Project = null;
            ProjectViewModel = null;

            SavePath = null;
            RecentProjects = new ObservableDictionary<Guid, RecentProjectEntry>();

            Errors = new ObservableCollection<ProjectError>();
        }

        #endregion



        #region PUBLIC METHODS

        public void New()
        {
            Project = new Core.Project()
            {
                Name = Resources.Global_DefaultProjectName,
            };
            ProjectViewModel = new ProjectVM(Project);
            SavePath = null;
            RefreshErrors();
        }

        public async void Load(string path)
        {
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();

                Project = JsonConvert.DeserializeObject<Core.Project>(content, _serializationSettings);
                ProjectViewModel = new ProjectVM(Project);
            }
            SavePath = path;
            RefreshErrors();
        }

        public void Save(string path)
        {
            using (FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string content = JsonConvert.SerializeObject(Project, _serializationSettings);

                writer.Write(content);
            }
            SavePath = path;

            if (!RecentProjects.ContainsKey(Project.Guid))
            {
                if (RecentProjects.Count == Settings.Default.RecentProjectsCount)
                {
                    IEnumerable<ObservableKeyValuePair<Guid, RecentProjectEntry>> kvp = RecentProjects.Cast<ObservableKeyValuePair<Guid, RecentProjectEntry>>();
                    RecentProjects.Remove(kvp.Where(x => x.Value.SaveDate <= kvp.Min(y => y.Value.SaveDate)).FirstOrDefault());
                }
                RecentProjects.Add(Project.Guid, new(Project.Name, DateTime.Now, SavePath));
            }
            else
            {
                RecentProjects[Project.Guid] = new(Project.Name, DateTime.Now, SavePath);
            }
            SaveRecentProjectsList();
        }


        public void RefreshErrors()
        {
            Errors.Clear();

            foreach (ClassVM class1 in ProjectViewModel.Classes)
            {
                foreach (ClassVM class2 in ProjectViewModel.Classes)
                {
                    if (class1.Equals(class2))
                    {
                        continue;
                    }

                    if (class1.Day is not null
                        &&
                        class2.Day is not null
                        &&
                        class1.Day.Equals(class2.Day)
                        &&
                        class1.Slot is not null
                        &&
                        class2.Slot is not null
                        &&
                        class1.Slot.Equals(class2.Slot))
                    {
                        // same classroom at the same time
                        bool classroomCondition = class1.Classroom is not null
                                                  &&
                                                  class2.Classroom is not null
                                                  &&
                                                  class1.Classroom.Equals(class2.Classroom);
                        if (classroomCondition)
                        {
                            ProjectError newEntry = new ProjectError(ProjectErrorType.Error, class2.Classroom, class2, Resources.Errors_ClassroomBusy);
                            Errors.Add(newEntry);
                        }

                        // same teacher at the same time
                        bool teacherCondition = class1.Teacher is not null
                                                &&
                                                class2.Teacher is not null
                                                &&
                                                class1.Teacher.Equals(class2.Teacher);
                        if (teacherCondition)
                        {
                            ProjectError newEntry = new ProjectError(ProjectErrorType.Error, class2.Teacher, class2, Resources.Errors_TeacherBusy);
                            Errors.Add(newEntry);
                        }

                        // same group at the same time
                        if (class1.Group is not null && class2.Group is not null)
                        {
                            if (class1.Group.Equals(class2.Group))
                            {
                                ProjectError newEntry = new ProjectError(ProjectErrorType.Error, class2.Group, class2, Resources.Errors_GroupBusy);
                                Errors.Add(newEntry);
                            }
                            else if (class1.Group is GroupVM group1 && class2.Group is SubgroupVM subgroup2 && group1.AssignedSubgroups.Contains(subgroup2))
                            {
                                ProjectError newEntry = new ProjectError(ProjectErrorType.Error, class2.Group, class2, Resources.Errors_GroupPartBusy);
                                Errors.Add(newEntry);
                            }
                            else if (class1.Group is SubgroupVM subgroup1 && class2.Group is GroupVM group2 && group2.AssignedSubgroups.Contains(subgroup1))
                            {
                                ProjectError newEntry = new ProjectError(ProjectErrorType.Error, class2.Group, class2, Resources.Errors_GroupAllBusy);
                                Errors.Add(newEntry);
                            }
                        }
                    }
                }

                bool teacherHoursCondition = class1.Teacher is not null
                                             &&
                                             class1.Day is not null
                                             &&
                                             class1.Slot is not null
                                             &&
                                             (
                                                 !class1.Teacher.AvailabilityHours.ContainsKey(class1.Day)
                                                 ||
                                                 (
                                                     class1.Teacher.AvailabilityHours.ContainsKey(class1.Day)
                                                     &&
                                                     !class1.Teacher.AvailabilityHours[class1.Day].Any(x => x.CheckCollision(class1.Slot) == TimetableSpanCollision.CheckedSlotIn)
                                                 )
                                             );
                if (teacherHoursCondition)
                {
                    ProjectError newEntry = new ProjectError(ProjectErrorType.Warning, class1.Teacher, class1, Resources.Errors_TeacherNotAvailable);
                    Errors.Add(newEntry);
                }

                bool teacherAssigned = class1.Teacher is not null;
                bool classroomAssigned = class1.Classroom is not null;
                bool groupAssigned = class1.Group is not null;

                List<IUnitVM> assigned = new List<IUnitVM>();
                if (groupAssigned)
                {
                    assigned.Add(class1.Group);
                }
                if (teacherAssigned)
                {
                    assigned.Add(class1.Teacher);
                }
                if (classroomAssigned)
                {
                    assigned.Add(class1.Classroom);
                }
                IUnitVM unit = assigned.FirstOrDefault();

                if (!teacherAssigned)
                {
                    ProjectError newEntry = new ProjectError(ProjectErrorType.Warning, unit, class1, Resources.Errors_TeacherNotAssigned);
                    Errors.Add(newEntry);
                }
                if (!classroomAssigned)
                {
                    ProjectError newEntry = new ProjectError(ProjectErrorType.Warning, unit, class1, Resources.Errors_ClassroomNotAssigned);
                    Errors.Add(newEntry);
                }
                if (!groupAssigned)
                {
                    ProjectError newEntry = new ProjectError(ProjectErrorType.Warning, unit, class1, Resources.Errors_GroupNotAssigned);
                    Errors.Add(newEntry);
                }

                if (class1.Slot is null || class1.Day is null)
                {
                    ProjectError newEntry = new ProjectError(ProjectErrorType.Information, unit, class1, Resources.Errors_SlotNotAssigned);
                    Errors.Add(newEntry);
                }
            }
        }


        public void LoadRecentProjectsList()
        {
            RecentProjects.Clear();

            if (!File.Exists(Globals.Path.RecentProjectsFile))
            {
                return;
            }

            string[] projects;
            using (FileStream stream = File.Open(Globals.Path.RecentProjectsFile, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(stream))
            {
                projects = reader.ReadToEnd().Split('\n');
            }
            foreach (string project in projects)
            {
                string[] projectData = project.Split(" : ");
                if (projectData.Length < 4)
                {
                    continue;
                }

                string name = projectData[1];
                string path = projectData[3];
                if (Guid.TryParse(projectData[0], out Guid guid) && DateTime.TryParse(projectData[2], out DateTime saveDate) && !string.IsNullOrWhiteSpace(path) && File.Exists(path) && !string.IsNullOrWhiteSpace(name))
                {
                    RecentProjects.Add(guid, new(name, saveDate, path));
                }
            }
        }

        public void SaveRecentProjectsList()
        {
            List<string> lines = new List<string>();
            foreach (ObservableKeyValuePair<Guid, RecentProjectEntry> project in RecentProjects)
            {
                lines.Add($"{project.Key} : {project.Value.Name} : {project.Value.SaveDate} : {project.Value.Path}");
            }

            using (FileStream stream = File.Open(Globals.Path.RecentProjectsFile, FileMode.Create, FileAccess.Write))
            using (StreamWriter reader = new StreamWriter(stream))
            {
                reader.Write(string.Join('\n', lines));
            }
        }

        #endregion
    }
}
