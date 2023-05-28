using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;

namespace TimetableDesigner.ViewModels.Models.Base
{
    public interface IUnitVM : IModelVM
    {
        #region PROPERTIES
        
        Unit Unit { get; }
        Guid Guid { get; }
        ulong Id { get; }
        string Name { get; }

        #endregion
    }
}
