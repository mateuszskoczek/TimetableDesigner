using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models.Base;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.Customs;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TimetableDesigner.Commands;
using System.Windows;
using System.Diagnostics;
using TimetableDesigner.Core;
using System.Xml.Linq;
using TimetableDesigner.Services.TabNavigation;
using System.Security.Policy;
using TimetableDesigner.Converters;
using System.Data;
using static TimetableDesigner.ViewModels.Views.TimetableEditorViewVM;
using TimetableDesigner.Services.Scheduler;

namespace TimetableDesigner.ViewModels.Views
{
    public class TimetableEditorViewVM : ObservableObject, IViewVM
    {
        #region FIELDS

        private IProjectService _projectService;
        private ITabNavigationService _tabNavigationService;
        private ISchedulerService _schedulerService;

        private IUnitVM? _unit;

        #endregion



        #region PROPERTIES

        public TimetableTemplateVM TimetableTemplate => _projectService.ProjectViewModel.TimetableTemplate;

        public IUnitVM? Unit
        {
            get => _unit;
            set
            {
                if (value != _unit)
                {
                    _unit = value;
                    NotifyPropertyChanged(nameof(Unit));
                }
            }
        }

        public IEnumerable<ClassVM> AssignedClasses => _projectService.ProjectViewModel.Classes.Where(c => c.Classroom == Unit || c.Teacher == Unit || c.Group == Unit || (Unit is GroupVM g && c.Group is SubgroupVM s && g.AssignedSubgroups.Contains(s)));
        public IEnumerable<ClassVM> ScheduledClasses => AssignedClasses.Where(c => c.Slot is not null && c.Day is not null);
        public IEnumerable<ClassVM> UnscheduledClasses => AssignedClasses.Where(c => c.Slot is null || c.Day is null);

        public ICommand AddClassCommand { get; set; }
        public ICommand AutoScheduleCommand { get; set; }
        public ICommand RemoveClassCommand { get; set; }
        public ICommand CloneClassCommand { get; set; }
        public ICommand RefreshScheduledClassesCommand { get; set; }
        public ICommand RefreshUnscheduledClassesCommand { get; set; }
        public ICommand RefreshAllClassesCommand { get; set; }

        #endregion



        #region CONSTRUCTORS

        public TimetableEditorViewVM() : this(null)
        { }

        public TimetableEditorViewVM(IUnitVM? unit)
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            _tabNavigationService = ServiceProvider.Instance.GetService<ITabNavigationService>();
            _schedulerService = ServiceProvider.Instance.GetService<ISchedulerService>();

            _unit = unit;

            AddClassCommand = new RelayCommand<object>(arg => AddClass());
            AutoScheduleCommand = new RelayCommand<object>(arg => AutoSchedule());
            RemoveClassCommand = new RelayCommand<ClassVM>(RemoveClass);
            CloneClassCommand = new RelayCommand<ClassVM>(CloneClass);
            RefreshScheduledClassesCommand = new RelayCommand<object>(arg => RefreshClassesAndErrors(RefreshMode.Scheduled));
            RefreshUnscheduledClassesCommand = new RelayCommand<object>(arg => RefreshClassesAndErrors(RefreshMode.Unscheduled));
            RefreshAllClassesCommand = new RelayCommand<object>(arg => RefreshClassesAndErrors(RefreshMode.Scheduled | RefreshMode.Unscheduled));
        }

        #endregion



        #region PUBLIC METHODS

        public void RefreshClasses(RefreshMode mode)
        {
            if ((mode & RefreshMode.Scheduled) == RefreshMode.Scheduled)
                NotifyPropertyChanged(nameof(ScheduledClasses));
            if ((mode & RefreshMode.Unscheduled) == RefreshMode.Unscheduled)
                NotifyPropertyChanged(nameof(UnscheduledClasses));
        }

        #endregion



        #region PRIVATE METHODS

