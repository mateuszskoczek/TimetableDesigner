using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public class Group : IGroup
    {
        #region PROPERTIES

        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Subgroup> AssignedSubgroups { get; set; }

        #endregion



        #region CONSTRUCTORS

        public Group()
        {
            Name = string.Empty;
            Description = string.Empty;
            AssignedSubgroups = new HashSet<Subgroup>();
        }

        #endregion
    }
}
