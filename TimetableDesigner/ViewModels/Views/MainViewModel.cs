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

namespace TimetableDesigner.ViewModels.Views
{
    class MainViewModel : BaseViewViewModel
    {
        #region FIELDS

        private IMessageBoxService _messageBoxService;
        private ITabNavigationService _tabNavigationService;
        private IProjectService _projectService;

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
        public ICommand ProjectSettingsCommand { get; set; }
        public ICommand NewClassroomCommand { get; set; }
        public ICommand EditClassroomCommand { get; set; }
        public ICommand RemoveClassroomCommand { get; set; }
        public ICommand NewTeacherCommand { get; set; }
        public ICommand EditTeacherCommand { get; set; }
        public ICommand RemoveTeacherCommand { get; set; }
        public ICommand NewGroupCommand { get; set; }
        public ICommand EditGroupCommand { get; set; }
        public ICommand RemoveGroupCommand { get; set; }
        public ICommand RemoveSubgroupCommand { get; set; }

        // Others
        public string Version { get; set; }

        #endregion



        #region CONSTRUCTORS

        public MainViewModel()
        {
            _messageBoxService = ServiceProvider.Instance.GetService<IMessageBoxService>();
            _tabNavigationService = ServiceProvider.Instance.GetService<ITabNavigationService>();
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();

            NewProjectCommand = new RelayCommand<object>(param => NewProject());
            OpenProjectCommand = new RelayCommand<object>(param => OpenProject());
            ProjectSettingsCommand = new RelayCommand<object>(param => ProjectSettings());
            NewClassroomCommand = new RelayCommand<object>(param => NewClassroom());
            EditClassroomCommand = new RelayCommand<ClassroomViewModel>(EditClassroom);
            RemoveClassroomCommand = new RelayCommand<ClassroomViewModel>(DeleteClassroom);
            NewTeacherCommand = new RelayCommand<object>(param => NewTeacher());
            EditTeacherCommand = new RelayCommand<TeacherViewModel>(EditTeacher);
            RemoveTeacherCommand = new RelayCommand<TeacherViewModel>(DeleteTeacher);
            NewGroupCommand = new RelayCommand<object>(param => NewGroup());
            EditGroupCommand = new RelayCommand<GroupViewModel>(EditGroup);
            RemoveGroupCommand = new RelayCommand<GroupViewModel>(DeleteGroup);
            RemoveSubgroupCommand = new RelayCommand<SubgroupViewModel>(DeleteSubgroup);

            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            TabItem welcomeTab = new TabItem()
            {
                Title = Resources.Tabs_Welcome,
                ViewModel = new WelcomeViewModel()
            };
            _tabNavigationService.AddAndActivate(welcomeTab);
        }

        #endregion



        #region PRIVATE METHODS

        private void OpenProject()
        {
        }

        private void NewProject()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                MessageBoxQuestionResult result = _messageBoxService.ShowQuestion(Resources.Main_Message_SaveCurrentProject);

                switch (result)
                {
                    case MessageBoxQuestionResult.Yes: break;
                    case MessageBoxQuestionResult.No: break;
                    default: return;
                }
            }
            _tabNavigationService.CloseAll();
            _projectService.New();
            ProjectSettings();
        }

        private void ProjectSettings()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem projectSettingsTab = new TabItem()
                {
                    Title = Resources.Tabs_ProjectSettings,
                    IsClosable = true,
                    ViewModel = new ProjectSettingsViewModel()
                };
                _tabNavigationService.AddAndActivate(projectSettingsTab);
            }
        }

        private void NewClassroom()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                Classroom classroom = new Classroom()
                {
                    Name = Resources.Global_DefaultClassroomName
                };
                ClassroomViewModel classroomVM = new ClassroomViewModel(classroom);
                ProjectService.ProjectViewModel.Classrooms.Add(classroomVM);
                EditClassroom(classroomVM);
            }
        }

        private void EditClassroom(ClassroomViewModel classroomViewModel)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem classroomEditTab = new TabItem()
                {
                    Title = $"{Resources.Tabs_ClassroomEdit}: {classroomViewModel.Name}",
                    IsClosable = true,
                    ViewModel = new ClassroomEditViewModel(classroomViewModel)
                };
                _tabNavigationService.AddAndActivate(classroomEditTab);
            }  
        }

        private void DeleteClassroom(ClassroomViewModel classroomViewModel)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                ProjectService.ProjectViewModel.Classrooms.Remove(classroomViewModel);
            }
        }

        private void NewTeacher()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                Teacher teacher = new Teacher()
                {
                    Name = Resources.Global_DefaultTeacherName
                };
                TeacherViewModel teacherVM = new TeacherViewModel(teacher);
                ProjectService.ProjectViewModel.Teachers.Add(teacherVM);
                EditTeacher(teacherVM);
            }
        }

        private void EditTeacher(TeacherViewModel teacherViewModel)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem teacherEditTab = new TabItem()
                {
                    Title = $"{Resources.Tabs_TeacherEdit}: {teacherViewModel.Name}",
                    IsClosable = true,
                    ViewModel = new TeacherEditViewModel(teacherViewModel)
                };
                _tabNavigationService.AddAndActivate(teacherEditTab);
            }
        }

        private void DeleteTeacher(TeacherViewModel teacherViewModel)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                ProjectService.ProjectViewModel.Teachers.Remove(teacherViewModel);
            }
        }

        private void NewGroup()
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                Group group = new Group()
                {
                    Name = Resources.Global_DefaultGroupName
                };
                GroupViewModel groupVM = new GroupViewModel(group);
                ProjectService.ProjectViewModel.Groups.Add(groupVM);
                EditGroup(groupVM);
            }
        }

        private void EditGroup(GroupViewModel groupViewModel)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                TabItem groupEditTab = new TabItem()
                {
                    Title = $"{Resources.Tabs_GroupEdit}: {groupViewModel.Name}",
                    IsClosable = true,
                    ViewModel = new GroupEditViewModel(groupViewModel)
                };
                _tabNavigationService.AddAndActivate(groupEditTab);
            }
        }

        private void DeleteGroup(GroupViewModel groupViewModel)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                ProjectService.ProjectViewModel.Groups.Remove(groupViewModel);
            }
        }

        private void DeleteSubgroup(SubgroupViewModel subgroupViewModel)
        {
            if (ProjectService.ProjectViewModel is not null)
            {
                MessageBoxQuestionResult result = _messageBoxService.ShowQuestion(Resources.Main_Treeview_Subgroups_Message_Remove, true);
                if (result == MessageBoxQuestionResult.Yes)
                {
                    foreach (GroupViewModel group in ProjectService.ProjectViewModel.Groups)
                    {
                        group.RemoveSubgroup(subgroupViewModel);
                    }
                    _projectService.ProjectViewModel.Subgroups.Remove(subgroupViewModel);
                }
            }
        }

        #endregion
    }
}
