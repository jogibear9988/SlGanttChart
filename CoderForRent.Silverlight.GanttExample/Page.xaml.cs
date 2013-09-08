/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using CoderForRent.Charting.Gantt;

namespace CoderForRent.Silverlight.GanttExample
{
	public partial class Page : UserControl
	{
		public Page()
		{
			InitializeComponent();
			gantt.GeneratingGanttPanelColumn += new GanttPanelColumnHandler(gantt_GeneratingGanttPanelColumn);
            
            //Change the zoom for using Hours.
            //Zoom.Value = 100;

			gantt.ColumnGeneratingType = typeof(ExampleGanttNode);
			gantt.Loaded += gantt_Loaded;
		}

		void gantt_GeneratingGanttPanelColumn(object sender, GanttPanelColumnEventArgs e)
		{
            //bool important = false;

            ////if it's the first of the month, add month separator
            //if (e.Column.RepresentedDate.Day == 1)
            //{
            //    e.Column.BorderThickness = new Thickness(1, 0, 0, 0);
            //    e.Column.BorderBrush = new SolidColorBrush(Colors.Gray);
            //    important = true;
            //}

            ////If weekend, then gray background. 
            //if (e.Column.RepresentedDate.DayOfWeek == DayOfWeek.Saturday || e.Column.RepresentedDate.DayOfWeek == DayOfWeek.Sunday)
            //{
            //    e.Column.Background = new SolidColorBrush(Colors.LightGray);
            //    important = true;
            //}

            //if (!important)
            //{
            //    e.Column.Background = new SolidColorBrush(Colors.Transparent);
            //    e.Cancel = true;
            //}
		}

		private void gantt_Loaded(object sender, RoutedEventArgs e)
		{

            List<MyTask> tasks = GetTasks(); ;
            ObservableCollection<IGanttNode> nodes = new ObservableCollection<IGanttNode>();
            foreach (MyTask t in tasks)
                nodes.Add(ConvertTaskToNode(t));

            gantt.Nodes = nodes;

            nodes[0].ChildNodes[0].Sections.Add(new GanttNodeSection() { BackgroundBrush = (Brush)this.Resources["greenGradiantSectionBrush"], StartDate = nodes[0].ChildNodes[0].EndDate.AddDays(1), EndDate = nodes[0].ChildNodes[0].EndDate.AddDays(3) });


            gantt.Dependencies.Add(new GanttDependency
            {
                ParentNode = nodes[0].ChildNodes[0],
                ChildNode = nodes[0].ChildNodes[1],
                Type = DependencyType.ChildBeginsAtParentEnd
            });

            gantt.Dependencies.Add(new GanttDependency
            {
                ParentNode = nodes[0].ChildNodes[1],
                ChildNode = nodes[0].ChildNodes[2],
                Type = DependencyType.ChildBeginsAtParentEnd
            });
		}

		private ExampleGanttNode ConvertTaskToNode(MyTask t)
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

            //Example of how to keep the time between sections equal... not necessary
			if (t.Children != null)
			{
				foreach (MyTask child in t.Children)
				{
					result.ChildNodes.Add(ConvertTaskToNode(child));
				}
			}

			return result;
		}

 
       


