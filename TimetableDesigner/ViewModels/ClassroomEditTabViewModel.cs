using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.ViewModels.Base;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.ViewModels
{
    public class ClassroomEditTabViewModel : BaseTabViewModel
    {
        #region FIELDS

        private ClassroomViewModel _classroom;

        #endregion



        #region PROPERTIES

        public ClassroomViewModel Classroom
        {
            get => _classroom;
            set
            {
                if (_classroom != value)
                {
                    _classroom = value;
                    NotifyPropertyChanged(nameof(Classroom));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ClassroomEditTabViewModel() : this(new ClassroomViewModel(new Core.Classroom()))
        { }

        public ClassroomEditTabViewModel(ClassroomViewModel classroom) : base()
        {
            _classroom = classroom;
        }

        #endregion
    }
}
