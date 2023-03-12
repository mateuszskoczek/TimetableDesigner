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
using TimetableDesigner.MessageBox;
using TimetableDesigner.Properties;
using TimetableDesigner.ViewModels.Base;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.ViewModels
{
    public class ProjectSettingsTabViewModel : BaseTabViewModel
    {
        #region FIELDS

        private string _newDayName;
        private DateTime? _newSlotFrom;
        private DateTime? _newSlotTo;

        #endregion



        #region PROPERTIES

        public ICommand AddDayCommand { get; set; }
        public ICommand AddSlotCommand { get; set; }
        public ICommand RemoveDayCommand { get; set; }
        public ICommand RemoveSlotCommand { get; set; }

        public ProjectViewModel? Project { get; set; }

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

        #endregion



        #region CONSTRUCTORS

        public ProjectSettingsTabViewModel() : base()
        {
            Project = new ProjectViewModel(new Project());

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
            if (!string.IsNullOrWhiteSpace(NewDayName))
            {
                Project.TimetableTemplate.AddDay(new TimetableDay(NewDayName));
                NewDayName = string.Empty;
            }
        }

        private void RemoveDay(TimetableDay day) => Project.TimetableTemplate.RemoveDay(day);

        private void AddSlot()
        {
            if (NewSlotFrom.HasValue && NewSlotTo.HasValue)
            {
                TimeOnly from = new TimeOnly(NewSlotFrom.Value.Hour, NewSlotFrom.Value.Minute);
                TimeOnly to = new TimeOnly(NewSlotTo.Value.Hour, NewSlotTo.Value.Minute);
                if (from >= to)
                {
                    MessageBoxService.ShowError(Resources.ProjectSettings_Message_FromHigherThanTo);
                    return;
                }

                try
                {
                    Project.TimetableTemplate.AddSlot(new TimetableSpan(from, to));

                    double delta = (to - from).TotalMinutes;
                    DateTime newFrom = NewSlotTo.Value;
                    DateTime newTo = NewSlotTo.Value.AddMinutes(delta);
                    NewSlotFrom = newFrom;
                    NewSlotTo = newTo;
                }
                catch (ArgumentException)
                {
                    MessageBoxService.ShowError(Resources.ProjectSettings_Message_SlotCollision);
                }
            }
        }

        private void RemoveSlot(TimetableSpan slot) => Project.TimetableTemplate.RemoveSlot(slot);

        #endregion
    }
}
