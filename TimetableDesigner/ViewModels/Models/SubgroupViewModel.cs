using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;

namespace TimetableDesigner.ViewModels.Models
{
    public class SubgroupViewModel : BaseModelViewModel, IGroupViewModel
    {
        #region FIELDS

        private Subgroup _subgroup;

        #endregion



        #region PROPERTIES

        IGroup IGroupViewModel.Group => _subgroup;
        public Subgroup Subgroup => _subgroup;

        public string Name
        {
            get => _subgroup.Name;
            set
            {
                if (_subgroup.Name != value)
                {
                    _subgroup.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public SubgroupViewModel(Subgroup subgroup)
        {
            _subgroup = subgroup;
        }

        #endregion
    }
}
