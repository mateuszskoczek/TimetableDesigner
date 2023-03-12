using System;

namespace TimetableDesigner.Core
{
    [Serializable]
    public struct TimetableSlot
    {
        #region PROPERTIES

        public TimeOnly From { get; private set; }
        public TimeOnly To { get; private set; }

        #endregion



        #region CONSTRUCTORS

        public TimetableSlot(TimeOnly from, TimeOnly to)
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

        internal TimetableSlotsCollision CheckCollision(TimetableSlot slot)
        {
            if (slot.To <= this.From)
            {
                return TimetableSlotsCollision.CheckedSlotBefore;
            }
            else if (this.To <= slot.From) 
            {
                return TimetableSlotsCollision.CheckedSlotAfter;
            }
            else
            {
                if (this.From < slot.From && slot.To < this.To)
                {
                    return TimetableSlotsCollision.CheckedSlotIn;
                }
                else if (this.From < slot.From && slot.From < this.To && this.To < slot.To)
                {
                    return TimetableSlotsCollision.CheckedSlotFromIn;
                }
                else if (slot.From < this.From && this.From < slot.To && slot.To < this.To)
                {
                    return TimetableSlotsCollision.CheckedSlotToIn;
                }
                else
                {
                    throw new ArgumentException("Unknown collision");
                }
            }
        }

        public override bool Equals(object? obj) => obj is TimetableSlot slot && From == slot.From && To == slot.To;

        public override int GetHashCode() => HashCode.Combine(From, To);

        public override string? ToString() => $"{From}-{To}";

        #endregion
    }
}
