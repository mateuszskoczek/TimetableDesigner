using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public class Class
    {
        #region PROPERTIES

        public string Name { get; set; }
        public string Description { get; set; }
        public Teacher Teacher { get; set; }
        public Subgroup Subgroup { get; set; }
        public Classroom Classroom { get; set; }

        #endregion
    }
}
