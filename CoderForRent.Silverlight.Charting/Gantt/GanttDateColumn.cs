using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CoderForRent.Charting.Gantt
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
