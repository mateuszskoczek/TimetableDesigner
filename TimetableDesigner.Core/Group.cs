using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public class Group
    {
        #region PROPERTIES

        public string Name { get; set; }
        public string Description { get; set; }
        public Subgroup MainSubgroup { get; set; }
        public ICollection<Subgroup> AssignedSubgroups { get; set; }

        #endregion
    }
}
