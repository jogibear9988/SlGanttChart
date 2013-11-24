/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CoderForRent.Charting.Gantt
{
	public class RowExpandedChangedEventArgs : EventArgs
	{
		public DataGridRow Row { get; set; }
		public bool IsExpanded { get; set; }
	}
	public class GanttExpanderColumn : DataGridTextColumn
	{
	    public GanttExpanderColumn()
	    {
	        
	    }

		public event EventHandler<RowExpandedChangedEventArgs> RowExpandedChanged;
		protected void RaiseRowExpandedChanged(RowExpandedChangedEventArgs e)
		{
			if (RowExpandedChanged != null)
				RowExpandedChanged(this, e);
		}

		public SimpleExpander GetExpander(DataGridRow row)
		{
			StackPanel panel = (StackPanel)this.GetCellContent(row);
			return panel.Children[1] as SimpleExpander;

		}

	    protected override void RefreshCellContent(FrameworkElement element, string propertyName)
	    {
	        base.RefreshCellContent(element, propertyName);
	    }

	    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			//StackPanel panel = GeneratePanel(dataItem);
			//panel.Children.Add(base.GenerateEditingElement(cell, dataItem));

			//return panel;

			return base.GenerateEditingElement(cell, dataItem);

		}

		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			StackPanel panel = GeneratePanel(dataItem);
			panel.Children.Add(base.GenerateElement(cell,dataItem));

			return panel;
		}

		private StackPanel GeneratePanel(object dataItem)
		{
			StackPanel panel = new StackPanel();
			panel.Orientation = Orientation.Horizontal;

			IGanttNode node = (IGanttNode)dataItem;

			Binding bind = new Binding("Level");
			bind.ConverterParameter = dataItem;
			bind.Mode = BindingMode.OneWay;
			bind.Converter = new LevelToWidthConverter();

			Border b = new Border();
			b.BorderThickness = new Thickness(0);
			b.Background = new SolidColorBrush(Colors.Transparent);
			b.SetBinding(Border.WidthProperty, bind);

			bind = new Binding("Expanded");
			bind.Mode = BindingMode.TwoWay;

			SimpleExpander expander = new SimpleExpander();
			expander.Visibility = (node.ChildNodes.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
			expander.IsExpandedChanged += new EventHandler(expander_IsExpandedChanged);
			expander.SetBinding(SimpleExpander.IsExpandedProperty, bind);

			panel.Children.Add(b);
			panel.Children.Add(expander);

			return panel;
		}

		void expander_IsExpandedChanged(object sender, EventArgs e)
		{
			RaiseRowExpandedChanged(new RowExpandedChangedEventArgs { Row = DataGridRow.GetRowContainingElement((SimpleExpander)sender), IsExpanded = ((SimpleExpander)sender).IsExpanded });
		}
		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			TextBox box = editingElement as TextBox;
			if (box == null)
			{
				return string.Empty;
			}
			string text = box.Text;
			int length = text.Length;
			KeyEventArgs args = editingEventArgs as KeyEventArgs;
			if ((args != null) && (args.Key == Key.F2))
			{
				box.Select(length, length);
				return text;
			}
			box.Select(0, length);

			return base.PrepareCellForEdit(editingElement, editingEventArgs);
		}
	}
}
