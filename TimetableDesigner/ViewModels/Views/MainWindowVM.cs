using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimetableDesigner.Commands;
using System.Windows.Navigation;
using TimetableDesigner.Core;
using System.Windows;
using TimetableDesigner.Properties;
using System.Reflection;
using TimetableDesigner.ViewModels.Models;
using System.Windows.Data;
using TimetableDesigner.Services.MessageBox;
using System.ComponentModel.Design;
using TimetableDesigner.Services;
using TimetableDesigner.Services.TabNavigation;
using TimetableDesigner.Services.Project;
using System.Drawing;
using TimetableDesigner.ViewModels.Models.Base;
using System.Collections;
using TimetableDesigner.Customs;
using Microsoft.Win32;
using System.Windows.Forms;
using TimetableDesigner.Services.FileDialog;
using TimetableDesigner.Services.Scheduler;
using static TimetableDesigner.ViewModels.Views.TimetableEditorViewVM;
using TimetableDesigner.Views;

namespace TimetableDesigner.ViewModels.Views
{
    class MainWindowVM : ObservableObject, IViewVM
    {
        #region FIELDS

        private IMessageBoxService _messageBoxService;
        private IFileDialogService _fileDialogService;
        private ITabNavigationService _tabNavigationService;
        private IProjectService _projectService;
        private ISchedulerService _schedulerService;

        private IDictionary<string, IEnumerable<string>> _projectFileTypes = new Dictionary<string, IEnumerable<string>>()
        {
            { Resources.Global_ProjectFiletypeDescription, new List<string>() { "ttdp" } }
        };

        #endregion



        #region PROPERTIES

        // Observable services
        public IProjectService ProjectService => _projectService;
        public ITabNavigationService TabNavigationService => _tabNavigationService;

        // Tabs
        public ObservableCollection<TabItem> Tabs => _tabNavigationService.Tabs;
        public TabItem SelectedTab => _tabNavigationService.SelectedTab;

        // Commands
        public ICommand NewProjectCommand { get; set; }
        public ICommand OpenProjectCommand { get; set; }
        public ICommand OpenRecentProjectCommand { get; set; }
        public ICommand SaveProjectCommand { get; set; }
        public ICommand SaveAsProjectCommand { get; set; }
        public ICommand ProjectSettingsCommand { get; set; }
        public ICommand NewClassroomCommand { get; set; }
        public ICommand NewTeacherCommand { get; set; }
        public ICommand NewGroupCommand { get; set; }
        public ICommand EditClassroomCommand { get; set; }
        public ICommand EditTeacherCommand { get; set; }
        public ICommand EditGroupCommand { get; set; }
        public ICommand RemoveClassroomCommand { get; set; }
        public ICommand RemoveTeacherCommand { get; set; }
        public ICommand RemoveGroupCommand { get; set; }
        public ICommand RemoveSubgroupCommand { get; set; }
        public ICommand EditTimetableCommand { get; set; }
        public ICommand AutoScheduleCommand { get; set; }
        public ICommand ExportHTMLCommand { get; set; }

        // Others
        public string? Version { get; set; }

        #endregion



        #region CONSTRUCTORS

        public MainWindowVM()
        {
            _messageBoxService = ServiceProvider.Instance.GetService<IMessageBoxService>();
            _fileDialogService = ServiceProvider.Instance.GetService<IFileDialogService>();
            _tabNavigationService = ServiceProvider.Instance.GetService<ITabNavigationService>();
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            _schedulerService = ServiceProvider.Instance.GetService<ISchedulerService>();

            NewProjectCommand = new RelayCommand<object>(param => NewProject());
            OpenProjectCommand = new RelayCommand<object>(param => OpenProject());
            OpenRecentProjectCommand = new RelayCommand<string>(OpenProject);
            SaveProjectCommand = new RelayCommand<object>(param => SaveProject());
            SaveAsProjectCommand = new RelayCommand<object>(param => SaveAsProject());
            ProjectSettingsCommand = new RelayCommand<object>(param => ProjectSettings());
            NewClassroomCommand = new RelayCommand<object>(param => NewClassroom());
            NewTeacherCommand = new RelayCommand<object>(param => NewTeacher());
            NewGroupCommand = new RelayCommand<object>(param => NewGroup());
            EditClassroomCommand = new RelayCommand<ClassroomVM>(EditClassroom);
            EditTeacherCommand = new RelayCommand<TeacherVM>(EditTeacher);
            EditGroupCommand = new RelayCommand<GroupVM>(EditGroup);
            RemoveClassroomCommand = new RelayCommand<ClassroomVM>(RemoveClassroom);
            RemoveTeacherCommand = new RelayCommand<TeacherVM>(RemoveTeacher);
            RemoveGroupCommand = new RelayCommand<GroupVM>(RemoveGroup);
            RemoveSubgroupCommand = new RelayCommand<SubgroupVM>(RemoveSubgroup);
            EditTimetableCommand = new RelayCommand<IUnitVM>(EditTimetable);
            AutoScheduleCommand = new RelayCommand<object>(param => AutoSchedule());
            ExportHTMLCommand = new RelayCommand<object>(param => ExportHTML());

            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }

