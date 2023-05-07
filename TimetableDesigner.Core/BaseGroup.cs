using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public abstract class BaseGroup : IUnit
    {
        #region FIELDS

        private string _name;
        private string _shortName;

        #endregion



        #region PROPERTIES

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value;
        }

        #endregion



        #region CONSTRUCTORS

        public BaseGroup()
        {
            _name = string.Empty;
            _shortName = string.Empty;
        }

        #endregion
    }
}
