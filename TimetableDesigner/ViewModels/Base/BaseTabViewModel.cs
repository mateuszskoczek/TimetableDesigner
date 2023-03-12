using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace TimetableDesigner.ViewModels.Base
{
    public abstract class BaseTabViewModel : BaseViewModel
    {
        #region FIELDS

        private ImageSource _tabImage;
        private string _tabTitle;
        private bool _isTabClosable;

        #endregion



        #region PROPERTIES

        public ImageSource TabImage
        {
            get => _tabImage;
            set
            {
                _tabImage = value;
                NotifyPropertyChanged(nameof(TabImage));
            }
        }
        public string TabTitle
        { 
            get => _tabTitle; 
            set
            {
                _tabTitle = value;
                NotifyPropertyChanged(nameof(TabTitle));
            }
        }
        public bool IsTabClosable
        {
            get => _isTabClosable;
            set
            {
                _isTabClosable = value;
                NotifyPropertyChanged(nameof(IsTabClosable));
            }
        }

        #endregion
    }
}
