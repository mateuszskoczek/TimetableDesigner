using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static TimetableDesigner.ViewModels.Views.TimetableEditorViewVM;

namespace TimetableDesigner.Controls
{
    public partial class DynamicGrid : UserControl
    {
        #region FIELDS

        private Border[,]? _contentCells;
        private Border[]? _columnHeadersCells;
        private Border[]? _rowHeadersCells;
        private List<DispatcherOperation> _dispatcherOperations;

        #endregion



        #region PROPERTIES

        private static DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", 
            typeof(int), 
            typeof(DynamicGrid), 
            new PropertyMetadata(0, new PropertyChangedCallback(OnLayoutChanged))
        );
        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        private static DependencyProperty RowsProperty = DependencyProperty.Register(
            "Rows", 
            typeof(int), 
            typeof(DynamicGrid), 
            new PropertyMetadata(0, new PropertyChangedCallback(OnLayoutChanged))
        );
        public int Rows
        {
            get => (int)GetValue(RowsProperty);
            set => SetValue(RowsProperty, value);
        }


        private static DependencyProperty ColumnHeadersSourceProperty = DependencyProperty.Register(
            "ColumnHeadersSource", 
            typeof(IEnumerable), 
            typeof(DynamicGrid),
            new PropertyMetadata(new PropertyChangedCallback(OnColumnHeadersChanged))
        );
        public IEnumerable ColumnHeadersSource
        {
            get => (IEnumerable)GetValue(ColumnHeadersSourceProperty);
            set => SetValue(ColumnHeadersSourceProperty, value);
        }


        private static DependencyProperty ColumnHeadersTemplateProperty = DependencyProperty.Register(
            "ColumnHeadersTemplate",
            typeof(DataTemplate),
            typeof(DynamicGrid),
            new PropertyMetadata(new PropertyChangedCallback(OnColumnHeadersChanged))
        );
        public DataTemplate ColumnHeadersTemplate
        {
            get => (DataTemplate)GetValue(ColumnHeadersTemplateProperty);
            set => SetValue(ColumnHeadersTemplateProperty, value);
        }


        private static DependencyProperty RowHeadersSourceProperty = DependencyProperty.Register(
            "RowHeadersSource", 
            typeof(IEnumerable), 
            typeof(DynamicGrid),
            new PropertyMetadata(new PropertyChangedCallback(OnRowHeadersChanged))
        );
        public IEnumerable RowHeadersSource
        {
            get => (IEnumerable)GetValue(RowHeadersSourceProperty);
            set => SetValue(RowHeadersSourceProperty, value);
        }

        private static DependencyProperty RowHeadersTemplateProperty = DependencyProperty.Register(
            "RowHeadersTemplate",
            typeof(DataTemplate),
            typeof(DynamicGrid),
            new PropertyMetadata(new PropertyChangedCallback(OnRowHeadersChanged))
        );
        public DataTemplate RowHeadersTemplate
        {
            get => (DataTemplate)GetValue(RowHeadersTemplateProperty);
            set => SetValue(RowHeadersTemplateProperty, value);
        }


