using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;

namespace TimetableDesigner.ViewModels.Models
{
    public class TimetableTemplateViewModel : BaseModelViewModel
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

        public TimetableTemplateViewModel(TimetableTemplate timetableTemplate)
        {
            _timetableTemplate = timetableTemplate;
        }

        #endregion



        #region PUBLIC METHODS

        public void AddDay(TimetableDay day)
        {
            _timetableTemplate.AddDay(day);
            NotifyPropertyChanged(nameof(Days));
        }

        public void RemoveDay(TimetableDay day)
        {
            _timetableTemplate.RemoveDay(day);
            NotifyPropertyChanged(nameof(Days));
        }

        public void AddSlot(TimetableSpan slot)
        {
            _timetableTemplate.AddSlot(slot);
            NotifyPropertyChanged(nameof(Slots));
        }

        public void RemoveSlot(TimetableSpan slot)
        {
            _timetableTemplate.RemoveSlot(slot);
            NotifyPropertyChanged(nameof(Slots));
        }

        #endregion
    }
}
