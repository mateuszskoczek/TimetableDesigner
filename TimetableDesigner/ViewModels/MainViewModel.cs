using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TimetableDesigner.Commands;
using TimetableDesigner.ViewModels.Base;
using System.Windows.Navigation;
using TimetableDesigner.Core;
using System.Windows;
using TimetableDesigner.Properties;
using TimetableDesigner.MessageBox;
using System.Reflection;
using TimetableDesigner.ViewModels.Models;
using System.Windows.Data;

namespace TimetableDesigner.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region FIELDS

        private ProjectViewModel? _project;

        private ObservableCollection<BaseTabViewModel> _tabs;
        private BaseTabViewModel? _selectedTab;

        #endregion



        #region PROPERTIES

        public string Version { get; set; }

        public ICommand CloseTabCommand { get; set; }
        public ICommand NewProjectCommand { get; set; }
        public ICommand OpenProjectCommand { get; set; }
        public ICommand ProjectSettingsCommand { get; set; }
        public ICommand NewClassroomCommand { get; set; }
        public ICommand EditClassroomCommand { get; set; }
        public ICommand RemoveClassroomCommand { get; set; }
        public ICommand NewTeacherCommand { get; set; }
        public ICommand EditTeacherCommand { get; set; }
        public ICommand RemoveTeacherCommand { get; set; }

        public ObservableCollection<BaseTabViewModel> Tabs
        {
            get => _tabs;
            set
            {
                _tabs = value;
                NotifyPropertyChanged(nameof(Tabs));
            }
        }
        public BaseTabViewModel? SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                NotifyPropertyChanged(nameof(SelectedTab));
            }
        }

        public ProjectViewModel? Project 
        {
            get => _project;
            set
            {
                if (value != _project)
                {
                    _project = value;
                    NotifyPropertyChanged(nameof(Project));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public MainViewModel()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            CloseTabCommand = new RelayCommand<BaseTabViewModel>(CloseTab);
            NewProjectCommand = new RelayCommand<object>(param => NewProject());
            OpenProjectCommand = new RelayCommand<object>(param => OpenProject());
            ProjectSettingsCommand = new RelayCommand<object>(param => ProjectSettings());
            NewClassroomCommand = new RelayCommand<object>(param => NewClassroom());
            EditClassroomCommand = new RelayCommand<ClassroomViewModel>(EditClassroom);
            RemoveClassroomCommand = new RelayCommand<ClassroomViewModel>(DeleteClassroom);
            NewTeacherCommand = new RelayCommand<object>(param => NewTeacher());
            EditTeacherCommand = new RelayCommand<TeacherViewModel>(EditTeacher);
            RemoveTeacherCommand = new RelayCommand<TeacherViewModel>(DeleteTeacher);

            _tabs = new ObservableCollection<BaseTabViewModel>
            {
                new WelcomeTabViewModel() { TabTitle = Resources.Tabs_Welcome }
            };
            _selectedTab = Tabs.Last();
        }

        #endregion



        #region PUBLIC METHODS

        private void CloseTab(BaseTabViewModel tab)
        {
            if (SelectedTab == tab)
            {
                int index = Tabs.IndexOf(tab);
                int before = index - 1;
                int after = index + 1;

                BaseTabViewModel newSelectedTab = null;
                if (before >= 0)
                {
                    newSelectedTab = Tabs[before];
                }
                else if (after >= 0 && after < Tabs.Count - 1) 
                {
                    newSelectedTab = Tabs[after];
                }
                SelectedTab = newSelectedTab;
            }
            Tabs.Remove(tab);
        }

        private void OpenProject()
        {
        }

        private void NewProject()
        {
            if (Project is not null)
            {
                MessageBoxYesNoCancelResult result = MessageBoxService.ShowYesNoCancelQuestion(Resources.Main_Message_SaveCurrentProject);

                switch (result)
                {
                    case MessageBoxYesNoCancelResult.Yes: break;
                    case MessageBoxYesNoCancelResult.No: break;
                    default: return;
                }
            }
            Tabs.Clear();

            Project project = new Project()
            {
                Name = Resources.Global_DefaultProjectName,
            };
            Project = new ProjectViewModel(project);
            ProjectSettings();
        }

        private void ProjectSettings()
        {
            if (Project is not null)
            {
                ProjectSettingsTabViewModel projectSettingsTabVM = new ProjectSettingsTabViewModel()
                {
                    Project = Project,
                    TabTitle = Resources.Tabs_ProjectSettings,
                    IsTabClosable = true,
                };
                Tabs.Add(projectSettingsTabVM);
                SelectedTab = Tabs.Last();
            }
        }

        private void NewClassroom()
        {
            if (Project is not null)
            {
                Classroom classroom = new Classroom()
                {
                    Name = Resources.Global_DefaultClassroomName
                };
                ClassroomViewModel classroomVM = new ClassroomViewModel(classroom);
                Project.Classrooms.Add(classroomVM);
                EditClassroom(classroomVM);
            }
        }

        private void EditClassroom(ClassroomViewModel classroomViewModel)
        {
            ClassroomEditTabViewModel classroomEdit = new ClassroomEditTabViewModel(classroomViewModel)
            {
                TabTitle = $"{Resources.Tabs_ClassroomEdit}: {classroomViewModel.Name}",
                IsTabClosable = true                
            };
            Tabs.Add(classroomEdit);
            SelectedTab = classroomEdit;
        }

        private void DeleteClassroom(ClassroomViewModel classroomViewModel)
        {
            if (Project is not null)
            {
                Project.Classrooms.Remove(classroomViewModel);
            }
        }

        private void NewTeacher()
        {
            if (Project is not null)
            {
                Teacher teacher = new Teacher()
                {
                    Name = Resources.Global_DefaultTeacherName
                };
                TeacherViewModel teacherVM = new TeacherViewModel(teacher);
                Project.Teachers.Add(teacherVM);
                EditTeacher(teacherVM);
            }
        }

        private void EditTeacher(TeacherViewModel teacherViewModel)
        {
            if (Project is not null)
            {
                TeacherEditTabViewModel teacherEdit = new TeacherEditTabViewModel(teacherViewModel, Project.TimetableTemplate)
                {
                    Teacher = teacherViewModel,
                    TabTitle = $"{Resources.Tabs_TeacherEdit}: {teacherViewModel.Name}",
                    IsTabClosable = true
                };
                Tabs.Add(teacherEdit);
                SelectedTab = teacherEdit;
            }
            
        }

        private void DeleteTeacher(TeacherViewModel teacherViewModel)
        {
            if (Project is not null)
            {
                Project.Teachers.Remove(teacherViewModel);
            }
        }

        #endregion
    }
}
