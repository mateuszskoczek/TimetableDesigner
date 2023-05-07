using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public class ClassroomVM : ObservableObject, IModelVM, IUnitVM, IRemovableVM
    {
        #region FIELDS

        private Classroom _classroom;

        #endregion



        #region PROPERTIES

        IUnit IUnitVM.Unit => _classroom;
        public Classroom Classroom => _classroom;

        public string Name
        {
            get => _classroom.Name;
            set
            {
                if (_classroom.Name != value)
                {
                    _classroom.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Description
        {
            get => _classroom.Description;
            set
            {
                if (_classroom.Description != value)
                {
                    _classroom.Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }
        public bool IsCapacityLimited
        {
            get => _classroom.IsCapacityLimited;
            set
            {
                if (_classroom.IsCapacityLimited != value)
                {
                    _classroom.IsCapacityLimited = value;
                    NotifyPropertyChanged(nameof(IsCapacityLimited));
                }
            }
        }
        public uint Capacity
        {
            get => _classroom.Capacity;
            set
            {
                if (_classroom.Capacity != value)
                {
                    _classroom.Capacity = value;
                    NotifyPropertyChanged(nameof(Capacity));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ClassroomVM(Classroom classroom)
        {
            _classroom = classroom;
        }

        #endregion
    }
}
