/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CoderForRent.Silverlight.Charting.Gantt
{
	/// <summary>
	/// A drag state determines how the mouse move events should act on a GanttItem
	/// </summary>
	public enum DragState
	{
		/// <summary>
		/// Move the entire item.
		/// </summary>
		Whole,
		/// <summary>
		/// Do not move the item
		/// </summary>
		None,
		/// <summary>
		/// The right side of the item is being drug.
		/// </summary>
		ResizeRight,
		/// <summary>
		/// The left side of the item is being drug.
		/// </summary>
		ResizeLeft

	}

	/// <summary>
	/// This control represents a node's date range in the GanttPanel
	/// </summary>
	/// 
	[TemplatePart(Name = "GapElement", Type = typeof(FrameworkElement))]
	public class GanttItem : Control
	{
		#region Constants
		public const double HANDLE_MARGIN = 5.0;
		#endregion

		#region Dependency Properties

		public static DependencyProperty ToolTipContentProperty = DependencyProperty.Register("ToolTipContent", typeof(object), typeof(GanttItem), new PropertyMetadata(null));
		public static DependencyProperty ToolTipContentTemplateProperty = DependencyProperty.Register("ToolTipContentTemplate", typeof(DataTemplate), typeof(GanttItem), new PropertyMetadata(null));
		public static DependencyProperty IsDragDropEnabledProperty = DependencyProperty.Register("IsDragDropEnabled", typeof(bool), typeof(GanttItem), new PropertyMetadata(true));
		public static DependencyProperty PercentCompleteWidthProperty = DependencyProperty.Register("PercentCompleteWidth", typeof(double), typeof(GanttItem), new PropertyMetadata(0d));
		public static DependencyProperty GapBackgroundBrushProperty = DependencyProperty.Register("GapBackgroundBrush", typeof(Brush), typeof(GanttItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
		public static DependencyProperty GapBorderBrushProperty = DependencyProperty.Register("GapBorderBrush", typeof(Brush), typeof(GanttItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
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
		public object ToolTipContent
		{
			get { return GetValue(ToolTipContentProperty); }
			set
			{

				SetValue(ToolTipContentProperty, value);

			}
		}
		public DataTemplate ToolTipContentTemplate
		{
			get { return (DataTemplate)GetValue(ToolTipContentTemplateProperty); }
			set
			{
				SetValue(ToolTipContentTemplateProperty, value);
			}
		}
		public double PercentCompleteWidth
		{
			get
			{
				return (double)GetValue(PercentCompleteWidthProperty);
			}

			internal set
			{
				SetValue(PercentCompleteWidthProperty, value);
			}
		}
		public bool IsDragDropEnabled
		{
			get
			{
				return !ParentRow.ParentPanel.IsReadOnly && (bool)GetValue(IsDragDropEnabledProperty);
			}
			set
			{
				SetValue(IsDragDropEnabledProperty, value);

			}

		}
		#endregion

		#region Template Parts
		protected internal FrameworkElement GapElement { get; set; }
		protected internal FrameworkElement NodeElement { get; set; }
		#endregion

		#region Properties
		private IGanttNode _Node;
		internal IGanttNode Node
		{
			get { return _Node; }
			set
			{
				_Node = value;
				_Node.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Node_PropertyChanged);
			}
		}

        internal GanttNodeSection Section { get; set; }
		internal GanttRow ParentRow { get; set; }
		private DragState _DragState = DragState.None;
		public DragState DragState { get { return _DragState; } set { _DragState = value; } }
		protected internal double NodeWidth { get; set; }
		#endregion

		#region Private variables
		private Grid _PercentCompleteElement;
		private double _OriginalPercentCompleteElementHeight = 0d;
		#endregion

		#region Constructors and overrides
		public GanttItem()
		{
			this.DefaultStyleKey = this.GetType();

			UseLayoutRounding = false;
			this.MouseEnter += new MouseEventHandler(GanttItem_MouseEnter);
			this.MouseLeave += new MouseEventHandler(GanttItem_MouseLeave);
			this.MouseMove += new MouseEventHandler(GanttItem_MouseMove);
			this.MouseLeftButtonDown += new MouseButtonEventHandler(GanttItem_MouseLeftButtonDown);
			this.MouseLeftButtonUp += new MouseButtonEventHandler(GanttItem_MouseLeftButtonUp);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_PercentCompleteElement = (Grid)GetTemplateChild("PercentCompleteElement");

			GapElement = (FrameworkElement)GetTemplateChild("GapElement");

            if (GapElement != null)
            {
                if (!Node.ShowGap)
                    GapElement.Opacity = 0;

            }

			NodeElement = (FrameworkElement)GetTemplateChild("NodeElement");
		}
		protected override Size MeasureOverride(Size availableSize)
		{
			double nodeStartPosition = ParentRow.ParentPanel.ConvertDateToPosition(Section.StartDate);
            double nodeEndPosition = ParentRow.ParentPanel.ConvertDateToPosition(Section.EndDate);
			NodeWidth = (nodeEndPosition - nodeStartPosition);

			if (NodeWidth > 0)
				PercentCompleteWidth = (Node.PercentComplete / 100) * NodeWidth;
			else
				PercentCompleteWidth = 0;

			if (GapElement != null)
			{
				GapElement.MinWidth = 0;
				NodeElement.MaxWidth = availableSize.Width;

				if (ParentRow.RowIndex < ParentRow.ParentPanel.Nodes.Count - 1)
				{
					IGanttNode nextNode = ParentRow.ParentPanel.Nodes[ParentRow.RowIndex + 1];
					if (nextNode.StartDate > Node.EndDate)
					{
						double gapWidth = availableSize.Width - NodeWidth;

						if (NodeWidth > 0)
						{
							NodeElement.MaxWidth = NodeWidth;
						}
						else
						{
							NodeElement.MaxWidth = 0;
						}
						if( gapWidth > 0)
						{
							GapElement.MinWidth = gapWidth;
						}
						else
						{
							GapElement.MinWidth = 0;
						}
					}

				}
			}

			return base.MeasureOverride(availableSize);
		}
		#endregion

		#region Event Handlers
		private void Node_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			ParentRow.Invalidate();

			if (e.PropertyName == "PercentComplete")
			{
				if (_PercentCompleteElement == null)
					return;

				PercentCompleteWidth = (Node.PercentComplete / 100) * NodeWidth;

				_PercentCompleteElement.Width = PercentCompleteWidth;
				if (_PercentCompleteElement.Width == 0)
				{
					_OriginalPercentCompleteElementHeight = _PercentCompleteElement.Height;
					_PercentCompleteElement.Height = 0;
				}
				else if (_OriginalPercentCompleteElementHeight > 0)
					_PercentCompleteElement.Height = _OriginalPercentCompleteElementHeight;

			}

		}
		#endregion

		#region Drag/Drop
		void GanttItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DragState = DragState.None;
		}
		void GanttItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (IsDragDropEnabled)
			{
				Point p = e.GetPosition(this);

				if (p.X < HANDLE_MARGIN)
				{
					DragState = DragState.ResizeLeft;
				}
				else if (p.X > NodeWidth - HANDLE_MARGIN && p.X < NodeWidth + HANDLE_MARGIN)
				{
					DragState = DragState.ResizeRight;
				}
				else if(p.X < NodeWidth)
				{
					DragState = DragState.Whole;
				}
			}
		}
		void GanttItem_MouseMove(object sender, MouseEventArgs e)
		{
			Point p = e.GetPosition(this);

			if (DragState == DragState.None && IsDragDropEnabled)
			{
				if (p.X < HANDLE_MARGIN)
				{
					this.Cursor = Cursors.SizeWE;
				}
				else if (p.X > NodeWidth - HANDLE_MARGIN && p.X < NodeWidth + HANDLE_MARGIN)
				{
					this.Cursor = Cursors.SizeWE;
				}
				else if (p.X < NodeWidth)
				{
					this.Cursor = Cursors.Hand;
				}
				else
					this.Cursor = Cursors.Arrow;
			}
		}
		void GanttItem_MouseLeave(object sender, MouseEventArgs e)
		{
			if (DragState == DragState.None)
			{
				VisualStateManager.GoToState(this, "Normal", true);

			}
		}
		void GanttItem_MouseEnter(object sender, MouseEventArgs e)
		{
			if (DragState == DragState.None)
			{
				VisualStateManager.GoToState(this, "MouseOver", true);
			}
		}
		#endregion
	}
}
