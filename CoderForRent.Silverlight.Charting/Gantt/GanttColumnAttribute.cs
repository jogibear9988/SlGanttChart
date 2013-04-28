/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;

namespace CoderForRent.Silverlight.Charting.Gantt
{
	public enum GanttColumnType
	{
		/// <summary>
		/// A typical text-only column.
		/// </summary>
		TextBlock,
		/// <summary>
		/// A date time picker should be shown in edit mode.
		/// </summary>
		DateTime
	}

	/// <summary>
	/// This attribute is applied to properties of classes that implement IGanttNode.
	/// The properties will then be shown in the corresponding GanttChart's data grid.
	/// </summary>
    public class GanttColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public int ColumnIndex { get; set; }
		private GanttColumnType _ColumnType = GanttColumnType.TextBlock;
		public GanttColumnType ColumnType { get { return _ColumnType; } set { _ColumnType = value; } }

        public GanttColumnAttribute()
        {
           
        }

    }
}
