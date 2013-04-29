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

namespace CoderForRent.Silverlight.GanttExample
{
	public partial class PageWithColumnsDefined : UserControl
	{
		public PageWithColumnsDefined()
		{
			InitializeComponent();
			gantt.Loaded += gantt_Loaded;
		}

		private void gantt_Loaded(object sender, RoutedEventArgs e)
		{


			ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode> nodes = new System.Collections.ObjectModel.ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode>
            {
                new ExampleGanttNode{ TaskName="Test MyTask", StartDate=DateTime.Now, EndDate=DateTime.Now.AddDays(5), Resources="dave", PercentComplete=25d },
                new ExampleGanttNode{ TaskName="Another Test MyTask", StartDate=DateTime.Now.AddDays(5), EndDate=DateTime.Now.AddDays(6), Resources="parker", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="Yet Test MyTask", StartDate=DateTime.Now.AddDays(6), EndDate=DateTime.Now.AddDays(10), Resources="dylan", PercentComplete=75d },
                new ExampleGanttNode{ TaskName="Yo Momma's MyTask", StartDate=DateTime.Now.AddDays(10), EndDate=DateTime.Now.AddDays(20), Resources="missy", PercentComplete=2d },
                new ExampleGanttNode{ TaskName="Yo Daddy's MyTask", StartDate=DateTime.Now.AddDays(20), EndDate=DateTime.Now.AddDays(30), Resources="missy", PercentComplete=10d },
                new ExampleGanttNode{ TaskName="My MyTask's MyTask", StartDate=DateTime.Now.AddDays(30), EndDate=DateTime.Now.AddDays(40), Resources="missy", PercentComplete=40d },
                new ExampleGanttNode{ TaskName="Your Next MyTask", StartDate=DateTime.Now.AddDays(40), EndDate=DateTime.Now.AddDays(50), Resources="missy", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="The Big MyTask", StartDate=DateTime.Now.AddDays(50), EndDate=DateTime.Now.AddDays(60), Resources="missy", PercentComplete=60d },
                new ExampleGanttNode{ TaskName="The Small MyTask", StartDate=DateTime.Now.AddDays(60), EndDate=DateTime.Now.AddDays(70), Resources="missy", PercentComplete=70d },
                new ExampleGanttNode{ TaskName="Nothing Special", StartDate=DateTime.Now.AddDays(70), EndDate=DateTime.Now.AddDays(80), Resources="missy", PercentComplete=80d },
                new ExampleGanttNode{ TaskName="Something To Write Home About", StartDate=DateTime.Now.AddDays(80), EndDate=DateTime.Now.AddDays(90), Resources="missy", PercentComplete=90d }
            };

			nodes[0].ChildNodes = new System.Collections.ObjectModel.ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode>
            {
                new ExampleGanttNode{ TaskName="Sub MyTask 1", StartDate= nodes[0].StartDate, EndDate=nodes[0].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new ExampleGanttNode{ TaskName="Sub MyTask 2", StartDate= nodes[0].StartDate.AddDays(2), EndDate=nodes[0].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="Sub MyTask 3", StartDate= nodes[0].StartDate.AddDays(3), EndDate=nodes[0].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new ExampleGanttNode{ TaskName="Sub MyTask 4", StartDate= nodes[0].StartDate.AddDays(5), EndDate=nodes[0].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[1].ChildNodes = new System.Collections.ObjectModel.ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode>
            {
                new ExampleGanttNode{ TaskName="Sub MyTask 1", StartDate= nodes[1].StartDate, EndDate=nodes[1].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new ExampleGanttNode{ TaskName="Sub MyTask 2", StartDate= nodes[1].StartDate.AddDays(2), EndDate=nodes[1].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="Sub MyTask 3", StartDate= nodes[1].StartDate.AddDays(3), EndDate=nodes[1].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new ExampleGanttNode{ TaskName="Sub MyTask 4", StartDate= nodes[1].StartDate.AddDays(5), EndDate=nodes[1].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[2].ChildNodes = new System.Collections.ObjectModel.ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode>
            {
                new ExampleGanttNode{ TaskName="Sub MyTask 1", StartDate= nodes[2].StartDate, EndDate=nodes[2].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new ExampleGanttNode{ TaskName="Sub MyTask 2", StartDate= nodes[2].StartDate.AddDays(2), EndDate=nodes[2].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="Sub MyTask 3", StartDate= nodes[2].StartDate.AddDays(3), EndDate=nodes[2].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new ExampleGanttNode{ TaskName="Sub MyTask 4", StartDate= nodes[2].StartDate.AddDays(5), EndDate=nodes[2].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[3].ChildNodes = new System.Collections.ObjectModel.ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode>
            {
                new ExampleGanttNode{ TaskName="Sub MyTask 1", StartDate= nodes[3].StartDate, EndDate=nodes[3].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new ExampleGanttNode{ TaskName="Sub MyTask 2", StartDate= nodes[3].StartDate.AddDays(2), EndDate=nodes[3].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="Sub MyTask 3", StartDate= nodes[3].StartDate.AddDays(3), EndDate=nodes[3].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new ExampleGanttNode{ TaskName="Sub MyTask 4", StartDate= nodes[3].StartDate.AddDays(5), EndDate=nodes[3].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[4].ChildNodes = new System.Collections.ObjectModel.ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode>
            {
                new ExampleGanttNode{ TaskName="Sub MyTask 1", StartDate= nodes[4].StartDate, EndDate=nodes[4].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new ExampleGanttNode{ TaskName="Sub MyTask 2", StartDate= nodes[4].StartDate.AddDays(2), EndDate=nodes[4].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="Sub MyTask 3", StartDate= nodes[4].StartDate.AddDays(3), EndDate=nodes[4].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new ExampleGanttNode{ TaskName="Sub MyTask 4", StartDate= nodes[4].StartDate.AddDays(5), EndDate=nodes[4].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[5].ChildNodes = new System.Collections.ObjectModel.ObservableCollection<CoderForRent.Silverlight.Charting.Gantt.IGanttNode>
            {
                new ExampleGanttNode{ TaskName="Sub MyTask 1", StartDate= nodes[5].StartDate, EndDate=nodes[5].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new ExampleGanttNode{ TaskName="Sub MyTask 2", StartDate= nodes[5].StartDate.AddDays(2), EndDate=nodes[5].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new ExampleGanttNode{ TaskName="Sub MyTask 3", StartDate= nodes[5].StartDate.AddDays(3), EndDate=nodes[5].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new ExampleGanttNode{ TaskName="Sub MyTask 4", StartDate= nodes[5].StartDate.AddDays(5), EndDate=nodes[5].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };

			gantt.Nodes = nodes;
		}

	}
}
