using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimetableDesigner.Core;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models;

namespace TimetableDesigner.Services.Scheduler
{
    public class SchedulerService : ISchedulerService
    {
        #region FIELDS

        private IProjectService _projectService;

        #endregion



        #region CONSTRUCTORS

        public SchedulerService(IProjectService projectService)
        {
            _projectService = projectService;
        }

        #endregion



        #region PUBLIC METHODS

        public (TimetableDay? Day, TimetableSpan? Slot) Schedule(ClassVM @class)
        {
            IEnumerable<(TimetableDay Day, TimetableSpan Slot)> freeSlots = GetFreeSlots(@class);

            List<(TimetableDay Day, TimetableSpan Slot)> allMidBreaks = new List<(TimetableDay Day, TimetableSpan Slot)>();
            List<(TimetableDay Day, TimetableSpan Slot)> secondTierBreaks = new List<(TimetableDay Day, TimetableSpan Slot)>();

            IEnumerable<TimetableDay> days = freeSlots.Select(x => x.Day).Distinct();
            foreach (TimetableDay day in days)
            {
                IEnumerable<int> unusedIndexes = freeSlots.Where(x => x.Day == day).Select(x => _projectService.ProjectViewModel.TimetableTemplate.Slots.IndexOf(x.Slot));
                Debug.WriteLine(day.Name);
                foreach (int unusedIndex in unusedIndexes)
                {
                    Debug.WriteLine(unusedIndex);
                }
                Debug.WriteLine("");

                IEnumerable<(int Start, int End)> breaks = FindBreaks(unusedIndexes);
                int last = _projectService.ProjectViewModel.TimetableTemplate.Slots.Count - 1;

                IEnumerable<(int Start, int End)> midBreaks = breaks.Where(x => x.Start != 0 && x.End != last);
                if (midBreaks.Any())
                {
                    allMidBreaks.Add((day, _projectService.ProjectViewModel.TimetableTemplate.Slots.ElementAt(midBreaks.First().Start)));
                }
                else
                {
                    (int start, int end) = breaks.First();
                    int selIndex = start == 0 && end != last ? end : start;
                    secondTierBreaks.Add((day, _projectService.ProjectViewModel.TimetableTemplate.Slots.ElementAt(selIndex)));
                }
            }

            if (allMidBreaks.Any())
            {
                return allMidBreaks.First();
            }

            if (secondTierBreaks.Any())
            {
                Random rd = new Random();
                return secondTierBreaks.ElementAt(rd.Next(secondTierBreaks.Count));
            }

            return (null, null);
        }

        #endregion



        #region PRIVATE METHODS

        private IEnumerable<(int Start, int End)> FindBreaks(IEnumerable<int> unusedIndexes)
        {
            unusedIndexes = unusedIndexes.Order();

            int prev = unusedIndexes.ElementAt(0);
            int first = unusedIndexes.ElementAt(0);
            List<(int Start, int End)> breaks = new List<(int Start, int End)>();
            foreach (int index in unusedIndexes.Skip(1))
            {
                if (prev + 1 != index)
                {
                    breaks.Add((first, prev));
                    first = index;
                }
                prev = index;
            }
            breaks.Add((first, prev));
            return breaks;
        }

        private Dictionary<TimetableDay, List<TimetableSpan>> CreateSlotsDictionary()
        {
            Dictionary<TimetableDay, List<TimetableSpan>> slotsDictionary = new Dictionary<TimetableDay, List<TimetableSpan>>();
            foreach (TimetableDay day in _projectService.ProjectViewModel.TimetableTemplate.Days)
            {
                slotsDictionary[day] = new List<TimetableSpan>();
                foreach (TimetableSpan slot in _projectService.ProjectViewModel.TimetableTemplate.Slots)
                {
                    slotsDictionary[day].Add(slot);
                }
            }
            return slotsDictionary;
        }

        private IEnumerable<(TimetableDay Day, TimetableSpan Slot)> GetFreeSlots(ClassVM @class)
        {
            Dictionary<TimetableDay, List<TimetableSpan>> classroomFreeSlotsDict = CreateSlotsDictionary();
            if (@class.Classroom is not null)
            {
                IEnumerable<ClassVM> classroomClasses = _projectService.ProjectViewModel.Classes.Where(x => x.Classroom == @class.Classroom).Where(x => x.Day is not null && x.Slot is not null);
                foreach (ClassVM classroomClass in classroomClasses)
                {
                    classroomFreeSlotsDict[classroomClass.Day].Remove(classroomClass.Slot);
                }
            }
            ICollection<(TimetableDay Day, TimetableSpan Slot)> classroomFreeSlots = new List<(TimetableDay Day, TimetableSpan Slot)>();
            foreach (KeyValuePair<TimetableDay, List<TimetableSpan>> pair in classroomFreeSlotsDict)
            {
                foreach (TimetableSpan slot in pair.Value)
                {
                    classroomFreeSlots.Add((pair.Key, slot));
                }
            }

            Dictionary<TimetableDay, List<TimetableSpan>> teacherFreeSlotsDict = CreateSlotsDictionary();
            if (@class.Teacher is not null)
            {
                IEnumerable<ClassVM> teacherClasses = _projectService.ProjectViewModel.Classes.Where(x => x.Teacher == @class.Teacher).Where(x => x.Day is not null && x.Slot is not null);
                foreach (ClassVM teacherClass in teacherClasses)
                {
                    teacherFreeSlotsDict[teacherClass.Day].Remove(teacherClass.Slot);
                }
            }
            ICollection<(TimetableDay Day, TimetableSpan Slot)> teacherFreeSlots = new List<(TimetableDay Day, TimetableSpan Slot)>();
            foreach (KeyValuePair<TimetableDay, List<TimetableSpan>> pair in teacherFreeSlotsDict)
            {
                foreach (TimetableSpan slot in pair.Value)
                {
                    teacherFreeSlots.Add((pair.Key, slot));
                }
            }

            Dictionary<TimetableDay, List<TimetableSpan>> groupFreeSlotsDict = CreateSlotsDictionary();
            if (@class.Group is not null)
            {
                IEnumerable<ClassVM> groupClasses;
                if (@class.Group is SubgroupVM subgroup)
                {
                    IEnumerable<GroupVM> groups = _projectService.ProjectViewModel.Groups.Where(x => x.AssignedSubgroups.Contains(subgroup));
                    groupClasses = _projectService.ProjectViewModel.Classes.Where(x => (x.Group is GroupVM group && groups.Contains(group)) || x.Group == subgroup);
                }
                else
                {
                    GroupVM group = @class.Group as GroupVM;
                    groupClasses = _projectService.ProjectViewModel.Classes.Where(x => x.Group == group || (x.Group is SubgroupVM subgroup && group.AssignedSubgroups.Contains(subgroup)));
                }
                groupClasses = groupClasses.Where(x => x.Day is not null && x.Slot is not null);
                foreach (ClassVM groupClass in groupClasses)
                {
                    groupFreeSlotsDict[groupClass.Day].Remove(groupClass.Slot);
                }
            }
            ICollection<(TimetableDay Day, TimetableSpan Slot)> groupFreeSlots = new List<(TimetableDay Day, TimetableSpan Slot)>();
            foreach (KeyValuePair<TimetableDay, List<TimetableSpan>> pair in groupFreeSlotsDict)
            {
                foreach (TimetableSpan slot in pair.Value)
                {
                    groupFreeSlots.Add((pair.Key, slot));
                }
            }

            return classroomFreeSlots.Intersect(teacherFreeSlots).Intersect(groupFreeSlots);
        }

        #endregion
    }
}
