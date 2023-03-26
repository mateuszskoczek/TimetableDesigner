using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Customs
{
    public class ObservableObject : INotifyPropertyChanged
    {
        #region PRIVATE METHODS

        protected void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion



        #region EVENTS

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }
}
