using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public class Teacher
    {
        #region PROPERTIES

        public string Name { get; set; }
        public string Description { get; set; }
        public IDictionary<TimetableDay, TimetableSpanCollection> AvailabilityHours { get; set; }

        #endregion



        #region CONSTRUCTORS

        public Teacher()
        {
            Name = string.Empty;
            Description = string.Empty;
            AvailabilityHours = new Dictionary<TimetableDay, TimetableSpanCollection>();
        }

        #endregion
    }
}
