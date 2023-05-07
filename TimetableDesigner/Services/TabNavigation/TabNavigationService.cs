using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TimetableDesigner.Customs;

namespace TimetableDesigner.Services.TabNavigation
{
    public class TabNavigationService : ObservableObject, ITabNavigationService
    {
        #region FIELDS

        private TabItem _selectedTab;

        #endregion



        #region PROPERTIES

        public ObservableCollection<TabItem> Tabs { get; private set; }
        public TabItem? SelectedTab 
        { 
            get => _selectedTab;
            private set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    NotifyPropertyChanged(nameof(SelectedTab));
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public TabNavigationService()
        {
            Tabs = new ObservableCollection<TabItem>();
            SelectedTab = null;
        }

        #endregion



        #region PUBLIC METHODS

        public void AddAndActivate(TabItem item)
        {
            Add(item);
            Activate(item);
        }

        public void Add(TabItem item) => Tabs.Add(item);

        public void InsertAndActivate(int index, TabItem item)
        {
            Insert(index, item);
            Activate(item);
        }

        public void Insert(int index, TabItem item) => Tabs.Insert(index, item);

        public void Close(TabItem item) => Close(new List<TabItem>() { item });
        public void Close(IEnumerable<TabItem> items)
        {
            TabItem? selected = SelectedTab;
            while (items.Contains(selected) && selected != null)
            {
                int nextIndex = Tabs.IndexOf(selected) + 1;
                if (Tabs.Count > nextIndex)
                {
                    selected = Tabs[nextIndex];
                }
                else
                {
                    selected = null;
                }
            }
            if (selected == null)
            {
                selected = SelectedTab;
                while (items.Contains(selected) && selected != null)
                {
                    int prevIndex = Tabs.IndexOf(selected) - 1;
                    if (prevIndex >= 0)
                    {
                        selected = Tabs[prevIndex];
                    }
                    else
                    {
                        selected = null;
                    }
                }
            }
            foreach (TabItem item in items.ToList())
            {
                Tabs.Remove(item);
            }
            SelectedTab = selected;
        }

        public void CloseAt(int index) => CloseAt(new List<int>() { index });
        public void CloseAt(IEnumerable<int> indexes)
        {
            List<TabItem> items = new List<TabItem>();
            foreach (int index in indexes)
            {
                if (Tabs.Count > index)
                {
                    items.Add(Tabs[index]);
                }
            }
            Close(items);
        }

        public void CloseAll()
        {
            Tabs.Clear();
            SelectedTab = null;
        }

        public void Activate(TabItem item)
        {
            if (Tabs.Contains(item))
            {
                SelectedTab = item;
            }
        }

        #endregion
    }
}
