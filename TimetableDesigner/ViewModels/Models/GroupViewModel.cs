using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;

namespace TimetableDesigner.ViewModels.Models
{
    public class GroupViewModel : BaseModelViewModel, IGroupViewModel
    {
        #region FIELDS

        private IProjectService _projectService;

        private Group _group;

        #endregion



        #region PROPERTIES

        IGroup IGroupViewModel.Group => _group;
        public Group Group => _group;

        public string Name
        {
            get => _group.Name;
            set
            {
                if (_group.Name != value)
                {
                    _group.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Description
        {
            get => _group.Description;
            set
            {
                if (_group.Description != value)
                {
                    _group.Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }
        public ObservableCollection<SubgroupViewModel> AssignedSubgroups => new ObservableCollection<SubgroupViewModel>(_projectService.ProjectViewModel.Subgroups.Where(vm => Group.AssignedSubgroups.Contains(vm.Subgroup)));

        #endregion



        #region CONSTRUCTORS

        public GroupViewModel(Group group)
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            _group = group;
        }

        #endregion



        #region PUBLIC METHODS

        public void AddSubgroup(SubgroupViewModel subgroup)
        {
            Group.AssignedSubgroups.Add(subgroup.Subgroup);
            NotifyPropertyChanged(nameof(AssignedSubgroups));
        }

        public void RemoveSubgroup(SubgroupViewModel subgroup)
        {
            Group.AssignedSubgroups.Remove(subgroup.Subgroup);
            NotifyPropertyChanged(nameof(AssignedSubgroups));
        }

        #endregion
    }
}
