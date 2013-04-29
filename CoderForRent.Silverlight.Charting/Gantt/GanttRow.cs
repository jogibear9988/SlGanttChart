/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace CoderForRent.Silverlight.Charting.Gantt
{
	[TemplatePart(Name = "ItemsPresenterElement", Type = typeof(GanttItemsPresenter))]
	public class GanttRow : Control
	{
		#region Dependency Properties
		public static DependencyProperty GapBackgroundBrushProperty = DependencyProperty.Register("GapBackgroundBrush", typeof(Brush), typeof(GanttRow), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
		public static DependencyProperty GapBorderBrushProperty = DependencyProperty.Register("GapBorderBrush", typeof(Brush), typeof(GanttRow), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
	
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
		#endregion

		#region Template Parts
		internal GanttItemsPresenter ItemsPresenter { get; set; }
		#endregion

		#region Private variables
		Point _DragStart = new Point(0, 0);
		bool _ProcessingMove = false;
		#endregion

		#region Properties
		internal GanttPanel ParentPanel { get; set; }
		public bool ItemsValid { get; set; }
		private IGanttNode _Node;
		public IGanttNode Node
		{
			get { return _Node; }

			internal set
			{
				if (_Node != value)
				{
					_Node = value;
					ItemsValid = false;
					Invalidate();
				}
			}
		}
		public bool IsReadOnly { get { return ParentPanel.IsReadOnly; } }
        public int RowIndex { get { return ParentPanel.Nodes.IndexOf(Node); } }
		#endregion

		#region Constructors and overrides
		public GanttRow(IGanttNode node)
			: this()
		{
			this.Node = node;

		}
		public GanttRow()
		{
			this.DefaultStyleKey = this.GetType();
			UseLayoutRounding = false;
			ItemsValid = false;

			this.MouseMove += new MouseEventHandler(GanttRow_MouseMove);
			this.MouseLeftButtonUp += new MouseButtonEventHandler(GanttRow_MouseLeftButtonUp);
			this.MouseLeftButtonDown += new MouseButtonEventHandler(GanttRow_MouseLeftButtonDown);
			this.MouseLeave += new MouseEventHandler(GanttRow_MouseLeave);

		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			ItemsPresenter = (GanttItemsPresenter)GetTemplateChild("ItemsPresenterElement");
			ItemsPresenter.ParentRow = this;
		}

	    private Size _oldSize;
        private Size _oldRetSize;
	    private int _sameSizeCnt = 0;
		protected override Size ArrangeOverride(Size finalSize)
		{
            Debug.WriteLine("GanttRow.ArrangeOverride(" + finalSize.ToString() + ")");
            if (_oldSize != finalSize || _sameSizeCnt != 0)
            {
                if (_oldSize != finalSize)
                    _sameSizeCnt = 1;
                else
                    _sameSizeCnt--;

		        _oldSize = finalSize;
		        if (Node != null)
		            GenerateItems();
		        else
		            ItemsPresenter.Children.Clear();

		        ItemsPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
                _oldRetSize = base.ArrangeOverride(finalSize);

		        return _oldRetSize;
		    }

            return _oldRetSize;
		}
		protected override Size MeasureOverride(Size availableSize)
		{
            Debug.WriteLine("GanttRow.MeasureOverride()");
			ItemsPresenter.Measure(availableSize);
			return base.MeasureOverride(availableSize);
		}
		#endregion

		#region Event Handling Functions
		#region Drag/Drop
		private void GanttRow_MouseLeave(object sender, MouseEventArgs e)
		{
			_ProcessingMove = false;
			foreach(GanttItem item in ItemsPresenter.Children)
			{
				item.DragState = DragState.None;
			}
		}
		private void GanttRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!IsReadOnly)
				_DragStart = e.GetPosition(Application.Current.RootVisual);
		}
		private void GanttRow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			_DragStart = new Point(0, 0);
			_ProcessingMove = false;
			Cursor = Cursors.Arrow;

			if (!IsReadOnly)
			{
				ItemsPresenter.Children.OfType<GanttItem>().ToList().ForEach(item =>
				{
					if (item.DragState != DragState.None)
					{
						item.DragState = DragState.None;
					}
				});
			}
		}
		private void GanttRow_MouseMove(object sender, MouseEventArgs e)
		{
			if (_ProcessingMove || IsReadOnly)
				return;
			else
				_ProcessingMove = true;

			Point cursorPosition = e.GetPosition(Application.Current.RootVisual);

			List<GanttItem> items = ItemsPresenter.Children.OfType<GanttItem>().ToList();

			for(int i = 0; i< items.Count;i++)
			{
				GanttItem item = items[i];

				GeneralTransform gt = item.TransformToVisual(Application.Current.RootVisual);
				Point itemPosition = gt.Transform(new Point(0, 0));

				double distance = 0d;
				distance = cursorPosition.X - _DragStart.X;

				if (item.DragState == DragState.ResizeLeft)
				{
					Cursor = Cursors.SizeWE;
				}
				else if (item.DragState == DragState.ResizeRight)
				{
					Cursor = Cursors.SizeWE;
				}
				else if (item.DragState == DragState.Whole)
				{
					Cursor = Cursors.Hand;
				}
				else
				{
					_ProcessingMove = false;
					continue;
				}

				TimeSpan ts = GetTimeSpanFromDistance(distance);

#if DEBUG
                    ParentPanel.Content = item.Name;
					ParentPanel.Content = distance.ToString() + "   " + ts.ToString() + " " + item.DragState.ToString();
#endif

				if (Math.Abs(ts.TotalDays) >= 1)
				{

					if (item.DragState == DragState.ResizeLeft)
					{
						ParentPanel.RaiseItemChanging(new GanttItemEventArgs(item));
                        DateTime newDate = item.Section.StartDate.Add(ts);

                        if (newDate < item.Section.EndDate)
						{
							item.Section.StartDate = newDate;
                            item.InvalidateMeasure();
                            
							if(RowIndex > 0)
								(ParentPanel.RowPresenter.Children[RowIndex - 1] as GanttRow).Invalidate();
						}
						ParentPanel.RaiseItemChanged(new GanttItemEventArgs(item));
					}
					else if (item.DragState == DragState.ResizeRight)
					{

						ParentPanel.RaiseItemChanging(new GanttItemEventArgs(item));
                        DateTime newDate = item.Section.EndDate.Add(ts);

                        if (newDate > item.Section.StartDate)
						{
                            item.Section.EndDate = newDate;
							item.InvalidateMeasure();
							
							if (RowIndex > 0)
								(ParentPanel.RowPresenter.Children[RowIndex - 1] as GanttRow).Invalidate();

						}

						ParentPanel.RaiseItemChanged(new GanttItemEventArgs(item));
					}
					else if (item.DragState == DragState.Whole)
					{
						ParentPanel.RaiseItemChanging(new GanttItemEventArgs(item));

                        DateTime newStart = item.Section.StartDate.Add(ts);
						DateTime newEnd = item.Section.EndDate.Add(ts);

						if (distance > 0)
						{
							item.Section.EndDate = newEnd;
                            item.Section.StartDate = newStart;
						}
						else
						{
                            item.Section.StartDate = newStart;
                            item.Section.EndDate = newEnd;
						}
                        Invalidate();
                        
                       // item.InvalidateArrange();
						if (RowIndex > 0)
							(ParentPanel.RowPresenter.Children[RowIndex - 1] as GanttRow).Invalidate();

						ParentPanel.RaiseItemChanged(new GanttItemEventArgs(item));

					}

					_DragStart = e.GetPosition(Application.Current.RootVisual);

				}

			}

			_ProcessingMove = false;
		}
		#endregion
		#endregion

		#region Private functions
		private void GenerateItems()
		{
			if (Node == null)
			{
				ItemsPresenter.Children.Clear();
			}
            else if (!ItemsValid)
			{
				ItemsValid = true;
				ItemsPresenter.Children.Clear();

				GanttItem item = null;


                if (Node.Sections.Count == 1)
                {
                    if (Node.ChildNodes.Count == 0)
                        item = new GanttItem();
                    else
                        item = new HeaderGanttItem();

                    
                    item.GapBackgroundBrush = this.GapBackgroundBrush;
                    item.GapBorderBrush = this.GapBorderBrush;
                    item.ToolTipContent = this.Node;
                    item.ToolTipContentTemplate = this.ParentPanel.ToolTipContentTemplate;
                    item.Section = Node.Sections[0];
                    item.ParentRow = this;
                    item.Node = Node;
                    ItemsPresenter.Children.Add(item);

                }
                else
                {
                    foreach (GanttNodeSection section in Node.Sections)
                    {
                        item = new GanttItem();

                        if(section.BackgroundBrush != null)
                            item.Background = section.BackgroundBrush;

                        item.ToolTipContent = this.Node;
                        item.ToolTipContentTemplate = this.ParentPanel.ToolTipContentTemplate;
                        item.ParentRow = this;
                        item.Node = Node;
                        item.Section = section;
                        ItemsPresenter.Children.Add(item);
                    }
                }
			}
		}
		#endregion

		#region Internal/Public functions
		internal void Invalidate()
		{
            Debug.WriteLine("GanttRow.Invalidate()");
			if (ItemsPresenter == null)
				return;

		    ParentPanel.RowsValid = false;

			//this.InvalidateArrange();
			this.InvalidateMeasure();

			//ItemsPresenter.InvalidateArrange();
			ItemsPresenter.InvalidateMeasure();
		}
		internal TimeSpan GetTimeSpanFromDistance(double distance)
		{
			return ParentPanel.ConvertDistanceToTimeSpan(distance);
			//return new TimeSpan((int)Math.Round(distance / 16.6d), 0, 0, 0);
		}
		#endregion
	}
}
