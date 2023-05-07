using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class Group : BaseGroup
    {
        #region FIELDS

        private string _description;
        private HashSet<Subgroup> _assignedSubgroups;

        #endregion



        #region PROPERTIES

        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public ICollection<Subgroup> AssignedSubgroups => _assignedSubgroups;

        #endregion



        #region CONSTRUCTORS

        public Group() : base()
        {
            _description = string.Empty;
            _assignedSubgroups = new HashSet<Subgroup>();
        }

        #endregion
    }
}
