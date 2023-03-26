using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimetableDesigner.Services;
using TimetableDesigner.Services.MessageBox;
using TimetableDesigner.Services.Project;
using TimetableDesigner.Services.TabNavigation;

namespace TimetableDesigner
{
    public partial class App : Application
    {
        public App()
        {
            ServiceProvider.Instance.AddService<IMessageBoxService>(new MessageBoxService());
            ServiceProvider.Instance.AddService<ITabNavigationService>(new TabNavigationService());
            ServiceProvider.Instance.AddService<IProjectService>(new ProjectService());
        }
    }
}
