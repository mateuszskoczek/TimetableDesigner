using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels
{
    internal interface IUnitEditorViewVM : IViewVM
    {
        #region PROPERTIES

        IUnitVM Unit { get; }

        #endregion
    }
}
