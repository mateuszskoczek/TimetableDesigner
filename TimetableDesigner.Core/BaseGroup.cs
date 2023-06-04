using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public abstract class BaseGroup : Unit
    {
        #region CONSTRUCTORS

        public BaseGroup(ulong id) : base(id)
        { }

        #endregion
    }
}
