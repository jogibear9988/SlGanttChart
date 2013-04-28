/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CoderForRent.Silverlight.Charting.Core;

namespace CoderForRent.Silverlight.Charting.Gantt
{
    /// <summary>
    /// The Gantt Chart will display a datagrid with a graphically represented schedule cross-referenced by
    /// a timeline.
    /// </summary>
    [
    TemplatePart(Name = "TimespanElement", Type = typeof(TimespanHeader.TimespanHeader)),
    TemplatePart(Name = "VerticalScrollbar", Type = typeof(ScrollBar)),
    TemplatePart(Name = "HorizontalScrollbar", Type = typeof(ScrollBar)),
    TemplatePart(Name = "GridSplitterElement", Type = typeof(GridSplitter)),
    TemplatePart(Name = "TaskGrid", Type = typeof(GanttDataGrid)),
    TemplatePart(Name = "PanelElement", Type = typeof(GanttPanel))
    ]
    public class GanttChart : Chart
    {
        public event GanttPanelColumnHandler GeneratingGanttPanelColumn;
        protected internal void RaiseGeneratingGanttPanelColumn(GanttPanelColumnEventArgs e)
        {
            if (GeneratingGanttPanelColumn != null)
                GeneratingGanttPanelColumn(this, e);
        }

        #region Dependency Properties
        public static DependencyProperty GanttRowBorderBrushProperty = DependencyProperty.Register("GanttRowBorderBrush", typeof(Brush), typeof(GanttChart), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));
        public static DependencyProperty HorizontalScrollbarVisibilityProperty = DependencyProperty.Register("HorizontalScrollbarVisibility", typeof(Visibility), typeof(GanttChart), new PropertyMetadata(Visibility.Visible));
        public static DependencyProperty ToolTipContentTemplateProperty = DependencyProperty.Register("ToolTipContentTemplate", typeof(DataTemplate), typeof(GanttChart), new PropertyMetadata(null));
        public static DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(GanttChart), new PropertyMetadata(false));
        public static DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(GanttChart), new PropertyMetadata(DateTime.Now));
        public static DependencyProperty SupportLinkVisibilityProperty = DependencyProperty.Register("SupportLinkVisibility", typeof(Visibility), typeof(GanttChart), new PropertyMetadata(Visibility.Visible));
        public static DependencyProperty TimespanBackgroundProperty = DependencyProperty.Register("TimespanBackground", typeof(Brush), typeof(GanttChart), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public static DependencyProperty TimespanBorderBrushProperty = DependencyProperty.Register("TimespanBorderBrush", typeof(Brush), typeof(GanttChart), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public static DependencyProperty TimespanBorderThicknessProperty = DependencyProperty.Register("TimespanBorderThickness", typeof(Thickness), typeof(GanttChart), new PropertyMetadata(new Thickness(1d)));
        public static DependencyProperty NodesProperty = DependencyProperty.Register("Nodes", typeof(ObservableCollection<IGanttNode>), typeof(GanttChart), new PropertyMetadata(new ObservableCollection<IGanttNode>()));
        public static DependencyProperty GapBackgroundBrushProperty = DependencyProperty.Register("GapBackgroundBrush", typeof(Brush), typeof(GanttChart), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static DependencyProperty GapBorderBrushProperty = DependencyProperty.Register("GapBorderBrush", typeof(Brush), typeof(GanttChart), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Brush GapBorderBrush
        {
            get { return (Brush)GetValue(GapBorderBrushProperty); }
            set { SetValue(GapBorderBrushProperty, value); }
        }
        public Brush GapBackgroundBrush
        {
            get { return (Brush)GetValue(GapBackgroundBrushProperty); }
            set { SetValue(GapBackgroundBrushProperty, value); }
        }

        public Brush GanttRowBorderBrush
        {
            get { return (Brush)GetValue(GanttRowBorderBrushProperty); }
            set { SetValue(GanttRowBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets and sets the visibility of the Horizontal Scrollbar.
        /// </summary>
        public Visibility HorizontalScrollbarVisibility
        {

            get
            {
                return (Visibility)GetValue(HorizontalScrollbarVisibilityProperty);
            }
            set
            {
                SetValue(HorizontalScrollbarVisibilityProperty, value);

                if (HorizontalScrollbar != null)
                    HorizontalScrollbar.Visibility = value;
            }
        }

        /// <summary>
        /// Gets and sets the Content Template that is shown in the tooltip 
        /// for the displayed GanttItems in the GanttPanel.
        /// </summary>
        public DataTemplate ToolTipContentTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipContentTemplateProperty); }
            set { SetValue(ToolTipContentTemplateProperty, value); }
        }
        /// <summary>
        /// Determines whether the datagrid can be edited and the items in the Gantt can be moved.
        /// </summary>
        public bool IsReadOnly { get { return (bool)GetValue(IsReadOnlyProperty); } set { SetValue(IsReadOnlyProperty, value); if (Panel != null) Panel.IsReadOnly = value; if (TaskGrid != null) TaskGrid.IsReadOnly = value; } }
        /// <summary>
        /// This is the current or starting time for the gantt chart.
        /// </summary>
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set
            {
                SetValue(CurrentTimeProperty, value);
                if (TimespanHeader != null)
                {
                    _CurrentTimeChangedLocally = true;
                    TimespanHeader.CurrentTime = value;

                }


            }
        }
        /// <summary>
        /// This is the brush to apply to the background of the TimespanHeader.
        /// </summary>
        public Brush TimespanBackground { get { return (Brush)GetValue(TimespanBackgroundProperty); } set { SetValue(TimespanBackgroundProperty, value); } }
        /// <summary>
        /// This is the brush to apply to the border of the TimespanHeader.
        /// </summary>
        public Brush TimespanBorderBrush { get { return (Brush)GetValue(TimespanBorderBrushProperty); } set { SetValue(TimespanBorderBrushProperty, value); } }
        /// <summary>
        /// This is the Thickness to apply to the border of the TimespanHeader.
        /// </summary>
        public Thickness TimespanBorderThickness { get { return (Thickness)GetValue(TimespanBorderThicknessProperty); } set { SetValue(TimespanBorderThicknessProperty, value); } }
        /// <summary>
        /// The visibility of the support link at the bottom of the control.
        /// </summary>
        public Visibility SupportLinkVisibility
        {
            get { return (Visibility)GetValue(SupportLinkVisibilityProperty); }
            set { SetValue(SupportLinkVisibilityProperty, value); }
        }
        /// <summary>
        /// A list of the top level nodes to display on the gantt chart.
        /// </summary>
        public ObservableCollection<IGanttNode> Nodes
        {
            get
            {
                return (ObservableCollection<IGanttNode>)GetValue(NodesProperty);
            }
            set
            {
                SetValue(NodesProperty, value);
                BindControlsToNodes();

            }
        }

        #endregion

        #region Private Variables
        private bool _CurrentTimeChangedLocally = false;
        private DateTime _StartScrollingTime = DateTime.MinValue;

        #endregion

        #region Properties
        private Type _ColumnGeneratingType;
        public Type ColumnGeneratingType { get { return _ColumnGeneratingType; } set { _ColumnGeneratingType = value; if (TaskGrid != null && Columns.Count == 0) TaskGrid.ColumnGeneratingType = value; } }

        private ObservableCollection<DataGridColumn> _Columns;
        public ObservableCollection<DataGridColumn> Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }

        }

        private ObservableCollection<GanttDependency> _Dependencies;
        public ObservableCollection<GanttDependency> Dependencies
        {
            get
            {
                if (_Dependencies == null)
                    _Dependencies = new ObservableCollection<GanttDependency>();

                return _Dependencies;
            }
            set
            {
                _Dependencies = value;
                if (Panel != null)
                    Panel.Dependencies = value;
            }
        }

        private TimeUnits _TopBarTimeUnits;
        public TimeUnits TopBarTimeUnits
        {
            get
            {
                if (TimespanHeader != null)
                    return (TimespanHeader.RowsPresenter.Children[0] as TimespanHeader.TimespanHeaderRow).TimeUnit;

                return _TopBarTimeUnits;
            }
            set
            {
                _TopBarTimeUnits = value; 
                
                if (TimespanHeader != null)
                {
                    (TimespanHeader.RowsPresenter.Children[0] as TimespanHeader.TimespanHeaderRow).TimeUnit = value;
                    {
                        TimespanHeader.InvalidateMeasure();
                        TimespanHeader.InvalidateArrange();
                    }
                }
            }
        }

        private TimeUnits _BottomBarTimeUnits;
        public TimeUnits BottomBarTimeUnits
        {
            get
            {
                if (TimespanHeader != null)
                    return (TimespanHeader.RowsPresenter.Children[1] as TimespanHeader.TimespanHeaderRow).TimeUnit;

                return _BottomBarTimeUnits;
            }
            set
            {
                _BottomBarTimeUnits = value;

                if (TimespanHeader != null)
                {
                    (TimespanHeader.RowsPresenter.Children[1] as TimespanHeader.TimespanHeaderRow).TimeUnit = value;
                    {
                        TimespanHeader.InvalidateMeasure();
                        TimespanHeader.InvalidateArrange();
                    }
                }
            }
        }

        #endregion

        #region Template Parts
        protected ScrollBar VerticalScrollbar { get; private set; }
        protected ScrollBar HorizontalScrollbar { get; private set; }
        protected GridSplitter GridSplitter { get; private set; }
        internal TimespanHeader.TimespanHeader TimespanHeader { get; private set; }
        protected GanttPanel Panel { get; private set; }
        protected GanttDataGrid TaskGrid { get; private set; }
        #endregion

        #region Constructors and Overrides
        public GanttChart()
        {

            _Columns = new ObservableCollection<DataGridColumn>();
            this.SizeChanged += new SizeChangedEventHandler(GanttChart_SizeChanged);

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TimespanHeader = (TimespanHeader.TimespanHeader)GetTemplateChild("TimespanElement");
            TimespanHeader.CurrentTimeChanged += Timespan_CurrentTimeChanged;
            TimespanHeader.ZoomFactorChanged += Timespan_ZoomFactorChanged;
            TimespanHeader.Loaded += new RoutedEventHandler(TimespanHeader_Loaded);


            Panel = (GanttPanel)GetTemplateChild("PanelElement");
            Panel.Nodes = Nodes;
            Panel.ParentGanttChart = this;
            Panel.Dependencies = this.Dependencies;


            TaskGrid = (GanttDataGrid)GetTemplateChild("TaskGrid");
            TaskGrid.LoadingRow += TaskGrid_LoadingRow;
            TaskGrid.ColumnGeneratingType = ColumnGeneratingType;
            TaskGrid.UnloadingRow += TaskGrid_UnloadingRow;
            TaskGrid.Loaded += new RoutedEventHandler(TaskGrid_Loaded);
            TaskGrid.RowExpandedChanged += new EventHandler<RowExpandedChangedEventArgs>(TaskGrid_RowExpanded);
            TaskGrid.Nodes = Nodes;
            Columns.ToList().ForEach(c => TaskGrid.Columns.Add(c));


            VerticalScrollbar = (ScrollBar)GetTemplateChild("VerticalScrollbar");
            VerticalScrollbar.Scroll += new ScrollEventHandler(VerticalScrollbar_Scroll);

            HorizontalScrollbar = (ScrollBar)GetTemplateChild("HorizontalScrollbar");
            HorizontalScrollbar.Scroll += new ScrollEventHandler(HorizontalScrollbar_Scroll);
            HorizontalScrollbar.MouseEnter += new MouseEventHandler(HorizontalScrollbar_MouseEnter);
            HorizontalScrollbar.MouseLeave += new MouseEventHandler(HorizontalScrollbar_MouseLeave);

            HorizontalScrollbar.Minimum = 0;
            HorizontalScrollbar.Maximum = 100;
            HorizontalScrollbar.SmallChange = 1;
            HorizontalScrollbar.LargeChange = 5;
            HorizontalScrollbar.Value = 50;

            GridSplitter = (GridSplitter)GetTemplateChild("GridSplitterElement");
            GridSplitter.MouseLeftButtonUp += new MouseButtonEventHandler(GridSplitter_MouseLeftButtonUp);

            SetupVerticalScrollbar();

        }







        #endregion

        #region Private functions
        private void BindControlsToNodes()
        {
            IEnumerable<IGanttNode> traversed = Nodes.Traverse<IGanttNode>(g =>
            {
                if (g.Expanded)
                    return g.ChildNodes;
                else
                    return null;
            });

            ObservableCollection<IGanttNode> list = new ObservableCollection<IGanttNode>();

            foreach (IGanttNode node in traversed)
                list.Add(node);


            if (Panel != null)
            {
                Panel.Nodes = list;
            }

            if (TaskGrid != null)
            {
                TaskGrid.Nodes = list;
            }

            SetupVerticalScrollbar();

        }
        private void SetupVerticalScrollbar()
        {
            if (Panel == null)
                return;

            IList<IGanttNode> list = Panel.Nodes;
            if (list == null)
                return;

            VerticalScrollbar.Maximum = list.Count;
            VerticalScrollbar.SmallChange = 2;
            VerticalScrollbar.LargeChange = 5;
            VerticalScrollbar.Visibility = (Panel.RowCount < list.Count || Panel.TopNodeIndex > 0) ? Visibility.Visible : Visibility.Collapsed;
            VerticalScrollbar.Value = Panel.TopNodeIndex;


        }
        #endregion

        #region Control event handlers
        private void GanttChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Panel != null)
            {
                Panel.ValidateRowCount();
            }
        }
        private void VerticalScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            IEnumerable<IGanttNode> list = (IEnumerable<IGanttNode>)TaskGrid.ItemsSource;

            int topIndex = (int)Math.Ceiling((VerticalScrollbar.Value / VerticalScrollbar.Maximum) * list.Count()) - 1;
            if (topIndex > 0)
                Panel.TopNodeIndex = topIndex;
            else
                Panel.TopNodeIndex = 0;

            TaskGrid.SetTopRow(Panel.TopNodeIndex);
        }

        private void HorizontalScrollbar_MouseEnter(object sender, MouseEventArgs e)
        {
            _StartScrollingTime = TimespanHeader.CurrentTime;
        }
        private void HorizontalScrollbar_MouseLeave(object sender, MouseEventArgs e)
        {
            HorizontalScrollbar.Value = 50;
        }
        private void HorizontalScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            TimespanHeader.CurrentTime = _StartScrollingTime.AddDays(e.NewValue - 50);

            if (e.NewValue == HorizontalScrollbar.Minimum || e.NewValue == HorizontalScrollbar.Maximum)
            {
                _StartScrollingTime = TimespanHeader.CurrentTime;
                HorizontalScrollbar.Value = 50;

            }
        }
        private void TaskGrid_Loaded(object sender, RoutedEventArgs e)
        {
            SetupVerticalScrollbar();

        }
        private void TaskGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {

        }
        private void TaskGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.GetIndex() == 0)
            {
                Panel.TopNodeIndex = Panel.Nodes.IndexOf(e.Row.DataContext as IGanttNode);
                SetupVerticalScrollbar();
            }

        }
        private void TaskGrid_RowExpanded(object sender, RowExpandedChangedEventArgs e)
        {
            TaskGrid.SetTopRow(Panel.TopNodeIndex);
            SetupVerticalScrollbar();

        }
        private void GridSplitter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            //This is necessary because the DataGrid will not refresh the hidden contents otherwise.
            IList<IGanttNode> nodes = (IList<IGanttNode>)TaskGrid.ItemsSource;
            foreach (IGanttNode node in nodes)
            {
                DataGridRow row = TaskGrid.GetDataItemRow(node);
                if (row != null)
                    row.InvalidateMeasure();
            }
        }
        private void Timespan_ZoomFactorChanged(object sender, EventArgs e)
        {
            HorizontalScrollbar.SmallChange = 1d / Zoom.Value;
            HorizontalScrollbar.LargeChange = 5d / Zoom.Value;

            Panel.InvalidateItemPositions();
        }
        private void Timespan_CurrentTimeChanged(object sender, EventArgs e)
        {
            if (!_CurrentTimeChangedLocally)
            {
                Panel.CurrentTime = TimespanHeader.CurrentTime;
            }
            else
                _CurrentTimeChangedLocally = false;

        }
        private void TimespanHeader_Loaded(object sender, RoutedEventArgs e)
        {
            if(_TopBarTimeUnits != TopBarTimeUnits)
                (TimespanHeader.RowsPresenter.Children[0] as TimespanHeader.TimespanHeaderRow).TimeUnit = _TopBarTimeUnits;

            if(_BottomBarTimeUnits != BottomBarTimeUnits)
                (TimespanHeader.RowsPresenter.Children[1] as TimespanHeader.TimespanHeaderRow).TimeUnit = _BottomBarTimeUnits;

            Panel.CurrentTime = TimespanHeader.CurrentTime;

        }
        #endregion

    }
}
