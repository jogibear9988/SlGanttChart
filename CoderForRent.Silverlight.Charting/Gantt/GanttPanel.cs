/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CoderForRent.Charting.Core;

namespace CoderForRent.Charting.Gantt
{


	/// <summary>
	/// This control will display the gantt nodes' date ranges in a graphical layout.
	/// </summary>
	[
	TemplatePart(Name = "RowPresenter", Type = typeof(GanttRowsPresenter)),
	TemplatePart(Name = "ColumnPresenter", Type = typeof(GanttPanelColumnsPresenter)),
	TemplatePart(Name = "DependenciesPresenter", Type = typeof(GanttDependenciesPresenter)),
	TemplatePart(Name = "MainElement", Type = typeof(FrameworkElement))
	]
	public class GanttPanel : ContentControl
	{
		#region Dependency Properties
		public static DependencyProperty ToolTipContentTemplateProperty = DependencyProperty.Register("ToolTipContentTemplate", typeof(DataTemplate), typeof(GanttPanel), new PropertyMetadata(null));
		public static DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(GanttPanel), new PropertyMetadata(false));
		public static DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof(double), typeof(GanttPanel), new PropertyMetadata(20d));
		public static DependencyProperty GapBackgroundBrushProperty = DependencyProperty.Register("GapBackgroundBrush", typeof(Brush), typeof(GanttPanel), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
		public static DependencyProperty GapBorderBrushProperty = DependencyProperty.Register("GapBorderBrush", typeof(Brush), typeof(GanttPanel), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

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

		/// <summary>
		/// Defines the Content Template that is shown in the tooltip for the displayed GanttItems
		/// </summary>
		public DataTemplate ToolTipContentTemplate
		{
			get { return (DataTemplate)GetValue(ToolTipContentTemplateProperty); }
			set { SetValue(ToolTipContentTemplateProperty, value); }
		}
		/// <summary>
		/// Sets the Height of each of the GanttRows
		/// </summary>
		public double RowHeight { get { return (double)GetValue(RowHeightProperty); } set { SetValue(RowHeightProperty, value); } }
		/// <summary>
		/// Determines whether the items can be moved.
		/// </summary>
		public bool IsReadOnly { get { return (bool)GetValue(IsReadOnlyProperty); } set { SetValue(IsReadOnlyProperty, value); } }

		#endregion

		#region Events
		public event EventHandler<GanttItemEventArgs> ItemChanging;
		protected internal void RaiseItemChanging(GanttItemEventArgs e)
		{
			if (ItemChanging != null)
				ItemChanging(this, e);
		}


		public event EventHandler<GanttItemEventArgs> ItemChanged;
		protected internal void RaiseItemChanged(GanttItemEventArgs e)
		{
			if (ItemChanged != null)
				ItemChanged(this, e);

			UpdateDependencies(e.Item);
		}

		


		#endregion

		#region Properties
		/// <summary>
		/// This is the first DateTime represented in the panel.
		/// </summary>
		private DateTime _CurrentTime;

	    public DateTime CurrentTime
	    {
	        get { return _CurrentTime; }
	        set
	        {
	            _CurrentTime = value;
	            InvalidateItemPositions();
	        }
	    }

	    internal bool RowsValid { get; set; }

		private int _TopNodeIndex = 0;
		internal int TopNodeIndex
		{
			get { return _TopNodeIndex; }
			set
			{
				if (_TopNodeIndex != value)
				{
					_TopNodeIndex = value;
					ResetRows();
					UpdateDependencies();
				}
			}
		}
		internal int RowCount
		{
			get
			{
				if (RowPresenter == null)
					return 0;

				return this.RowPresenter.Children.Count;
			}
		}

		private ObservableCollection<IGanttNode> _Nodes;
		public ObservableCollection<IGanttNode> Nodes
		{
			get
			{
				return _Nodes;
			}
			set
			{
				_Nodes = value;
				_Nodes.CollectionChanged += Nodes_CollectionChanged;
				ResetRows();
				UpdateDependencies();
			}
		}

		private ObservableCollection<GanttDependency> _Dependencies;
		public ObservableCollection<GanttDependency> Dependencies
		{
			get
			{
				return _Dependencies;
			}
			set
			{
				if (_Dependencies != value)
				{
					_Dependencies = value;
					UpdateDependencies();
				}
			}
		}

		internal GanttChart ParentGanttChart { get; set; }
		#endregion

		#region Public Handlers

		internal double ConvertDateToPosition(DateTime date)
		{
			return TimeUnitScalar.GetPosition(CurrentTime, date);
		}
		internal TimeSpan ConvertDistanceToTimeSpan(double distance)
		{
			return TimeUnitScalar.GetTimespan(CurrentTime, distance);
		}
		#endregion

		#region Template Parts
		protected internal GanttRowsPresenter RowPresenter { get; private set; }
		protected internal GanttPanelColumnsPresenter ColumnPresenter { get; private set; }
		protected internal GanttDependenciesPresenter DependencyPresenter { get; private set; }
		protected internal FrameworkElement MainElement { get; private set; }
		#endregion

		#region Constructors and overrides

#if !SILVERLIGHT
        static GanttPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GanttPanel), new FrameworkPropertyMetadata(typeof(GanttPanel)));
        }
