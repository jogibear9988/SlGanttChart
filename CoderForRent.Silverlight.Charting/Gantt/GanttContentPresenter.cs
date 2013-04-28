/*
 * ********************************
 * Copyright © 2009. CoderForRent,LLC. All Rights Reserved.
 * 
 * Any use of this code without written permission from CoderForRent,LLC. is
 * strictly prohibited.
 * 
 * */

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
using System.Collections.Generic;
using System.Linq;

namespace CoderForRent.Silverlight.Charting.Gantt
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
