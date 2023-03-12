using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    internal enum TimetableSpansCollision
    {
        CheckedSlotBefore,
        CheckedSlotAfter,
        CheckedSlotIn,
        CheckedSlotFromIn,
        CheckedSlotToIn
    }
}
