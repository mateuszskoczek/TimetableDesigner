using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Customs;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.Services.Project
{
    public interface IProjectService : IService
    {
        #region PROPERTIES

        Core.Project? Project { get; }
        ProjectVM? ProjectViewModel { get; }

        ObservableCollection<ProjectError> Errors { get; }

        string? SavePath { get; }
        ObservableDictionary<Guid, RecentProjectEntry> RecentProjects { get; }

        #endregion



        #region METHODS

        void New();
        void Load(string path);
        void Save(string path);

        void RefreshErrors();

        void LoadRecentProjectsList();
        void SaveRecentProjectsList();

        #endregion
    }
}
