/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CoderForRent.Charting.Gantt
{
	/// <summary>
	/// The node interface for use in the GanttChart control.
	/// </summary>
    public interface IGanttNode : INotifyPropertyChanged
    {
		/// <summary>
		/// Gets the name of the task.
		/// </summary>
        string TaskName { get; }

		/// <summary>
		/// Gets or sets the date that the task starts.
		/// </summary>
        DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the date that the task ends.
		/// </summary>
        DateTime EndDate { get; set; }

        /// <summary>
        /// Different items to place in the same row.
        /// </summary>
        ObservableCollection<GanttNodeSection> Sections { get; set; }

		/// <summary>
		/// Gets the resources needed for the task.
		/// </summary>
        string Resources { get; }

		/// <summary>
		/// Gets the percentage of work that has been completed for the task.  
		/// Format: 30.2 == 30.2%.
		/// </summary>
        double PercentComplete { get; }

		/// <summary>
		/// Gets or sets True if the node is expanded, False if it is collapsed.
		/// </summary>
        bool Expanded { get; set; }

		/// <summary>
		/// Gets or sets the parent of this node.
		/// </summary>
        IGanttNode ParentNode { get; set; }

		/// <summary>
		/// Gets a 1 base value that represents the depth of the current node in the node hierarchy.
		/// </summary>
		int Level { get; }

        /// <summary>
        /// Gets or sets whether the Gap should be shown for the node.
        /// </summary>
        bool ShowGap { get; set; }

		/// <summary>
		/// Gets or set the nodes that are children to this node.
		/// </summary>
        ObservableCollection<IGanttNode> ChildNodes { get; set; }



    }
}
