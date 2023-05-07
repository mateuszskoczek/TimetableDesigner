using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableDesigner.Customs;
using TimetableDesigner.ViewModels.Models;
using TimetableDesigner.ViewModels.Models.Base;

namespace TimetableDesigner.Services.Project
{
    public class ProjectError : ObservableObject
    {
        #region FIELDS

        private ProjectErrorType _type;
        private IUnitVM _unit;
        private ClassVM _class;
        private string _description;

        #endregion



        #region PROPERTIES

        public ProjectErrorType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    NotifyPropertyChanged(nameof(Type));
                }
            }
        }
        public IUnitVM Unit
        {
            get => _unit;
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    NotifyPropertyChanged(nameof(Unit));
                }
            }
        }
        public ClassVM Class
        {
            get => _class;
            set
            {
                if (value != _class)
                {
                    _class = value;
                    NotifyPropertyChanged(nameof(Class));
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description) 
                { 
                    _description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ProjectError(ProjectErrorType type, IUnitVM unit, ClassVM @class, string description)
        {
            Type = type;
            Unit = unit;
            Class = @class;
            Description = description;
        }

        #endregion
    }
}
