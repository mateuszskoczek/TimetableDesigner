using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;

namespace TimetableDesigner.ViewModels.Models
{
    public class TimetableTemplateVM : ObservableObject, IModelVM
    {
        #region FIELDS

        private TimetableTemplate _timetableTemplate;

        #endregion



        #region PROPERTIES

        public TimetableTemplate TimetableTemplate => _timetableTemplate;

        public ObservableCollection<TimetableDay> Days => new ObservableCollection<TimetableDay>(_timetableTemplate.Days);
        public ObservableCollection<TimetableSpan> Slots => new ObservableCollection<TimetableSpan>(_timetableTemplate.Slots);

        #endregion



        #region CONSTRUCTORS

        public TimetableTemplateVM(TimetableTemplate timetableTemplate)
        {
            _timetableTemplate = timetableTemplate;
        }

        #endregion



        #region PUBLIC METHODS

        public void AddDay(TimetableDay day)
        {
            _timetableTemplate.Days.Add(day);
            NotifyPropertyChanged(nameof(Days));
        }

        public void RemoveDay(TimetableDay day)
        {
            _timetableTemplate.Days.Remove(day);
            NotifyPropertyChanged(nameof(Days));
        }

        public void AddSlot(TimetableSpan slot)
        {
            _timetableTemplate.Slots.Add(slot);
            NotifyPropertyChanged(nameof(Slots));
        }

        public void RemoveSlot(TimetableSpan slot)
        {
            _timetableTemplate.Slots.Remove(slot);
            NotifyPropertyChanged(nameof(Slots));
        }

        #endregion
    }
}
