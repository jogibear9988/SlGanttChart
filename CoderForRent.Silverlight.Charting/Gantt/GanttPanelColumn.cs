using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CoderForRent.Silverlight.Charting.Gantt
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
