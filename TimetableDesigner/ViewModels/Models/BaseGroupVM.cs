using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Core;
using TimetableDesigner.Customs;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.ViewModels.Models
{
    public abstract class BaseGroupVM : ObservableObject, IModelVM, IUnitVM
    {
        #region FIELDS

        protected BaseGroup _baseGroup;

        #endregion



        #region PROPERTIES

        IUnit IUnitVM.Unit => _baseGroup;
        public BaseGroup BaseGroup => _baseGroup;

        public string Name
        {
            get => _baseGroup.Name;
            set
            {
                if (_baseGroup.Name != value)
                {
                    _baseGroup.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string ShortName
        {
            get => _baseGroup.ShortName;
            set
            {
                if (_baseGroup.ShortName != value)
                {
                    _baseGroup.ShortName = value;
                    NotifyPropertyChanged(nameof(ShortName));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public BaseGroupVM(BaseGroup baseGroup)
        {
            _baseGroup = baseGroup;
        }

        #endregion
    }
}
