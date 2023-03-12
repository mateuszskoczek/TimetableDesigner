﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimetableDesigner.Commands;
using TimetableDesigner.Core;
using TimetableDesigner.MessageBox;
using TimetableDesigner.Properties;
using TimetableDesigner.ViewModels.Base;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.ViewModels
{
    public class TeacherEditTabViewModel : BaseTabViewModel
    {
        #region FIELDS

        private TeacherViewModel _teacher;
        private TimetableTemplateViewModel _timetableTemplate;

        private TimetableDay _selectedDay;

        private TimetableDay _selectedNewDay;
        private DateTime? _newHourFrom;
        private DateTime? _newHourTo;

        #endregion



        #region PROPERTIES

        public TeacherViewModel Teacher
        {
            get => _teacher;
            set
            {
                if (value != _teacher)
                {
                    _teacher = value;
                    NotifyPropertyChanged(nameof(Teacher));
                }
            }
        }
        public TimetableTemplateViewModel TimetableTemplate
        {
            get => _timetableTemplate;
            set
            {
                if (value != _timetableTemplate)
                {
                    _timetableTemplate = value;
                    NotifyPropertyChanged(nameof(TimetableTemplate));
                }
            }
        }

        public ICommand AddDayCommand { get; set; }
        public ICommand RemoveDayCommand { get; set; }
        public ICommand AddHourCommand { get; set; }
        public ICommand RemoveHourCommand { get; set; }

        public TimetableDay SelectedDay
        {
            get => _selectedDay;
            set
            {
                if (_selectedDay != value)
                {
                    _selectedDay = value;
                    NotifyPropertyChanged(nameof(SelectedDay));
                    NotifyPropertyChanged(nameof(SelectedDayHours));
                }
            }
        }

        public TimetableDay SelectedNewDay
        {
            get => _selectedNewDay;
            set
            {
                if (_selectedNewDay != value)
                {
                    _selectedNewDay = value;
                    NotifyPropertyChanged(nameof(SelectedNewDay));
                }
            }
        }

        public DateTime? NewHourFrom
        {
            get => _newHourFrom;
            set
            {
                _newHourFrom = value;
                NotifyPropertyChanged(nameof(NewHourFrom));
            }
        }
        public DateTime? NewHourTo
        {
            get => _newHourTo;
            set
            {
                _newHourTo = value;
                NotifyPropertyChanged(nameof(NewHourTo));
            }
        }

        public ObservableCollection<TimetableSpan> SelectedDayHours => SelectedDay is not null ? Teacher.AvailabilityHours[SelectedDay] : new ObservableCollection<TimetableSpan>();

        #endregion



        #region CONSTRUCTORS

        public TeacherEditTabViewModel() : this(new TeacherViewModel(new Core.Teacher()), new TimetableTemplateViewModel(new Core.TimetableTemplate()))
        { }

        public TeacherEditTabViewModel(TeacherViewModel teacher, TimetableTemplateViewModel timetableTemplate) : base()
        {
            _teacher = teacher;
            _timetableTemplate = timetableTemplate;

            AddDayCommand = new RelayCommand<object>(param => AddDay());
            RemoveDayCommand = new RelayCommand<TimetableDay>(RemoveDay);
            AddHourCommand = new RelayCommand<object>(param => AddHour());
            RemoveHourCommand = new RelayCommand<TimetableSpan>(RemoveHour);

            _newHourFrom = new DateTime(1, 1, 1, 8, 0, 0);
            _newHourTo = new DateTime(1, 1, 1, 16, 0, 0);
        }

        #endregion



        #region PRIVATE METHODS

        private void AddDay()
        {
            if (!Teacher.AvailabilityHours.ContainsKey(SelectedNewDay))
            {
                Teacher.AddDay(SelectedNewDay);
            }
        }

        private void RemoveDay(TimetableDay day)
        {
            Teacher.RemoveDay(day);
        }

        private void AddHour()
        {
            if (NewHourFrom.HasValue && NewHourTo.HasValue)
            {
                TimeOnly from = new TimeOnly(NewHourFrom.Value.Hour, NewHourFrom.Value.Minute);
                TimeOnly to = new TimeOnly(NewHourTo.Value.Hour, NewHourTo.Value.Minute);
                if (from >= to)
                {
                    MessageBoxService.ShowError(Resources.TeacherEdit_Message_FromHigherThanTo);
                    return;
                }

                try
                {
                    Teacher.AddHours(SelectedDay, new TimetableSpan(from, to));
                }
                catch (ArgumentException)
                {
                    MessageBoxService.ShowError(Resources.TeacherEdit_Message_HourCollision);
                }
            }
            NotifyPropertyChanged(nameof(SelectedDayHours));
        }

        private void RemoveHour(TimetableSpan hour)
        {
            Teacher.RemoveHours(SelectedDay, hour);
            NotifyPropertyChanged(nameof(SelectedDayHours));
        }

        #endregion
    }
}
