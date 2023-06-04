using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class Classroom : Unit
    {
        #region FIELDS

        private string _description;
        private bool _isCapacityLimited;
        private uint _capacity;

        #endregion



        #region PROPERTIES

        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public bool IsCapacityLimited
        {
            get => _isCapacityLimited;
            set => _isCapacityLimited = value;
        }
        public uint Capacity
        {
            get => _capacity;
            set => _capacity = value;
        }

        #endregion



        #region CONSTRUCTORS

        public Classroom(ulong id) : base(id)
        {
            _description = string.Empty;
            _isCapacityLimited = false;
            _capacity = 1;
        }

        #endregion
    }
}