        private static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", 
            typeof(IEnumerable), 
            typeof(DynamicGrid),
            new PropertyMetadata(new PropertyChangedCallback(OnItemsChanged))
        );
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", 
            typeof(DataTemplate), 
            typeof(DynamicGrid),
            new PropertyMetadata(new PropertyChangedCallback(OnItemsChanged))
        );
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        private static DependencyProperty CellBorderThicknessProperty = DependencyProperty.Register(
            "CellBorderThickness", 
            typeof(Thickness), 
            typeof(DynamicGrid), 
            new PropertyMetadata(new Thickness(0), new PropertyChangedCallback(OnCellBorderThicknessChanged))
        );
        public Thickness CellBorderThickness
        {
            get => (Thickness)GetValue(CellBorderThicknessProperty);
            set => SetValue(CellBorderThicknessProperty, value);
        }

        private static DependencyProperty CellBorderBrushProperty = DependencyProperty.Register(
            "CellBorderBrush", 
            typeof(Brush), 
            typeof(DynamicGrid), 
            new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(OnCellBorderBrushChanged))
        );
        public Brush CellBorderBrush
        {
            get => (Brush)GetValue(CellBorderBrushProperty);
            set => SetValue(CellBorderBrushProperty, value);
        }

        private static DependencyProperty HeaderCellBorderThicknessProperty = DependencyProperty.Register(
            "HeaderCellBorderThickness",
            typeof(Thickness?),
            typeof(DynamicGrid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnHeaderCellBorderThicknessChanged))
        );
        public Thickness HeaderCellBorderThickness
        {
            get
            {
                Thickness? headerThickness = (Thickness)GetValue(HeaderCellBorderThicknessProperty);
                if (headerThickness is null)
                {
                    return CellBorderThickness;
                }
                else
                {
                    return headerThickness.Value;
                }
            }
            set => SetValue(HeaderCellBorderThicknessProperty, value);
        }

        private static DependencyProperty HeaderCellBorderBrushProperty = DependencyProperty.Register(
            "HeaderCellBorderBrush",
            typeof(Brush),
            typeof(DynamicGrid),
            new PropertyMetadata(null, new PropertyChangedCallback(OnHeaderCellBorderBrushChanged))
        );
        public Brush? HeaderCellBorderBrush
        {
            get
            {
                Brush headerBrush = (Brush)GetValue(HeaderCellBorderBrushProperty);
                if (headerBrush is null)
                {
                    headerBrush = CellBorderBrush;
                }
                return headerBrush;
            }
            set => SetValue(HeaderCellBorderBrushProperty, value);
        }

        private static DependencyProperty StrechHeaderCellsToContentCellsSizeProperty = DependencyProperty.Register(
            "StrechHeaderCellsToContentCellsSize",
            typeof(bool),
            typeof(DynamicGrid),
            new PropertyMetadata(false, new PropertyChangedCallback(OnStrechHeaderCellsToContentCellsSizeChanged))
        );
        public bool StrechHeaderCellsToContentCellsSize
        {
            get => (bool)GetValue(StrechHeaderCellsToContentCellsSizeProperty);
            set => SetValue(StrechHeaderCellsToContentCellsSizeProperty, value);
        }

        #endregion



        #region EVENTS

        private static DependencyProperty ItemMovedCommandProperty = DependencyProperty.Register(
            "ItemMovedCommand",
            typeof(ICommand),
            typeof(DynamicGrid),
            new PropertyMetadata(null)
        );
        public ICommand ItemMovedCommand
        {
            get => (ICommand)GetValue(ItemMovedCommandProperty);
            set => SetValue(ItemMovedCommandProperty, value);
        }

        private static DependencyProperty ItemMovedCommandParameterProperty = DependencyProperty.Register(
            "ItemMovedCommandParameter",
            typeof(object),
            typeof(DynamicGrid),
            new PropertyMetadata(null)
        );
        public object ItemMovedCommandParameter
        {
            get => (object)GetValue(ItemMovedCommandParameterProperty);
            set => SetValue(ItemMovedCommandParameterProperty, value);
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when item moved to another cell")]
        public event EventHandler ItemMoved;

        #endregion



        #region CONSTRUCTORS

        public DynamicGrid()
        {
            _columnHeadersCells = null;
            _rowHeadersCells = null;
            _contentCells = null;
            _dispatcherOperations = new List<DispatcherOperation>();

            InitializeComponent();
        }

        #endregion



        #region PRIVATE METHODS

        private void LayoutRefresh()
        {
            BaseGrid.Children.Clear();
            BaseGrid.ColumnDefinitions.Clear();
            BaseGrid.RowDefinitions.Clear();

            if (Rows <= 0 || Columns <= 0)
            {
                _rowHeadersCells = null;
                _columnHeadersCells = null;
                _contentCells = null;
                return;
            }
            else
            {
                _rowHeadersCells = null;
                _columnHeadersCells = null;
                _contentCells = new Border[Rows, Columns];
            }

            bool columnHeaders = ColumnHeadersSource != null && ColumnHeadersSource.Cast<object>().Any();
            short columnHeadersNum = Convert.ToInt16(columnHeaders);

            bool rowHeaders = RowHeadersSource != null && RowHeadersSource.Cast<object>().Any();
            short rowHeadersNum = Convert.ToInt16(rowHeaders);

            if (columnHeaders)
            {
                RowDefinition columnHeaderRow = new RowDefinition();
                if (!StrechHeaderCellsToContentCellsSize)
                {
                    columnHeaderRow.Height = GridLength.Auto;
                }
                BaseGrid.RowDefinitions.Add(columnHeaderRow);
                _columnHeadersCells = new Border[Columns];

                for (int column = 0; column < Columns; column++)
                {
                    Border border = new Border()
                    {
                        BorderBrush = HeaderCellBorderBrush,
                        BorderThickness = HeaderCellBorderThickness,
                    };
                    Grid.SetColumn(border, column + rowHeadersNum);
                    Grid.SetRow(border, 0);
                    BaseGrid.Children.Add(border);
                    _columnHeadersCells[column] = border;
                }
                ColumnHeadersRefresh();
            }

            if (rowHeaders)
            {
                ColumnDefinition rowHeaderColumn = new ColumnDefinition();
                if (!StrechHeaderCellsToContentCellsSize)
                {
                    rowHeaderColumn.Width = GridLength.Auto;
                }
                BaseGrid.ColumnDefinitions.Add(rowHeaderColumn);
                _rowHeadersCells = new Border[Rows];

                for (int row = 0; row < Rows; row++)
                {
                    Border border = new Border()
                    {
                        BorderBrush = HeaderCellBorderBrush,
                        BorderThickness = HeaderCellBorderThickness,
                    };
                    Grid.SetColumn(border, 0);
                    Grid.SetRow(border, row + columnHeadersNum);
                    BaseGrid.Children.Add(border);
                    _rowHeadersCells[row] = border;
                }
                RowHeadersRefresh();
            }

            for (int _ = 0; _ < Rows; _++)
            {
                BaseGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto,
                });
            }
            for (int _ = 0; _ < Columns; _++)
            {
                BaseGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto,
                });
            }

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Border border = new Border()
                    {
                        BorderBrush = CellBorderBrush,
                        BorderThickness = CellBorderThickness,
                    };
                    Grid.SetColumn(border, column + rowHeadersNum);
                    Grid.SetRow(border, row + columnHeadersNum);

                    UniformGrid grid = new UniformGrid()
                    {
                        Rows = 1,
                        AllowDrop = true,
                        Background = Brushes.Transparent,
                    };
                    grid.Drop += OnItemMoved;

                    border.Child = grid;

                    BaseGrid.Children.Add(border);
                    _contentCells[row, column] = border;
                }
            }
            ItemsRefresh();
        }

        private void ColumnHeadersRefresh(bool switched = false) => HeadersRefresh(switched, ColumnHeadersSource, _columnHeadersCells, ColumnHeadersTemplate);
        private void RowHeadersRefresh(bool switched = false) => HeadersRefresh(switched, RowHeadersSource, _rowHeadersCells, RowHeadersTemplate);
        private void HeadersRefresh(bool switched, IEnumerable headerSource, Border[]? headerCells, DataTemplate headerTemplate)
        {
            if (switched)
            {
                LayoutRefresh();
            }

            if (headerSource is not null && headerCells is not null)
            {
                IEnumerable<object> headersSourceCollection = headerSource.Cast<object>();
                for (int index = 0; index < headerCells.Length; index++)
                {
                    headerCells[index].Child = null;
                    if (index < headersSourceCollection.Count())
                    {
                        object headerItem = headersSourceCollection.ElementAt(index);

                        FrameworkElement? control = null;
                        if (headerTemplate is not null)
                        {
                            control = headerTemplate.LoadContent() as FrameworkElement;
                            if (control is not null)
                            {
                                control.DataContext = headerItem;
                            }
                        }
                        control ??= new Label()
                        {
                            Content = headerItem.ToString()
                        };

                        headerCells[index].Child = control;
                    }
                }
            }
        }

        private void CellBorderRefresh(CellBorderRefreshProperty mode, CellBorderRefreshType type)
        {
            if ((type & CellBorderRefreshType.Headers) == CellBorderRefreshType.Headers)
            {
                List<IEnumerable<Border>> headerCellsCollection = new List<IEnumerable<Border>>();
                if (_columnHeadersCells is not null) headerCellsCollection.Add(_columnHeadersCells);
                if (_rowHeadersCells is not null) headerCellsCollection.Add(_rowHeadersCells);
                foreach (IEnumerable<Border> cells in headerCellsCollection)
                {
                    foreach (Border cell in cells)
                    {
                        if ((mode & CellBorderRefreshProperty.Thickness) == CellBorderRefreshProperty.Thickness) cell.BorderThickness = HeaderCellBorderThickness;
                        if ((mode & CellBorderRefreshProperty.Brush) == CellBorderRefreshProperty.Brush) cell.BorderBrush = HeaderCellBorderBrush;
                    }
                }
            }
            if ((type & CellBorderRefreshType.Content) == CellBorderRefreshType.Content && _contentCells is not null)
            {
                foreach (Border cell in _contentCells)
                {
                    if ((mode & CellBorderRefreshProperty.Thickness) == CellBorderRefreshProperty.Thickness) cell.BorderThickness = CellBorderThickness;
                    if ((mode & CellBorderRefreshProperty.Brush) == CellBorderRefreshProperty.Brush) cell.BorderBrush = CellBorderBrush;
                }
            }
        }

        private void ItemsRefresh()
        {
            if (ItemsSource is not null && _contentCells is not null)
            {
                foreach (DispatcherOperation operation in _dispatcherOperations)
                {
                    operation.Abort();
                }
                _dispatcherOperations.Clear();

                foreach (Border cell in _contentCells)
                {
                    ((UniformGrid)cell.Child).Children.Clear();
                }

                IEnumerable<object> itemsSourceCollection = ItemsSource.Cast<object>();
                foreach (object item in itemsSourceCollection)
                {
                    FrameworkElement? control = null;
                    if (ItemTemplate is not null)
                    {
                        control = ItemTemplate.LoadContent() as FrameworkElement;
                        if (control is not null)
                        {
                            control.DataContext = item;
                        }
                    }
                    control ??= new Label()
                    {
                        Content = item.ToString()
                    };

                    DispatcherOperation operation = Dispatcher.BeginInvoke(() =>
                    {
                        int row = GetRow(control);
                        int column = GetColumn(control);
                        Border cell = _contentCells[row, column];
                        UniformGrid cellGrid = (UniformGrid)cell.Child;
                        if (!cellGrid.Children.Contains(control))
                        {
                            cellGrid.Children.Add(control);
                        }
                    }, DispatcherPriority.Loaded);
                    _dispatcherOperations.Add(operation);
                }
            }
        }

        private void OnItemMoved(object sender, DragEventArgs e)
        {
            UniformGrid? target = sender as UniformGrid;
            object item = e.Data.GetData(DataFormats.Serializable);
            if (target is not null && item is UIElement element && _contentCells is not null)
            {
                int x = 0;
                int y = 0;
                for (x = 0; x < _contentCells.GetLength(0); x++)
                {
                    for (y = 0; y < _contentCells.GetLength(1); y++)
                    {
                        if (_contentCells[x, y].Child.Equals(target))
                        {
                            goto LoopEnd;
                        }
                    }
                }
                LoopEnd:
                if (x != GetRow(element) || y != GetColumn(element))
                {
                    SetRow(element, x);
                    SetColumn(element, y);
                    ItemMoved?.Invoke(this, EventArgs.Empty);
                    ItemMovedCommand?.Execute(ItemMovedCommandParameter);
                }
            }
        }

        private void HeaderRowColumnSizeRefresh()
        {
            GridLength size;
            if (StrechHeaderCellsToContentCellsSize)
            {
                size = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                size = GridLength.Auto;
            }

            if (BaseGrid.RowDefinitions.Count > 1 && ColumnHeadersSource is not null)
            {
                BaseGrid.RowDefinitions[0].Height = size;
            }
            if (BaseGrid.ColumnDefinitions.Count > 1 && RowHeadersSource is not null)
            {
                BaseGrid.ColumnDefinitions[0].Width = size;
            }
        }

        private static void OnLayoutChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).LayoutRefresh();
        private static void OnColumnHeadersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).ColumnHeadersRefresh((args.OldValue is null && args.NewValue is not null) || (args.OldValue is not null && args.NewValue is null));
        private static void OnRowHeadersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).RowHeadersRefresh((args.OldValue is null && args.NewValue is not null) || (args.OldValue is not null && args.NewValue is null));
        private static void OnItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).ItemsRefresh();
        private static void OnCellBorderThicknessChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).CellBorderRefresh(CellBorderRefreshProperty.Thickness, obj.GetValue(HeaderCellBorderThicknessProperty) is null ? CellBorderRefreshType.Headers | CellBorderRefreshType.Content : CellBorderRefreshType.Content);
        private static void OnCellBorderBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).CellBorderRefresh(CellBorderRefreshProperty.Brush, obj.GetValue(HeaderCellBorderBrushProperty) is null ? CellBorderRefreshType.Headers | CellBorderRefreshType.Content : CellBorderRefreshType.Content);
        private static void OnHeaderCellBorderThicknessChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).CellBorderRefresh(CellBorderRefreshProperty.Thickness, CellBorderRefreshType.Headers);
        private static void OnHeaderCellBorderBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).CellBorderRefresh(CellBorderRefreshProperty.Brush, CellBorderRefreshType.Headers);
        private static void OnStrechHeaderCellsToContentCellsSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((DynamicGrid)obj).HeaderRowColumnSizeRefresh();

        #endregion



        #region COLUMN/ROW SET/GET STATIC METHODS

        private static DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof(int), typeof(DynamicGrid), new PropertyMetadata(0));
        public static int GetColumn(UIElement element) => (int)element.GetValue(ColumnProperty);
        public static void SetColumn(UIElement element, int value) => element.SetValue(ColumnProperty, value);

        private static DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row", typeof(int), typeof(DynamicGrid), new PropertyMetadata(0));
        public static int GetRow(UIElement element) => (int)element.GetValue(RowProperty);
        public static void SetRow(UIElement element, int value) => element.SetValue(RowProperty, value);

        #endregion



        #region ENUMS

        [Flags]
        private enum CellBorderRefreshProperty
        {
            Brush = 1,
            Thickness = 2,
        }

        [Flags]
        private enum CellBorderRefreshType
        {
            Headers = 1,
            Content = 2,
        }

        #endregion
    }
}
