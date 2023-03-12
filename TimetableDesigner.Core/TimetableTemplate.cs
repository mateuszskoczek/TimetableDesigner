using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class TimetableTemplate 
    {
        #region FIELDS

        private List<TimetableDay> _days;
        private List<TimetableSlot> _slots;

        #endregion



        #region PROPERTIES

        public IEnumerable<TimetableDay> Days => _days;
        public IEnumerable<TimetableSlot> Slots => _slots;

        #endregion



        #region CONSTRUCTORS

        public TimetableTemplate()
        {
            _days = new List<TimetableDay>();
            _slots = new List<TimetableSlot>();
        }

        #endregion



        #region PUBLIC METHODS

        public void AddDay(TimetableDay name)
        {
            _days.Add(name);
        }

        public bool RemoveDay(TimetableDay day)
        {
            return _days.Remove(day);
        }

        public void AddSlot(TimetableSlot slot)
        {
            int i = 0;
            if (_slots.Count > 0)
            {
                bool done = false;
                while (i < _slots.Count && !done)
                {
                    switch (slot.CheckCollision(_slots[i]))
                    {
                        case TimetableSlotsCollision.CheckedSlotBefore: i++; break;
                        case TimetableSlotsCollision.CheckedSlotAfter: done ^= true; break;
                        default: throw new ArgumentException("Slot collide with another slot");
                    }
                }
            }
            _slots.Insert(i, slot);
        }

        public bool RemoveSlot(TimetableSlot slot)
        {
            return _slots.Remove(slot);
        }

        #endregion
    }
}
