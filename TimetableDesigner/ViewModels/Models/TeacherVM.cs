using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public class TeacherVM : ObservableObject, IModelVM, IUnitVM, IRemovableVM
    {
        #region FIELDS

        private IProjectService _projectService;
        private Teacher _teacher;

        #endregion



        #region PROPERTIES

        IUnit IUnitVM.Unit => _teacher;
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
        public ObservableDictionary<TimetableDay, ObservableCollection<TimetableSpan>> AvailabilityHours => new ObservableDictionary<TimetableDay, ObservableCollection<TimetableSpan>>(Teacher.AvailabilityHours.ToDictionary(i => i.Key, i => new ObservableCollection<TimetableSpan>(i.Value)));

        #endregion



        #region CONSTRUCTORS

        public TeacherVM(Teacher teacher)
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            _teacher = teacher;
        }

        #endregion



        #region PUBLIC METHODS

        public void AddDay(TimetableDay day)
        {
            if (day is not null)
            {
                Teacher.AvailabilityHours.Add(day, new TimetableSpanCollection());
                NotifyPropertyChanged(nameof(AvailabilityHours));

                //REFRESH: Errors
                _projectService.RefreshErrors();
            }
        }

        public void RemoveDay(TimetableDay day)
        {
            Teacher.AvailabilityHours.Remove(day);
            NotifyPropertyChanged(nameof(AvailabilityHours));

            //REFRESH: Errors
            _projectService.RefreshErrors();
        }

        public void AddHours(TimetableDay day, TimetableSpan hours) 
        {
            if (day is not null && hours is not null)
            {
                Teacher.AvailabilityHours[day].Add(hours);
                NotifyPropertyChanged(nameof(AvailabilityHours));

                //REFRESH: Errors
                _projectService.RefreshErrors();
            }
        }

        public void RemoveHours(TimetableDay day, TimetableSpan hours)
        {
            if (day is not null)
            {
                Teacher.AvailabilityHours[day].Remove(hours);
                NotifyPropertyChanged(nameof(AvailabilityHours));

                //REFRESH: Errors
                _projectService.RefreshErrors();
            }
        }

        #endregion
    }
}
