using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using TimetableDesigner.Commands;
using TimetableDesigner.Customs;
using TimetableDesigner.ViewModels;

namespace TimetableDesigner.Services.TabNavigation
{
    public class TabItem : ObservableObject
    {
        #region FIELDS

        private ITabNavigationService _tabNavigationService;

        private ImageSource _image;
        private string _title;
        private bool _isClosable;
        private IViewVM _viewModel;

        #endregion



        #region PROPERTIES

        public ImageSource Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    NotifyPropertyChanged(nameof(Image));
                }
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }
        public bool IsClosable
        {
            get => _isClosable;
            set
            {
                if (_isClosable != value)
                {
                    _isClosable = value;
                    NotifyPropertyChanged(nameof(IsClosable));
                }
            }
        }
        public IViewVM ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    NotifyPropertyChanged(nameof(ViewModel));
                }
            }
        }

        public ICommand CloseCommand { get; set; }
        public ICommand ActivateCommand { get; set; }

        #endregion



        #region CONSTRUCTORS

        public TabItem()
        {
            _tabNavigationService = ServiceProvider.Instance.GetService<ITabNavigationService>();

            CloseCommand = new RelayCommand<object>(args => Close());
            ActivateCommand = new RelayCommand<object>(args => Activate());
        }

        #endregion



        #region PUBLIC METHODS

        public void Close() => _tabNavigationService.Close(this);

        public void Activate() => _tabNavigationService.Activate(this);

        #endregion
    }
}
