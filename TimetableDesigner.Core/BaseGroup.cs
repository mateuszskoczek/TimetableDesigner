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
        #region FIELDS

        private string _shortName;

        #endregion



        #region PROPERTIES

        public string ShortName
        {
            get => _shortName;
            set => _shortName = value;
        }

        #endregion



        #region CONSTRUCTORS

        public BaseGroup(ulong id) : base(id)
        {
            _shortName = string.Empty;
        }

        #endregion
    }
}
