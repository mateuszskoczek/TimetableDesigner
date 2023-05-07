using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class TimetableDay
    {
        #region FIELDS

        private string _name;

        #endregion



        #region PROPERTIES

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        #endregion



        #region CONSTRUCTORS

        public TimetableDay(string name) 
        {
            _name = name;
        }

        #endregion
    }
}
