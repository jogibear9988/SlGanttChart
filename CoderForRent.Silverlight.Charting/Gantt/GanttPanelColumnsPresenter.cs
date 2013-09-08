using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CoderForRent.Charting.Core;
using CoderForRent.Charting.TimespanHeader;

namespace CoderForRent.Charting.Gantt
{
	public class GanttPanelColumnsPresenter : Panel
	{
		internal GanttPanel ParentPanel { get; set; }

		protected override Size ArrangeOverride(Size finalSize)
		{
            Debug.WriteLine("GanttPanelColumnsPresenter.ArrangeOverride()");
			TimeUnits unit = (ParentPanel.ParentGanttChart.TimespanHeader.RowsPresenter.Children[ParentPanel.ParentGanttChart.TimespanHeader.RowsPresenter.Children.Count - 1] as TimespanHeaderRow).TimeUnit;

			foreach (GanttPanelColumn column in this.Children)
			{
				double unitWidth = TimeUnitScalar.ConvertToPixels(column.RepresentedDate, unit);
				Rect r = new Rect(TimeUnitScalar.GetPosition(ParentPanel.CurrentTime, column.RepresentedDate), 0d, unitWidth, finalSize.Height);

				column.Arrange( r);

			}
			return base.ArrangeOverride(finalSize);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
            Debug.WriteLine("GanttPanelColumnsPresenter.MeasureOverride()");
			TimeUnits unit = (ParentPanel.ParentGanttChart.TimespanHeader.RowsPresenter.Children[ParentPanel.ParentGanttChart.TimespanHeader.RowsPresenter.Children.Count - 1] as TimespanHeaderRow).TimeUnit;

			foreach (GanttPanelColumn column in this.Children)
			{
				double unitWidth = TimeUnitScalar.ConvertToPixels(column.RepresentedDate, unit);
				column.Measure(new Size(unitWidth, availableSize.Height));
			}
			return base.MeasureOverride(availableSize);
		}

		internal void Invalidate()
		{
            Debug.WriteLine("GanttPanelColumnsPresenter.Invalidate()");
			TimeUnits unit = (ParentPanel.ParentGanttChart.TimespanHeader.RowsPresenter.Children[ParentPanel.ParentGanttChart.TimespanHeader.RowsPresenter.Children.Count - 1] as TimespanHeaderRow).TimeUnit;
			double totalWidth = 0d;
			DateTime date = this.ParentPanel.CurrentTime;
			this.Children.Clear();

			while (totalWidth < this.ActualWidth)
			{

				GanttPanelColumnEventArgs e = new GanttPanelColumnEventArgs()
				{
					Column = new GanttPanelColumn()
					{
						 RepresentedDate = date
					}
				};

				ParentPanel.ParentGanttChart.RaiseGeneratingGanttPanelColumn(e);

                if(!e.Cancel)
				    this.Children.Add(e.Column);

				date = date.AddType(unit, 1);
				double unitWidth = TimeUnitScalar.ConvertToPixels(date, unit);

				totalWidth += unitWidth;
			}
		}
	}
}
