/*
 * ********************************
 * Copyright © 2009. CoderForRent,LLC. All Rights Reserved.
 * 
 * Any use of this code without written permission from CoderForRent,LLC. is
 * strictly prohibited.
 * 
 * */

using System.Windows.Controls;
using System.Windows.Input;

namespace CoderForRent.Charting.Gantt
{
    public class GanttContentPresenter : ContentPresenter
    {
        public GanttContentPresenter()
		{
			this.MouseLeftButtonDown += new MouseButtonEventHandler(GanttContentPresenter_MouseLeftButtonDown);
			this.MouseLeftButtonUp += new MouseButtonEventHandler(GanttContentPresenter_MouseLeftButtonUp);

			
		}
		
	

		void GanttContentPresenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = false;
		}

	

		void GanttContentPresenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = false;
		}
		
    }
}
