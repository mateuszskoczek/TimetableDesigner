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
        #region PROPERTIES

        public string Name { get; set; }

        #endregion



        #region CONSTRUCTORS

        public TimetableDay(string name) 
        { 
            Name = name;
        }

        #endregion
    }
}
