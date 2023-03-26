using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Services.TabNavigation
{
    public interface ITabNavigationService : IService, INotifyPropertyChanged
    {
        #region PROPERTIES

        ObservableCollection<TabItem> Tabs { get; }
        TabItem SelectedTab { get; }

        #endregion



        #region PUBLIC METHODS

        void AddAndActivate(TabItem item);

        void Add(TabItem item);

        void InsertAndActivate(int index, TabItem item);

        void Insert(int index, TabItem item);

        void Close(TabItem item);
        void Close(IEnumerable<TabItem> item);

        void CloseAt(int index);
        void CloseAt(IEnumerable<int> indexes);

        void CloseAll();

        void Activate(TabItem item);

        #endregion
    }
}
