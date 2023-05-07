using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class TimetableSpanCollection : ICollection<TimetableSpan>
    {
        #region FIELDS

        private List<TimetableSpan> _list;

        #endregion



        #region PROPERTIES

        public int Count => _list.Count;
        public bool IsReadOnly => ((ICollection<TimetableSpan>)_list).IsReadOnly;

        #endregion



        #region CONSTRUCTORS

        public TimetableSpanCollection()
        {
            _list = new List<TimetableSpan>();
        }

        #endregion



        #region PUBLIC METHODS

        public void Add(TimetableSpan item)
        {
            int i = 0;
            if (Count > 0)
            {
                bool done = false;
                while (i < Count && !done)
                {
                    switch (item.CheckCollision(_list.ElementAt(i)))
                    {
                        case TimetableSpanCollision.CheckedSlotBefore: i++; break;
                        case TimetableSpanCollision.CheckedSlotAfter: done ^= true; break;
                        default: throw new ArgumentException("Slot collide with another slot");
                    }
                }
            }
            _list.Insert(i, item);
        }

        public void Clear() => _list.Clear();

        public bool Contains(TimetableSpan item) => _list.Contains(item);

        public void CopyTo(TimetableSpan[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public IEnumerator<TimetableSpan> GetEnumerator() => _list.GetEnumerator();

        public bool Remove(TimetableSpan item) => _list.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

        #endregion
    }
}
