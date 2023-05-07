using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public enum TimetableSpanCollision
    {
        CheckedSlotBefore,
        CheckedSlotAfter,
        CheckedSlotIn,
        CheckedSlotFromIn,
        CheckedSlotToIn
    }
}
