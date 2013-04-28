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