		public List<MyTask> GetTasks()
		{
			List<MyTask> nodes = new List<MyTask>
            {
                new MyTask{ TaskName="Test MyTask", StartDate=DateTime.Now, EndDate=DateTime.Now.AddDays(5), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Another Test MyTask", StartDate=DateTime.Now.AddDays(5), EndDate=DateTime.Now.AddDays(6), Resources="parker", PercentComplete=50d },
                new MyTask{ TaskName="Yet Test MyTask", StartDate=DateTime.Now.AddDays(6), EndDate=DateTime.Now.AddDays(10), Resources="dylan", PercentComplete=75d },
                new MyTask{ TaskName="Yo Momma's MyTask", StartDate=DateTime.Now.AddDays(10), EndDate=DateTime.Now.AddDays(20), Resources="missy", PercentComplete=2d },
                new MyTask{ TaskName="Yo Daddy's MyTask", StartDate=DateTime.Now.AddDays(20), EndDate=DateTime.Now.AddDays(30), Resources="missy", PercentComplete=10d },
                new MyTask{ TaskName="My MyTask's MyTask", StartDate=DateTime.Now.AddDays(30), EndDate=DateTime.Now.AddDays(40), Resources="missy", PercentComplete=40d },
                new MyTask{ TaskName="Your Next MyTask", StartDate=DateTime.Now.AddDays(40), EndDate=DateTime.Now.AddDays(50), Resources="missy", PercentComplete=50d },
                new MyTask{ TaskName="The Big MyTask", StartDate=DateTime.Now.AddDays(50), EndDate=DateTime.Now.AddDays(60), Resources="missy", PercentComplete=60d },
                new MyTask{ TaskName="The Small MyTask", StartDate=DateTime.Now.AddDays(60), EndDate=DateTime.Now.AddDays(70), Resources="missy", PercentComplete=70d },
                new MyTask{ TaskName="Nothing Special", StartDate=DateTime.Now.AddDays(70), EndDate=DateTime.Now.AddDays(80), Resources="missy", PercentComplete=80d },
                new MyTask{ TaskName="Something To Write Home About", StartDate=DateTime.Now.AddDays(80), EndDate=DateTime.Now.AddDays(90), Resources="missy", PercentComplete=90d }
            };

            nodes[0].Children = new ObservableCollection<MyTask>
            {
                new MyTask{ TaskName="Sub MyTask 1", StartDate= nodes[0].StartDate, EndDate=nodes[0].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Sub MyTask 2", StartDate= nodes[0].StartDate.AddDays(3), EndDate=nodes[0].StartDate.AddDays(5), Resources="parker", PercentComplete=50d },
                new MyTask{ TaskName="Sub MyTask 3", StartDate= nodes[0].StartDate.AddDays(6), EndDate=nodes[0].StartDate.AddDays(10), Resources="dylan", PercentComplete=75d },
                new MyTask{ TaskName="Sub MyTask 4", StartDate= nodes[0].StartDate.AddDays(11), EndDate=nodes[0].StartDate.AddDays(12), Resources="missy", PercentComplete=100d }
            };




            nodes[1].Children = new ObservableCollection<MyTask>
            {
                new MyTask{ TaskName="Sub MyTask 1", StartDate= nodes[1].StartDate, EndDate=nodes[1].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Sub MyTask 2", StartDate= nodes[1].StartDate.AddDays(2), EndDate=nodes[1].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new MyTask{ TaskName="Sub MyTask 3", StartDate= nodes[1].StartDate.AddDays(3), EndDate=nodes[1].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new MyTask{ TaskName="Sub MyTask 4", StartDate= nodes[1].StartDate.AddDays(5), EndDate=nodes[1].StartDate.AddDays(12), Resources="missy", PercentComplete=100d }
            };

            nodes[1].Children[0].Children = new ObservableCollection<MyTask>
            {
                new MyTask{ TaskName="Double Sub MyTask 1", StartDate= nodes[0].StartDate, EndDate=nodes[0].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Double Sub MyTask 2", StartDate= nodes[0].StartDate.AddDays(3), EndDate=nodes[0].StartDate.AddDays(6), Resources="dave", PercentComplete=25d }
			
            };
            nodes[2].Children = new ObservableCollection<MyTask>
            {
                new MyTask{ TaskName="Sub MyTask 1", StartDate= nodes[2].StartDate, EndDate=nodes[2].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Sub MyTask 2", StartDate= nodes[2].StartDate.AddDays(2), EndDate=nodes[2].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new MyTask{ TaskName="Sub MyTask 3", StartDate= nodes[2].StartDate.AddDays(3), EndDate=nodes[2].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new MyTask{ TaskName="Sub MyTask 4", StartDate= nodes[2].StartDate.AddDays(5), EndDate=nodes[2].StartDate.AddDays(7), Resources="missy", PercentComplete=100d }
            };


            nodes[3].Children = new ObservableCollection<MyTask>
            {
                new MyTask{ TaskName="Sub MyTask 1", StartDate= nodes[3].StartDate, EndDate=nodes[3].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Sub MyTask 2", StartDate= nodes[3].StartDate.AddDays(2), EndDate=nodes[3].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new MyTask{ TaskName="Sub MyTask 3", StartDate= nodes[3].StartDate.AddDays(3), EndDate=nodes[3].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new MyTask{ TaskName="Sub MyTask 4", StartDate= nodes[3].StartDate.AddDays(5), EndDate=nodes[3].StartDate.AddDays(17), Resources="missy", PercentComplete=100d }
            };


            nodes[4].Children = new ObservableCollection<MyTask>
            {
                new MyTask{ TaskName="Sub MyTask 1", StartDate= nodes[4].StartDate, EndDate=nodes[4].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Sub MyTask 2", StartDate= nodes[4].StartDate.AddDays(2), EndDate=nodes[4].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new MyTask{ TaskName="Sub MyTask 3", StartDate= nodes[4].StartDate.AddDays(3), EndDate=nodes[4].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new MyTask{ TaskName="Sub MyTask 4", StartDate= nodes[4].StartDate.AddDays(5), EndDate=nodes[4].StartDate.AddDays(12), Resources="missy", PercentComplete=100d }
            };


            nodes[5].Children = new ObservableCollection<MyTask>
            {
                new MyTask{ TaskName="Sub MyTask 1", StartDate= nodes[5].StartDate, EndDate=nodes[5].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new MyTask{ TaskName="Sub MyTask 2", StartDate= nodes[5].StartDate.AddDays(2), EndDate=nodes[5].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new MyTask{ TaskName="Sub MyTask 3", StartDate= nodes[5].StartDate.AddDays(3), EndDate=nodes[5].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new MyTask{ TaskName="Sub MyTask 4", StartDate= nodes[5].StartDate.AddDays(5), EndDate=nodes[5].StartDate.AddDays(11), Resources="missy", PercentComplete=100d }
            };

			return nodes;
		}

	}
}
