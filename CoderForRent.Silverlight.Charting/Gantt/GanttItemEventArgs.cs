using System;

namespace CoderForRent.Charting.Gantt
{
	public class GanttItemEventArgs : EventArgs
	{
		public GanttItem Item { get; set; }
		public GanttItemEventArgs(GanttItem item)
		{
			Item = item;
		}
	}
}
