/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * 
 * 
 * 
 * */

using System.Windows;

namespace CoderForRent.Charting.Gantt
{
	/// <summary>
	/// This class is the control that is displayed for nodes that have children.
	/// </summary>
	public class HeaderGanttItem : GanttItem
	{
		#region Constructors and overrides

#if !SILVERLIGHT
        static HeaderGanttItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderGanttItem), new FrameworkPropertyMetadata(typeof(HeaderGanttItem)));
        }
#endif
        public HeaderGanttItem()
		{
#if SILVERLIGHT
			this.DefaultStyleKey = typeof(HeaderGanttItem);
#endif
		}

		#endregion
	}
}
