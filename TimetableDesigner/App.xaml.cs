global using TimetableDesigner.Services;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimetableDesigner.Services.FileDialog;
using TimetableDesigner.Services.MessageBox;
using TimetableDesigner.Services.Project;
using TimetableDesigner.Services.Scheduler;
using TimetableDesigner.Services.TabNavigation;
using TimetableDesigner.ViewModels.Views;
using TimetableDesigner.Views;

namespace TimetableDesigner
{
    public partial class App : Application
    {
        #region FIELDS

        private IMessageBoxService _messageBoxService;
        private IFileDialogService _fileDialogService;
        private ITabNavigationService _tabNavigationService;
        private IProjectService _projectService;
        private ISchedulerService _schedulerService;

        #endregion



        #region CONSTRUCTORS

        public App()
        {
            _messageBoxService = new MessageBoxService();
            _fileDialogService = new FileDialogService();
            _tabNavigationService = new TabNavigationService();
            _projectService = new ProjectService();
            _schedulerService = new SchedulerService(_projectService);

            ServiceProvider.Instance.AddService(_messageBoxService);
            ServiceProvider.Instance.AddService(_fileDialogService);
            ServiceProvider.Instance.AddService(_tabNavigationService);
            ServiceProvider.Instance.AddService(_projectService);
            ServiceProvider.Instance.AddService(_schedulerService);
        }

        #endregion



        #region PRIVATE METHODS

        private void OnStartup(object sender, StartupEventArgs e)
        {
            if (!Directory.Exists(Globals.Path.ApplicationData))
            {
                Directory.CreateDirectory(Globals.Path.ApplicationData);
            }

            _projectService.LoadRecentProjectsList();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            if (e.Args.Length > 0 && File.Exists(e.Args[0]))
            {
                _projectService.Load(e.Args[0]);
            }
            else
            {
                TabItem welcomeTab = new TabItem()
                {
                    Title = TimetableDesigner.Properties.Resources.Tabs_Welcome,
                    ViewModel = new WelcomeViewVM()
                };
                _tabNavigationService.AddAndActivate(welcomeTab);
            }
        }

        #endregion
    }
}
