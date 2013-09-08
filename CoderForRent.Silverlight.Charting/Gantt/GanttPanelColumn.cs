using System;
using System.Windows.Controls;

namespace CoderForRent.Charting.Gantt
{
	public delegate void GanttPanelColumnHandler(object sender, GanttPanelColumnEventArgs e);
	public class GanttPanelColumnEventArgs : EventArgs
	{
		public GanttPanelColumn Column { get; set; }
        public bool Cancel { get; set; }
	}
	public class GanttPanelColumn : ContentControl
	{
		public DateTime RepresentedDate { get; set; }

		public GanttPanelColumn()
		{
			this.DefaultStyleKey = this.GetType();
            this.UseLayoutRounding = false;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}
	}
}
