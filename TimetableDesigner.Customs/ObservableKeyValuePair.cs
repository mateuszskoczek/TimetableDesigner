using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Customs
{
    public class ObservableKeyValuePair<TKey, TValue> : INotifyPropertyChanged
    {
        #region FIELDS

        private TKey _key;
        private TValue _value;

        #endregion



        #region PROPERTIES

        public TKey Key
        {
            get => _key;
            set
            {
                _key = value;
                NotifyPropertyChanged(nameof(Key));
            }
        }
        public TValue Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyPropertyChanged(nameof(Value));
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ObservableKeyValuePair() : this(default, default)
        { }

        public ObservableKeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        #endregion



        #region PRIVATE METHODS

        private void NotifyPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion



        #region EVENTS

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }
}
