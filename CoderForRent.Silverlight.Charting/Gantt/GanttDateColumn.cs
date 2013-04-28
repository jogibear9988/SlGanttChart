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
using System.Windows.Data;

namespace CoderForRent.Silverlight.Charting.Gantt
{
	public class GanttDateColumn : DataGridColumn
	{
		public Binding Binding { get; set; }
		public GanttDateColumn()
		{

		}

		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			DatePicker picker = new DatePicker();
			picker.SetBinding(DatePicker.SelectedDateProperty, this.Binding);
			picker.IsDropDownOpen = true;
			return picker;
				 
		}

		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			TextBlock block = new TextBlock
			{
				Margin = new Thickness(4.0),
				VerticalAlignment = VerticalAlignment.Center
			};
	
			block.SetBinding(TextBlock.TextProperty, this.Binding);
			return block;

		}


		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			DatePicker picker = editingElement as DatePicker;
			picker.IsDropDownOpen = true;

			return picker.SelectedDate;
		}
	}
}
