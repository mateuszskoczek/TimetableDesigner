using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Customs;

namespace TimetableDesigner.Core
{
    [Serializable]
    public class Teacher : Unit
    {
        #region FIELDS

        private string _shortName;
        private string _description;
        private JsonSerializableDictionary<TimetableDay, TimetableSpanCollection> _availabilityHours;

        #endregion



        #region PROPERTIES

        public string ShortName
        {
            get => _shortName;
            set => _shortName = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public JsonSerializableDictionary<TimetableDay, TimetableSpanCollection> AvailabilityHours => _availabilityHours;

        #endregion



        #region CONSTRUCTORS

        public Teacher(ulong id) : base(id)
        {
            _shortName = string.Empty;
            _description = string.Empty;
            _availabilityHours = new JsonSerializableDictionary<TimetableDay, TimetableSpanCollection>();
        }

        #endregion
    }
}
