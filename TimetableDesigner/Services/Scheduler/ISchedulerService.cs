using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.Services.Scheduler
{
    public interface ISchedulerService : IService
    {
        #region METHODS

        (TimetableDay? Day, TimetableSpan? Slot) Schedule(ClassVM @class);

        #endregion
    }
}
