/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * 
 * 
 * 
 * */

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace CoderForRent.Silverlight.Charting.Gantt
{
	/// <summary>
	/// Arranges and Measures it's child GanttRows for it's parent Panel.
	/// </summary>
    public class GanttRowsPresenter : Panel
	{
		#region Properties
		internal GanttPanel ParentPanel { get; set; }
		#endregion

		#region Constructors and Overrides
		protected override Size ArrangeOverride(Size finalSize)
        {
            Debug.WriteLine("GanttRowsPresenter.ArrangeOverride(" + finalSize.ToString() + ")");
            double position = 0d;

            Children.OfType<GanttRow>().ToList<GanttRow>().ForEach(g =>
                {
                    if (g.Visibility == Visibility.Visible)
                    {
                        g.Arrange(new Rect(0d, position, finalSize.Width, ParentPanel.RowHeight));
                        position += ParentPanel.RowHeight;
                    }
                    else
                        g.Arrange(new Rect(0d, 0d, 0d, 0d));
                }
            );

            return base.ArrangeOverride(finalSize);
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            Debug.WriteLine("GanttRowsPresenter.MeasureOverride()");
            Children.OfType<GanttRow>().ToList<GanttRow>().ForEach(g =>
            {
                g.Measure(new Size(availableSize.Width, ParentPanel.RowHeight));
            }
            );

            return base.MeasureOverride(availableSize);
		}
		#endregion
	}
}
