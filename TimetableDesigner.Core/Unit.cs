using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public abstract class Unit
    {
        #region FIELDS

        private ulong _id;
        private Guid _guid;
        private string _name;

        #endregion



        #region PROPERTIES

        public ulong Id => _id;
        public Guid Guid => _guid;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        #endregion



        #region CONSTRUCTORS

        public Unit(ulong id)
        {
            _id = id;
            _guid = Guid.NewGuid();
            _name = string.Empty;
        }

        #endregion
    }
}
