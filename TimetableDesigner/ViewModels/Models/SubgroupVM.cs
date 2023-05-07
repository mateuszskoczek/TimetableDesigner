using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public class SubgroupVM : BaseGroupVM, IRemovableVM
    {
        #region PROPERTIES

        public Subgroup Subgroup => (Subgroup)_baseGroup;

        #endregion



        #region CONSTRUCTORS

        public SubgroupVM(Subgroup subgroup) : base(subgroup)
        { }

        #endregion
    }
}
