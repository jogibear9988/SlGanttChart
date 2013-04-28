/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */
using System;
using System.Collections.Generic;

namespace CoderForRent.Silverlight.GanttExample
{
	public class Task
	{
		public string TaskName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public double PercentComplete { get; set; }
		public string Resources { get; set; }

		public List<Task> Children { get; set; }
	}
}
