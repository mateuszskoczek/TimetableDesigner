using System;
using System.Diagnostics;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class TimetableSpan
    {
        #region FIELDS

        private TimeOnly _from;
        private TimeOnly _to;

        #endregion



        #region PROPERTIES

        public TimeOnly From => _from;
        public TimeOnly To => _to;

        #endregion



        #region CONSTRUCTORS

        public TimetableSpan(TimeOnly from, TimeOnly to)
        {
            if (to <= from)
            {
                throw new ArgumentException("Ending value (\"to\") of TimetableSpan have to be greater than starting value (\"from\")");
            }

            _from = from;
            _to = to;
        }

        #endregion



        #region PUBLIC METHODS

        public override bool Equals(object? obj) => obj is TimetableSpan slot && From == slot.From && To == slot.To;

        public override int GetHashCode() => HashCode.Combine(From, To);

        public override string? ToString() => $"{From} - {To}";

        public TimetableSpanCollision CheckCollision(TimetableSpan slot)
        {
            if (slot.To <= this.From)
            {
                return TimetableSpanCollision.CheckedSlotBefore;
            }
            else if (this.To <= slot.From)
            {
                return TimetableSpanCollision.CheckedSlotAfter;
            }
            else
            {
                if (this.From <= slot.From && slot.To <= this.To)
                {
                    return TimetableSpanCollision.CheckedSlotIn;
                }
                else if (this.From < slot.From && slot.From < this.To && this.To <= slot.To)
                {
                    return TimetableSpanCollision.CheckedSlotFromIn;
                }
                else if (slot.From < this.From && this.From < slot.To && slot.To <= this.To)
                {
                    return TimetableSpanCollision.CheckedSlotToIn;
                }
                else
                {
                    throw new ArgumentException("Unknown collision");
                }
            }
        }

        #endregion
    }
}
