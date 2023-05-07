using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.Services;
using TimetableDesigner.Services.Project;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public class GroupVM : BaseGroupVM, IRemovableVM
    {
        #region FIELDS

        private IProjectService _projectService;

        #endregion



        #region PROPERTIES

        public Group Group => (Group)_baseGroup;

        public string Description
        {
            get => Group.Description;
            set
            {
                if (Group.Description != value)
                {
                    Group.Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }
        public ObservableCollection<SubgroupVM> AssignedSubgroups => new ObservableCollection<SubgroupVM>(_projectService.ProjectViewModel.Subgroups.Where(vm => Group.AssignedSubgroups.Contains(vm.Subgroup)));

        #endregion



        #region CONSTRUCTORS

        public GroupVM(Group group) : base(group)
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
        }

        #endregion



        #region PUBLIC METHODS

        public void AddSubgroup(SubgroupVM subgroup)
        {
            Group.AssignedSubgroups.Add(subgroup.Subgroup);
            NotifyPropertyChanged(nameof(AssignedSubgroups));

            //REFRESH: Errors
            _projectService.RefreshErrors();
        }

        public void RemoveSubgroup(SubgroupVM subgroup)
        {
            Group.AssignedSubgroups.Remove(subgroup.Subgroup);
            NotifyPropertyChanged(nameof(AssignedSubgroups));

            //REFRESH: Errors
            _projectService.RefreshErrors();
        }

        #endregion
    }
}
