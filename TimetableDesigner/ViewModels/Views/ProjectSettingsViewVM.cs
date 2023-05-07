using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TimetableDesigner.Commands;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Properties;
using TimetableDesigner.Services;
using TimetableDesigner.Services.MessageBox;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.ViewModels.Views
{
    public class ProjectSettingsViewVM : ObservableObject, IViewVM
    {
        #region FIELDS

        private IMessageBoxService _messageBoxService;
        private IProjectService _projectService;

        private string _newDayName;
        private DateTime? _newSlotFrom;
        private DateTime? _newSlotTo;

        #endregion



        #region PROPERTIES

        // Project
        public ProjectVM? Project => _projectService.ProjectViewModel;

        // Fields
        public string NewDayName
        {
            get => _newDayName;
            set
            {
                _newDayName = value;
                NotifyPropertyChanged(nameof(NewDayName));
            }
        }

        public DateTime? NewSlotFrom
        {
            get => _newSlotFrom;
            set
            {
                _newSlotFrom = value;
                NotifyPropertyChanged(nameof(NewSlotFrom));
            }
        }
        public DateTime? NewSlotTo
        {
            get => _newSlotTo;
            set
            {
                _newSlotTo = value;
                NotifyPropertyChanged(nameof(NewSlotTo));
            }
        }

        // Commands
        public ICommand AddDayCommand { get; set; }
        public ICommand AddSlotCommand { get; set; }
        public ICommand RemoveDayCommand { get; set; }
        public ICommand RemoveSlotCommand { get; set; }

        #endregion



        #region CONSTRUCTORS

        public ProjectSettingsViewVM()
        {
            _messageBoxService = ServiceProvider.Instance.GetService<IMessageBoxService>();
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();

            AddDayCommand = new RelayCommand<object>(param => AddDay());
            AddSlotCommand = new RelayCommand<object>(param => AddSlot());
            RemoveDayCommand = new RelayCommand<TimetableDay>(RemoveDay);
            RemoveSlotCommand = new RelayCommand<TimetableSpan>(RemoveSlot);

            _newDayName = string.Empty;
            _newSlotFrom = new DateTime(1, 1, 1, 8, 0, 0);
            _newSlotTo = new DateTime(1, 1, 1, 9, 0, 0);
        }

        #endregion



        #region PRIVATE METHODS

        private void AddDay()
        {
            if (Project is not null && !string.IsNullOrWhiteSpace(NewDayName))
            {
                Project.TimetableTemplate.AddDay(new TimetableDay(NewDayName));
                NewDayName = string.Empty;
            }
        }

        private void RemoveDay(TimetableDay day)
        {
            if (Project is not null)
            {
                foreach (TeacherVM teacher in Project.Teachers)
                {
                    teacher.RemoveDay(day);
                }
                Project.TimetableTemplate.RemoveDay(day);
            }
        }

        private void AddSlot()
        {
            if (Project is not null && NewSlotFrom.HasValue && NewSlotTo.HasValue)
            {
                TimeOnly from = new TimeOnly(NewSlotFrom.Value.Hour, NewSlotFrom.Value.Minute);
                TimeOnly to = new TimeOnly(NewSlotTo.Value.Hour, NewSlotTo.Value.Minute);
                if (from >= to)
                {
                    _messageBoxService.ShowError(Resources.ProjectSettings_Message_FromHigherThanTo);
                    return;
                }

                try
                {
                    TimetableSpan? lastSlot = Project.TimetableTemplate.Slots.LastOrDefault();

                    Project.TimetableTemplate.AddSlot(new TimetableSpan(from, to));

                    double offset = 0;
                    if (lastSlot != null)
                    {
                        offset = (from - lastSlot.To).TotalMinutes;
                    }

                    double delta = (to - from).TotalMinutes;
                    DateTime newFrom = NewSlotTo.Value.AddMinutes(offset);
                    DateTime newTo = NewSlotTo.Value.AddMinutes(offset + delta);
                    NewSlotFrom = newFrom;
                    NewSlotTo = newTo;
                }
                catch (ArgumentException)
                {
                    _messageBoxService.ShowError(Resources.ProjectSettings_Message_SlotCollision);
                }
            }
        }

        private void RemoveSlot(TimetableSpan slot) => Project.TimetableTemplate.RemoveSlot(slot);

        #endregion
    }
}
