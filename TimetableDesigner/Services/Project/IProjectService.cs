using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.Services.Project
{
    public interface IProjectService : IService
    {
        #region PROPERTIES

        Core.Project? Project { get; }
        ProjectViewModel? ProjectViewModel { get; }

        #endregion



        #region METHODS

        void New();

        void Load(string path);

        void Save(string path);

        #endregion
    }
}
