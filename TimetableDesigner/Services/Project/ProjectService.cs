using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Properties;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.Services.Project
{
    public class ProjectService : ObservableObject, IProjectService
    {
        #region FIELDS

        private Core.Project? _project;
        private ProjectViewModel? _projectViewModel;

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
        public ProjectViewModel? ProjectViewModel
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

        #endregion



        #region CONSTRUCTORS

        public ProjectService()
        {
            _project = null;
            _projectViewModel = null;
        }

        #endregion



        #region PUBLIC METHODS

        public void New()
        {
            Project = new Core.Project()
            {
                Name = Resources.Global_DefaultProjectName,
            };
            ProjectViewModel = new ProjectViewModel(Project);
        }

        public void Load(string path)
        {

        }

        public void Save(string path)
        {

        }

        #endregion
    }
}
