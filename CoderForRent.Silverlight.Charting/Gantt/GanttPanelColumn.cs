using System;
using System.Windows;
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

#if !SILVERLIGHT
        static GanttPanelColumn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GanttPanelColumn), new FrameworkPropertyMetadata(typeof(GanttPanelColumn)));
        }
#endif

        public GanttPanelColumn()
		{
#if SILVERLIGHT
			this.DefaultStyleKey = typeof(GanttPanelColumn);
#endif
            this.UseLayoutRounding = false;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}
	}
}
