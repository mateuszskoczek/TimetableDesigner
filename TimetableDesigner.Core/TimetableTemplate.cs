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
        private TimetableSpanCollection _slots;

        #endregion



        #region PROPERTIES

        public IEnumerable<TimetableDay> Days => _days;
        public IEnumerable<TimetableSpan> Slots => _slots;

        #endregion



        #region CONSTRUCTORS

        public TimetableTemplate()
        {
            _days = new List<TimetableDay>();
            _slots = new TimetableSpanCollection();
        }

        #endregion



        #region PUBLIC METHODS

        public void AddDay(TimetableDay name) => _days.Add(name);

        public bool RemoveDay(TimetableDay day) => _days.Remove(day);

        public void AddSlot(TimetableSpan slot) => _slots.Add(slot);

        public bool RemoveSlot(TimetableSpan slot) => _slots.Remove(slot);

        #endregion
    }
}