        #endregion



        #region PRIVATE METHODS

        private void NewProject()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                MessageBoxQuestionResult result = _messageBoxService.ShowQuestion(Resources.Main_Message_SaveCurrentProject);

                switch (result)
                {
                    case MessageBoxQuestionResult.Yes: SaveProject(); break;
                    case MessageBoxQuestionResult.No: break;
                    default: return;
                }
            }

            _tabNavigationService.CloseAll();
            _projectService.New();
            ProjectSettings();
        }

        private void OpenProject()
        {
            string? path = _fileDialogService.OpenFile(_projectFileTypes);
            if (path is not null)
            {
                OpenProject(path);
            }
        }

        private void OpenProject(string path)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                MessageBoxQuestionResult result = _messageBoxService.ShowQuestion(Resources.Main_Message_SaveCurrentProject);

                switch (result)
                {
                    case MessageBoxQuestionResult.Yes: SaveProject(); break;
                    case MessageBoxQuestionResult.No: break;
                    default: return;
                }
            }

            _projectService.Load(path);
            _tabNavigationService.CloseAll();
        }

        private void SaveProject()
        {
            string? path = _projectService.SavePath;
            if (_projectService.SavePath is null)
            {
                path = _fileDialogService.SaveFile(_projectFileTypes);
            }
            
            if (path is not null)
            {
                _projectService.Save(path);
            }
        }
        private void SaveAsProject()
        {
            string? path = _fileDialogService.SaveFile(_projectFileTypes);
            if (path is not null)
            {
                _projectService.Save(path);
            }
        }

        private void ProjectSettings()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem projectSettingsTab = new TabItem()
                {
                    Title = Resources.Tabs_ProjectSettings,
                    IsClosable = true,
                    ViewModel = new ProjectSettingsViewVM()
                };
                _tabNavigationService.AddAndActivate(projectSettingsTab);
            }
        }

        private void NewClassroom()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                ulong id = 0;
                if (ProjectService.ProjectViewModel.Classrooms.Count() > 0)
                {
                    id = ProjectService.ProjectViewModel.Classrooms.Select(x => x.Id).Max() + 1;
                }

                Classroom classroom = new Classroom(id)
                {
                    Name = Resources.Global_DefaultClassroomName
                };
                ClassroomVM classroomVM = new ClassroomVM(classroom);
                ProjectService.ProjectViewModel.Classrooms.Add(classroomVM);
                EditClassroom(classroomVM);
            }
        }
        private void NewTeacher()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                ulong id = 0;
                if (ProjectService.ProjectViewModel.Teachers.Count() > 0)
                {
                    id = ProjectService.ProjectViewModel.Teachers.Select(x => x.Id).Max() + 1;
                }

                Teacher teacher = new Teacher(id)
                {
                    Name = Resources.Global_DefaultTeacherName
                };
                TeacherVM teacherVM = new TeacherVM(teacher);
                ProjectService.ProjectViewModel.Teachers.Add(teacherVM);
                EditTeacher(teacherVM);
            }
        }
        private void NewGroup()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                ulong id = 0;
                if (ProjectService.ProjectViewModel.Groups.Count() > 0)
                {
                    id = ProjectService.ProjectViewModel.Groups.Select(x => x.Id).Max() + 1;
                }

                Group group = new Group(id)
                {
                    Name = Resources.Global_DefaultGroupName
                };
                GroupVM groupVM = new GroupVM(group);
                ProjectService.ProjectViewModel.Groups.Add(groupVM);
                EditGroup(groupVM);
            }
        }

        private void EditClassroom(ClassroomVM classroom) => EditUnit(new ClassroomEditorViewVM(classroom), Resources.Tabs_ClassroomEdit);
        private void EditTeacher(TeacherVM teacher) => EditUnit(new TeacherEditorViewVM(teacher), Resources.Tabs_TeacherEdit);
        private void EditGroup(GroupVM group) => EditUnit(new GroupEditorViewVM(group), Resources.Tabs_GroupEdit);
        private void EditUnit(IUnitEditorViewVM edit, string tabNamePrefix)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem groupEditTab = new TabItem()
                {
                    Title = $"{tabNamePrefix}: {edit.Unit.Name}",
                    IsClosable = true,
                    ViewModel = edit
                };
                _tabNavigationService.AddAndActivate(groupEditTab);
            }
        }

        private void RemoveClassroom(ClassroomVM classroom) => RemoveUnit(classroom, ProjectService.ProjectViewModel?.Classrooms, Resources.Main_Treeview_Classrooms_Message_Remove);
        private void RemoveTeacher(TeacherVM teacher) => RemoveUnit(teacher, ProjectService.ProjectViewModel?.Teachers, Resources.Main_Treeview_Teachers_Message_Remove);
        private void RemoveGroup(GroupVM group) => RemoveUnit(group, ProjectService.ProjectViewModel?.Groups, Resources.Main_Treeview_Groups_Message_Remove);
        private void RemoveSubgroup(SubgroupVM subgroup) => RemoveUnit(subgroup, ProjectService.ProjectViewModel?.Subgroups, Resources.Main_Treeview_Subgroups_Message_Remove);
        private void RemoveUnit(IRemovableVM unit, IList? collection, string questionMessage)
        {
            if (collection is not null)
            {
                MessageBoxQuestionResult result = _messageBoxService.ShowQuestion(questionMessage, true);
                if (result == MessageBoxQuestionResult.Yes)
                {
                    collection.Remove(unit);
                    _tabNavigationService.Close(_tabNavigationService.Tabs.Where(x => x.ViewModel is IUnitEditorViewVM model && model.Unit == unit));
                }
            }
        }

        private void EditTimetable(IUnitVM classUnit)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem timetableEditTab = new TabItem()
                {
                    Title = $"{Resources.Tabs_TimetableEdit}: {classUnit.Name}",
                    IsClosable = true,
                    ViewModel = new TimetableEditorViewVM(classUnit)
                };
                _tabNavigationService.AddAndActivate(timetableEditTab);
            }
        }

        private void AutoSchedule()
        {
            IEnumerable<ClassVM> unscheduledClasses = _projectService.ProjectViewModel.Classes.Where(x => x.Day is null || x.Slot is null);
            if (unscheduledClasses.Any())
            {
                List<ClassVM> errors = new List<ClassVM>();
                int successCount = 0;
                foreach (ClassVM @class in unscheduledClasses)
                {
                    (TimetableDay? day, TimetableSpan? slot) = _schedulerService.Schedule(@class);
                    if (day is not null && slot is not null)
                    {
                        @class.Day = day;
                        @class.Slot = slot;
                        successCount++;
                    }
                    else
                    {
                        errors.Add(@class);
                    }
                }

                //REFRESH: All editors & Errors
                foreach (TimetableEditorViewVM editor in _tabNavigationService.Tabs.Select(x => x.ViewModel).OfType<TimetableEditorViewVM>().Distinct())
                {
                    editor.RefreshClasses(RefreshMode.Scheduled | RefreshMode.Unscheduled);
                }
                _projectService.RefreshErrors();

                StringBuilder sb = new StringBuilder();
                sb.Append($"{Resources.Main_Ribbon_Edit_Timetable_Autoschedule_Message_SuccessfullyScheduled}: {successCount}");
                if (errors.Any())
                {
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append($"{Resources.Main_Ribbon_Edit_Timetable_Autoschedule_Message_FollowingClassesCouldNotBeScheduled}:");
                    sb.AppendLine();
                    int deleted = 0;
                    for (int i = 0; i < errors.Count && i < 5; i++)
                    {
                        ClassVM errorClass = errors[i];
                        string unit;
                        string unitName;
                        if (errorClass.Group is not null)
                        {
                            unit = Resources.Main_Ribbon_Edit_Timetable_Autoschedule_Message_UnitGroup;
                            unitName = errorClass.Group.Name;
                        }
                        else if (errorClass.Teacher is not null)
                        {
                            unit = Resources.Main_Ribbon_Edit_Timetable_Autoschedule_Message_UnitTeacher;
                            unitName = errorClass.Teacher.Name;
                        }
                        else if (errorClass.Classroom is not null)
                        {
                            unit = Resources.Main_Ribbon_Edit_Timetable_Autoschedule_Message_UnitClassroom;
                            unitName = errorClass.Classroom.Name;
                        }
                        else
                        {
                            deleted++;
                            i--;
                            _projectService.ProjectViewModel.Classes.Remove(errorClass);
                            continue;
                        }
                        sb.Append($"- {errorClass.Name} ({unit}: {unitName})");
                        sb.AppendLine();
                    }
                    if (errors.Count - deleted > 5)
                    {
                        sb.Append($"+ {errors.Count - deleted - 5} {Resources.Main_Ribbon_Edit_Timetable_Autoschedule_Message_More}");
                    }

                    _messageBoxService.ShowWarning(sb.ToString());
                }
                else
                {
                    _messageBoxService.ShowInformation(sb.ToString());
                }
            }
            else
            {
                _messageBoxService.ShowInformation(Resources.Main_Ribbon_Edit_Timetable_Autoschedule_Message_NoUnscheduledClasses);
            }
        }

        private void ExportHTML()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem exportHTMLTab = new TabItem()
                {
                    Title = Resources.Tabs_ExportHTML,
                    IsClosable = true,
                    ViewModel = new ExportHTMLViewVM()
                };
                _tabNavigationService.AddAndActivate(exportHTMLTab);
            }
        }

        #endregion
    }
}
