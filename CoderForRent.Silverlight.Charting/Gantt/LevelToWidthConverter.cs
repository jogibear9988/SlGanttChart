/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System;
using System.Windows.Data;

namespace CoderForRent.Charting.Gantt
{
	/// <summary>
	/// This class converts a node's Level property into an indention width for the gantt chart.
	/// </summary>
	public class LevelToWidthConverter : IValueConverter
	{
		public const double LEVEL_INDENTION = 15d;

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			double width = 0d;

			int Level = (int)value;
			if ((parameter as IGanttNode).ChildNodes.Count == 0)
				width += 10;

			Level--;

			width += Level * LEVEL_INDENTION;


			return width;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int Width = (int)value;
			double result = (double)Width / LEVEL_INDENTION;
			result++;
			return result;
		}

		#endregion
	}
}
