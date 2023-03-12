using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.ViewModels.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public class TeacherViewModel : BaseViewModel
    {
        #region FIELDS

        private Teacher _teacher;

        #endregion



        #region PROPERTIES

        public Teacher Teacher => _teacher;

        public string Name
        {
            get => _teacher.Name;
            set
            {
                if (_teacher.Name != value)
                {
                    _teacher.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Description
        {
            get => _teacher.Description;
            set
            {
                if (_teacher.Description != value)
                {
                    _teacher.Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public TeacherViewModel(Teacher teacher)
        {
            _teacher = teacher;
        }

        #endregion
    }
}