        private void AddClass()
        {
            Class @class = new Class()
            {
                Name = "New class",
                Teacher = _unit is TeacherVM t ? t.Teacher : null,
                Classroom = _unit is ClassroomVM c ? c.Classroom : null,
                Group = _unit is BaseGroupVM g ? g.BaseGroup : null,
            };
            ClassVM classVM = new ClassVM(@class);
            _projectService.ProjectViewModel.Classes.Add(classVM);

            //REFRESH: Unassigned classes of this unit & Errors
            RefreshClasses(RefreshMode.Unscheduled);
            _projectService.RefreshErrors();
        }

        private void AutoSchedule()
        {
            foreach (ClassVM @class in UnscheduledClasses)
            {
                (TimetableDay? day, TimetableSpan? slot) = _schedulerService.Schedule(@class);
                @class.Day = day;
                @class.Slot = slot;
            }

            //REFRESH: Scheduled & Errors
            foreach (TimetableEditorViewVM editors in _tabNavigationService.Tabs.Select(x => x.ViewModel).OfType<TimetableEditorViewVM>().Distinct())
            {
                editors.RefreshClasses(RefreshMode.Scheduled | RefreshMode.Unscheduled);
            }
            _projectService.RefreshErrors();
        }

        private void RemoveClass(ClassVM @class)
        {
            Debug.WriteLine(@class.Name);
            List<IUnitVM> units = new List<IUnitVM>();
            if (@class.Teacher is not null)
            {
                units.Add(@class.Teacher);
            }
            if (@class.Classroom is not null)
            {
                units.Add(@class.Classroom);
            }
            if (@class.Group is not null)
            {
                units.Add(@class.Group);
                if (@class.Group is SubgroupVM sg)
                {
                    units.AddRange(_projectService.ProjectViewModel.Groups.Where(x => x.AssignedSubgroups.Contains(sg)));
                }
            }

            RefreshMode refreshMode = RefreshMode.Unscheduled;
            if (@class.Day is not null && @class.Slot is not null)
            {
                refreshMode = RefreshMode.Scheduled;
            }

            _projectService.ProjectViewModel.Classes.Remove(@class);

            //REFRESH: Assigned classes of units assigned to class & Errors
            foreach (TimetableEditorViewVM editors in _tabNavigationService.Tabs.Select(x => x.ViewModel).OfType<TimetableEditorViewVM>().Where(x => units.Contains(x.Unit)).Distinct())
            {
                editors.RefreshClasses(refreshMode);
            }
            _projectService.RefreshErrors();
        }

        private void CloneClass(ClassVM @class)
        {
            List<IUnitVM> units = new List<IUnitVM>();
            if (@class.Teacher is not null)
            {
                units.Add(@class.Teacher);
            }
            if (@class.Classroom is not null)
            {
                units.Add(@class.Classroom);
            }
            if (@class.Group is not null)
            {
                units.Add(@class.Group);
                if (@class.Group is SubgroupVM sg)
                {
                    units.AddRange(_projectService.ProjectViewModel.Groups.Where(x => x.AssignedSubgroups.Contains(sg)));
                }
            }

            RefreshMode refreshMode = RefreshMode.Unscheduled;
            if (@class.Day is not null && @class.Slot is not null)
            {
                refreshMode = RefreshMode.Scheduled;
            }

            Class newClass = new Class()
            {
                Name = @class.Name,
                Teacher = @class.Teacher?.Teacher,
                Classroom = @class.Classroom?.Classroom,
                Group = @class.Group?.BaseGroup,
                Day = @class.Day,
                Slot = @class.Slot,
                Color = @class.Color,
            };
            ClassVM classVM = new ClassVM(newClass);
            _projectService.ProjectViewModel.Classes.Add(classVM);

            //REFRESH: Assigned classes of units assigned to class & Errors
            foreach (TimetableEditorViewVM editors in _tabNavigationService.Tabs.Select(x => x.ViewModel).OfType<TimetableEditorViewVM>().Where(x => units.Contains(x.Unit)).Distinct())
            {
                editors.RefreshClasses(refreshMode);
            }
            _projectService.RefreshErrors();
        }

        private void RefreshClassesAndErrors(RefreshMode mode)
        {
            RefreshClasses(mode);
            _projectService.RefreshErrors();
        }

        #endregion



        #region ENUMS

        [Flags]
        public enum RefreshMode
        {
            Scheduled,
            Unscheduled
        }

        #endregion
    }
}
