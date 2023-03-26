using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;

namespace TimetableDesigner.ViewModels.Models
{
    public interface IGroupViewModel
    {
        #region PROPERTIES

        IGroup Group { get; }
        string Name { get; }

        #endregion
    }
}
