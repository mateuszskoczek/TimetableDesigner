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

        public ICollection<TimetableDay> Days => _days;
        public ICollection<TimetableSpan> Slots => _slots;

        #endregion



        #region CONSTRUCTORS

        public TimetableTemplate()
        {
            _days = new List<TimetableDay>();
            _slots = new TimetableSpanCollection();
        }

        #endregion
    }
}
