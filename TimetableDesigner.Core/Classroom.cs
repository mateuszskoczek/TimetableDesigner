using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public class Classroom
    {
        #region PROPERTIES

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCapacityLimited { get; set; }
        public uint Capacity { get; set; }

        #endregion



        #region CONSTRUCTORS

        public Classroom()
        {
            Name = string.Empty;
            Description = string.Empty;
            IsCapacityLimited = false;
            Capacity = 1;
        }

        #endregion
    }
}