#endif

        public GanttPanel()
		{
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(GanttPanel);
#endif
			UseLayoutRounding = false;        
		}

        protected override Size MeasureOverride(Size availableSize)
	    {
            GenerateRows(availableSize);

	        return base.MeasureOverride(availableSize);
	    }

	    protected override Size ArrangeOverride(Size finalSize)
	    {
            Debug.WriteLine("GanttPanel.ArrangeOverride(" + finalSize.ToString() + ")");
	        RectangleGeometry r = new RectangleGeometry();
	        r.Rect = new Rect(0, 0, finalSize.Width - BorderThickness.Left - BorderThickness.Right,
	            finalSize.Height - BorderThickness.Top - BorderThickness.Bottom);
	        MainElement.Clip = r;

            if (UIHelpers.IsInDesignModeStatic)
	        {
	            return base.ArrangeOverride(finalSize);
	        }
            
	        GenerateRows(finalSize);

	        Size result = base.ArrangeOverride(finalSize);

	        return result;
	    }

	    public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			RowPresenter = (GanttRowsPresenter)GetTemplateChild("RowPresenter");
            RowPresenter.ParentPanel = this;

			ColumnPresenter = (GanttPanelColumnsPresenter)GetTemplateChild("ColumnPresenter");
			ColumnPresenter.ParentPanel = this;

			DependencyPresenter = (GanttDependenciesPresenter)GetTemplateChild("DependenciesPresenter");
			DependencyPresenter.ParentPanel = this;

			MainElement = (FrameworkElement)GetTemplateChild("MainElement");
		}

		#endregion

		#region Public functions

		public void InvalidateItemPositions()
		{
            Debug.WriteLine("GanttPanel.InvalidateItemPositions()");
			if (RowPresenter != null)
				RowPresenter.Children.OfType<GanttRow>().ToList().ForEach(r =>
				{
					//r.ItemsPresenter.InvalidateArrange();
					r.ItemsPresenter.InvalidateMeasure();
				}
				);

			if (ColumnPresenter != null)
				ColumnPresenter.Invalidate();

			if (DependencyPresenter != null)
				DependencyPresenter.Invalidate();
		}

		public void ValidateRowCount()
		{
			if (RowCount != (int)Math.Round(this.ActualWidth / this.RowHeight))
			{
				RowsValid = false;
				InvalidateArrange();
				//	GenerateRows(new Size(this.ActualWidth,this.ActualHeight));
			}
		}

		#endregion

		#region Private functions

	    private Size _oldSize;
	    internal bool ignoreSameSize = true; 

	    internal void ReGenerateRows()
	    {
	        RowsValid = false;
            //GenerateRows(_oldSize);
	        _oldSize = new Size();
            this.InvalidateMeasure();
            InvalidateItemPositions();
	    }

	    private void GenerateRows(Size finalSize)
		{
		    //MessageBox.Show(new System.Diagnostics.StackTrace().ToString());
            if (!RowsValid && (_oldSize != finalSize || !ignoreSameSize))
			{
			    _oldSize = finalSize;

				if (RowPresenter == null)
					return;

				RowsValid = true;
				RowPresenter.Children.Clear();

				double height = finalSize.Height;

		
				for (double i = 0; i < height; i += RowHeight)
				{
					GanttRow row = new GanttRow
					{
						ParentPanel = this,
						BorderBrush = this.ParentGanttChart.GanttRowBorderBrush,
						GapBackgroundBrush = this.GapBackgroundBrush,
						GapBorderBrush = this.GapBorderBrush
					};

					this.RowPresenter.Children.Add(row);
				}

				ResetRows();
			}
		}
		private void ResetRows()
		{
			if (RowPresenter == null)
				return;

			for (int i = 0; i < RowPresenter.Children.Count; i++)
			{
				GanttRow row = RowPresenter.Children[i] as GanttRow;
				row.BorderBrush = ParentGanttChart.GanttRowBorderBrush;
				row.Background = Background;
			    row._sameSizeCnt = 1;

				if (i + TopNodeIndex < Nodes.Count)
				{
					row.Node = Nodes[i + TopNodeIndex];
				}
				else
					row.Node = null;
			}
		}
		private GanttRow CreateRow(IGanttNode node)
		{
			GanttRow row = new GanttRow();
			row.BorderBrush = BorderBrush;
			row.Background = Background;
			row.ParentPanel = this;
			row.Node = node;
			return row;
		}
		private GanttRow CreateRow()
		{
			GanttRow row = new GanttRow();
			row.BorderBrush = BorderBrush;
			row.Background = Background;
			row.ParentPanel = this;
			return row;
		}
		private void UpdateDependencies()
		{
			if (DependencyPresenter != null)
				DependencyPresenter.Invalidate();
		}
		private void UpdateDependencies(GanttItem ganttItem)
		{
			var deps = Dependencies.Where(d => d.ChildNode == ganttItem.Node || d.ParentNode == ganttItem.Node);

			var items = DependencyPresenter.Children.Cast<UIElement>().Where(ui => (ui as GanttDependencyItem).Dependency.ChildNode == ganttItem.Node || (ui as GanttDependencyItem).Dependency.ParentNode == ganttItem.Node);

			foreach (UIElement ui in items)
			{
				(ui as GanttDependencyItem).UpdateDependencyLines();
			}

		}
		#endregion

		#region Event Handler functions
		private void Nodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					//int i = e.NewStartingIndex;

					//(from IGanttNode n in e.NewItems select CreateRow(n)).ToList<GanttRow>().ForEach(n =>{
					//    RowPresenter.Children.Insert(i++, n);

					//});

					ResetRows();
					break;
				case NotifyCollectionChangedAction.Remove:
					//RowPresenter.Children.OfType<GanttRow>().ToList().ForEach(row =>
					//    {
					//        if (e.OldItems.Contains(row.Node))
					//        {
					//            RowPresenter.Children.Remove(row);
					//            return;
					//        }
					//    }
					//);

					ResetRows();
					break;
				case NotifyCollectionChangedAction.Reset:
				default:
					ResetRows();
					break;
			}

			UpdateDependencies();
		}
		#endregion
	}
}
