using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Customs;

namespace TimetableDesigner.Services.Project
{
    public class RecentProjectEntry : ObservableObject
    {
        #region FIELDS

        private string _name;
        private DateTime _saveDate;
        private string _path;

        #endregion



        #region PROPERTIES

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public DateTime SaveDate
        {
            get => _saveDate;
            set
            {
                if (_saveDate != value)
                {
                    _saveDate = value;
                    NotifyPropertyChanged(nameof(SaveDate));
                }
            }
        }
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    NotifyPropertyChanged(nameof(Path));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public RecentProjectEntry(string name, DateTime saveDate, string path) 
        { 
            Name = name;
            SaveDate = saveDate;
            Path = path;
        }

        #endregion
    }
}
