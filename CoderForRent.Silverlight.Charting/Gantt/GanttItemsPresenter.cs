/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CoderForRent.Silverlight.Charting.Gantt
{
	/// <summary>
	/// This class will arrange and measure it's child items for it's parent row.
	/// </summary>
    public class GanttItemsPresenter : Panel
	{
		#region Properties
		internal GanttRow ParentRow { get; set; }
        
		#endregion

		#region Constructors and overrides
		public GanttItemsPresenter()
        {
            this.UseLayoutRounding = false;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            Debug.WriteLine("GanttItemsPresenter.ArrangeOverride()");
            for(int i = 0;i<Children.Count;i++) 
            {
				GanttItem gi = (GanttItem)Children[i];

                double x1, x2;
				//Actual item
                x1 = ParentRow.ParentPanel.ConvertDateToPosition(gi.Section.StartDate);
                x2 = ParentRow.ParentPanel.ConvertDateToPosition(gi.Section.EndDate);
                double width = x2 - x1;

				double x3 = 0d;

				//Gap
                if (gi.Node.ShowGap && i == Children.Count - 1 && ParentRow.RowIndex < ParentRow.ParentPanel.Nodes.Count - 1)
				{
					IGanttNode nextNode = ParentRow.ParentPanel.Nodes[ParentRow.RowIndex + 1];

					if (nextNode.StartDate > gi.Node.EndDate && nextNode.ParentNode == gi.Node.ParentNode)
					{
						x3 = ParentRow.ParentPanel.ConvertDateToPosition(nextNode.StartDate);
						width += x3 - x2;
					}
				}

		

                if(width > 0)
                    gi.Arrange(new Rect(x1, 0, width, ParentRow.ActualHeight));
                else
                    gi.Arrange(new Rect(0, 0, 0, ParentRow.ActualHeight));

            }
            return base.ArrangeOverride(finalSize);
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            Debug.WriteLine("GanttItemsPresenter.MeasureOverride(" + availableSize.ToString() + ")");
			for (int i = 0; i < Children.Count; i++)
			{
				GanttItem gi = (GanttItem)Children[i];

                double x1, x2;
                x1 = ParentRow.ParentPanel.ConvertDateToPosition( gi.Section.StartDate );
                x2 = ParentRow.ParentPanel.ConvertDateToPosition( gi.Section.EndDate );
                double width = x2 - x1;

				double x3 = 0d;

				//Gap
                if (gi.Node.ShowGap && i == Children.Count - 1 && ParentRow.RowIndex < ParentRow.ParentPanel.Nodes.Count - 1)
                {
                    IGanttNode nextNode = ParentRow.ParentPanel.Nodes[ParentRow.RowIndex + 1];

                    if (nextNode.StartDate > gi.Node.EndDate && nextNode.ParentNode == gi.Node.ParentNode)
                    {
                        x3 = ParentRow.ParentPanel.ConvertDateToPosition(nextNode.StartDate);
                        width += x3 - x2;
                    }
                }

				if (width < 0)
					width = 0;

				gi.Measure(new Size(width, ParentRow.ActualHeight));
            }
            
            return base.MeasureOverride(availableSize);
		}
		#endregion

    }
}
