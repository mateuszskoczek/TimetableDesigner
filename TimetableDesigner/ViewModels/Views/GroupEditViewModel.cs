using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Services.TabNavigation;
using TimetableDesigner.Services;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.Properties;
using TimetableDesigner.Customs;
using TimetableDesigner.Services.Project;
using System.Windows.Input;
using TimetableDesigner.Commands;
using System.Diagnostics;
using TimetableDesigner.Services.MessageBox;

namespace TimetableDesigner.ViewModels.Views
{
    public class GroupEditViewModel : BaseViewViewModel
    {
        #region FIELDS

        private IProjectService _projectService;
        private IMessageBoxService _messageBoxService;

        private GroupViewModel _group;

        private string _newSubgroupName;

        #endregion



        #region PROPERTIES

        public GroupViewModel Group
        {
            get => _group;
            set
            {
                if (_group != value)
                {
                    _group = value;
                    NotifyPropertyChanged(nameof(Group));
                }
            }
        }

        public string Name
        {
            get => _group.Name;
            set
            {
                if (_group.Name != value)
                {
                    _group.Name = value;
                    NotifyPropertyChanged(nameof(Name));

                    TabItem? tab = ServiceProvider.Instance.GetService<ITabNavigationService>().Tabs.Where(tab => tab.ViewModel == this).FirstOrDefault();
                    if (tab != null)
                    {
                        tab.Title = $"{Resources.Tabs_GroupEdit}: {_group.Name}";
                    }
                }
            }
        }

        public ObservableDictionary<SubgroupViewModel, bool> Subgroups => new ObservableDictionary<SubgroupViewModel, bool>(_projectService.ProjectViewModel.Subgroups.ToDictionary(sg => sg, Group.AssignedSubgroups.Contains));

        public string NewSubgroupName
        {
            get => _newSubgroupName;
            set
            {
                if (_newSubgroupName != value)
                {
                    _newSubgroupName = value;
                    NotifyPropertyChanged(nameof(NewSubgroupName));
                }
            }
        }

        public ICommand AddSubgroupCommand { get; set; }
        public ICommand EditSubgroupAssignmentCommand { get; set; }
        public ICommand DeleteSubgroupCommand { get; set; }

        #endregion



        #region CONSTRUCTORS

        public GroupEditViewModel() : this(new GroupViewModel(new Group()))
        { }

        public GroupEditViewModel(GroupViewModel group)
        {
            _projectService = ServiceProvider.Instance.GetService<IProjectService>();
            _messageBoxService = ServiceProvider.Instance.GetService<IMessageBoxService>();

            _group = group;
            _newSubgroupName = string.Empty;

            AddSubgroupCommand = new RelayCommand<object>(args => AddSubgroup());
            EditSubgroupAssignmentCommand = new RelayCommand<SubgroupViewModel>(EditSubgroupAssignment);
            DeleteSubgroupCommand = new RelayCommand<SubgroupViewModel>(DeleteSubgroup);
        }

        #endregion



        #region PRIVATE METHODS

        private void AddSubgroup()
        {
            Subgroup subgroup = new Subgroup()
            {
                Name = NewSubgroupName
            };
            SubgroupViewModel subgroupViewModel = new SubgroupViewModel(subgroup);

            _projectService.ProjectViewModel.Subgroups.Add(subgroupViewModel);
            Group.AddSubgroup(subgroupViewModel);
            NotifyPropertyChanged(nameof(Subgroups));

            NewSubgroupName = string.Empty;
        }

        private void EditSubgroupAssignment(SubgroupViewModel subgroup)
        {
            bool assigned = Subgroups[subgroup];
            if (assigned)
            {
                Group.RemoveSubgroup(subgroup);
            }
            else
            {
                Group.AddSubgroup(subgroup);
            }
            NotifyPropertyChanged(nameof(Subgroups));
        }

        private void DeleteSubgroup(SubgroupViewModel subgroup)
        {
            MessageBoxQuestionResult result = _messageBoxService.ShowQuestion(Resources.GroupEdit_Message_SubgroupDelete, true);
            if (result == MessageBoxQuestionResult.Yes)
            {
                foreach (GroupViewModel group in _projectService.ProjectViewModel.Groups)
                {
                    group.RemoveSubgroup(subgroup);
                }
                _projectService.ProjectViewModel.Subgroups.Remove(subgroup);
                NotifyPropertyChanged(nameof(Subgroups));
            }
        }

        #endregion
    }
}
