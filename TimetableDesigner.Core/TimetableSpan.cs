using System;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class TimetableSpan
    {
        #region PROPERTIES

        public TimeOnly From { get; private set; }
        public TimeOnly To { get; private set; }

        #endregion



        #region CONSTRUCTORS

        public TimetableSpan(TimeOnly from, TimeOnly to)
        {
            if (to <= from)
            {
                throw new ArgumentException("\"to\" cannot be less or equal to \"from\"");
            }

            From = from;
            To = to;
        }

        #endregion



        #region PUBLIC METHODS

        internal TimetableSpansCollision CheckCollision(TimetableSpan slot)
        {
            if (slot.To <= this.From)
            {
                return TimetableSpansCollision.CheckedSlotBefore;
            }
            else if (this.To <= slot.From) 
            {
                return TimetableSpansCollision.CheckedSlotAfter;
            }
            else
            {
                if (this.From < slot.From && slot.To < this.To)
                {
                    return TimetableSpansCollision.CheckedSlotIn;
                }
                else if (this.From < slot.From && slot.From < this.To && this.To < slot.To)
                {
                    return TimetableSpansCollision.CheckedSlotFromIn;
                }
                else if (slot.From < this.From && this.From < slot.To && slot.To < this.To)
                {
                    return TimetableSpansCollision.CheckedSlotToIn;
                }
                else
                {
                    throw new ArgumentException("Unknown collision");
                }
            }
        }

        public override bool Equals(object? obj) => obj is TimetableSpan slot && From == slot.From && To == slot.To;

        public override int GetHashCode() => HashCode.Combine(From, To);

        public override string? ToString() => $"{From}-{To}";

        #endregion
    }
}
