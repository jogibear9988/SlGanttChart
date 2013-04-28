using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using CoderForRent.Silverlight.Charting.Gantt;
using CoderForRent.Silverlight.GanttExample.TaskService;

namespace CoderForRent.Silverlight.GanttExample
{
	public partial class PageWCFDriven : UserControl
	{
		public PageWCFDriven()
		{
			InitializeComponent();
			gantt.GeneratingGanttPanelColumn += new CoderForRent.Silverlight.Charting.Gantt.GanttPanelColumnHandler(gantt_GeneratingGanttPanelColumn);
			gantt.ColumnGeneratingType = typeof(ExampleGanttNode);
			gantt.Loaded += gantt_Loaded;
		}

		void gantt_GeneratingGanttPanelColumn(object sender, CoderForRent.Silverlight.Charting.Gantt.GanttPanelColumnEventArgs e)
		{
			bool important = false;

			//if it's the first of the month, add month separator
			if (e.Column.RepresentedDate.Day == 1)
			{
				e.Column.BorderThickness = new Thickness(1, 0, 0, 0);
				e.Column.BorderBrush = new SolidColorBrush(Colors.Gray);
				important = true;
			}

			//If weekend, then gray background. 
			if (e.Column.RepresentedDate.DayOfWeek == DayOfWeek.Saturday || e.Column.RepresentedDate.DayOfWeek == DayOfWeek.Sunday)
			{
				e.Column.Background = new SolidColorBrush(Colors.LightGray);
				important = true;
			}

			if (!important)
			{
				e.Column.Background = new SolidColorBrush(Colors.Transparent);
				e.Cancel = true;
			}
		}

		private void gantt_Loaded(object sender, RoutedEventArgs e)
		{
			TaskServiceClient client = new TaskServiceClient();
			client.GetTasksCompleted += client_GetTasksCompleted;
			client.GetTasksAsync();

		}
		private void client_GetTasksCompleted(object sender, GetTasksCompletedEventArgs e)
		{
			ObservableCollection<Task> tasks = e.Result;
			ObservableCollection<IGanttNode> nodes = new ObservableCollection<IGanttNode>();
			foreach (Task t in tasks)
				nodes.Add(ConvertTaskToNode(t));

			gantt.Nodes = nodes;
		}
		private ExampleGanttNode ConvertTaskToNode(Task t)
		{
			ExampleGanttNode result = new ExampleGanttNode
			{
				TaskName = t.TaskName,
				StartDate = t.StartDate,
				EndDate = t.EndDate,
				Resources = t.Resources,
				PercentComplete = t.PercentComplete,
				Expanded = false

			};

			if (t.Children != null)
			{
				foreach (Task child in t.Children)
				{
					result.ChildNodes.Add(ConvertTaskToNode(child));
				}
			}

			return result;
		}
	}
}
