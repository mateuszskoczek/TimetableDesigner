using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class Class
    {
        #region FIELDS

        private string _name;
        private Teacher? _teacher;
        private BaseGroup? _group;
        private Classroom? _classroom;
        private TimetableDay? _day;
        private TimetableSpan? _slot;
        private byte[] _color;

        #endregion



        #region PROPERTIES

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public Teacher? Teacher
        {
            get => _teacher;
            set => _teacher = value;
        }
        public BaseGroup? Group
        {
            get => _group;
            set => _group = value;
        }
        public Classroom? Classroom
        {
            get => _classroom;
            set => _classroom = value;
        }
        public TimetableDay? Day
        {
            get => _day;
            set => _day = value;
        }
        public TimetableSpan? Slot
        {
            get => _slot;
            set => _slot = value;
        }
        public byte[] Color
        {
            get => _color;
            set => _color = value;
        }

        #endregion



        #region CONSTRUCTORS

        public Class()
        {
            _name = string.Empty;
            _teacher = null;
            _group = null;
            _classroom = null;
            _day = null;
            _slot = null;
            _color = new byte[3] { 0xFA, 0x5A, 0x5A };
        }

        #endregion
    }
}
