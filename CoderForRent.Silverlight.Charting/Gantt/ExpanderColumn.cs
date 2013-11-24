using System.Windows;
using System.Windows.Controls;

namespace CoderForRent.Charting.Gantt
{
	public class ExpanderColumn : DataGridBoundColumn
	{
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			Border b = new Border();

			SimpleExpander Expander = new SimpleExpander();
			Expander.SetBinding(SimpleExpander.IsExpandedProperty, this.Binding);
			Expander.Visibility = ((dataItem as IGanttNode).ChildNodes.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
			b.Child = Expander;

			return b;
		}

		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			Border b = new Border();

			SimpleExpander Expander = new SimpleExpander();
			Expander.SetBinding(SimpleExpander.IsExpandedProperty, this.Binding);
			Expander.Visibility = ((dataItem as IGanttNode).ChildNodes.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
			b.Child = Expander;

			return b;
		}

		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			return ((editingElement as Border).Child as SimpleExpander).IsExpanded;
		}
	}
}
