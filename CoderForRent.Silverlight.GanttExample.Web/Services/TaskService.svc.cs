using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace CoderForRent.Silverlight.GanttExample.Web.Services
{
	[ServiceContract(Namespace = "")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class TaskService
	{
		[OperationContract]
		public List<Task> GetTasks()
		{
			List<Task> nodes = new List<Task>
            {
                new Task{ TaskName="Test Task", StartDate=DateTime.Now, EndDate=DateTime.Now.AddDays(5), Resources="dave", PercentComplete=25d },
                new Task{ TaskName="Another Test Task", StartDate=DateTime.Now.AddDays(5), EndDate=DateTime.Now.AddDays(6), Resources="parker", PercentComplete=50d },
                new Task{ TaskName="Yet Test Task", StartDate=DateTime.Now.AddDays(6), EndDate=DateTime.Now.AddDays(10), Resources="dylan", PercentComplete=75d },
                new Task{ TaskName="Yo Momma's Task", StartDate=DateTime.Now.AddDays(10), EndDate=DateTime.Now.AddDays(20), Resources="missy", PercentComplete=2d },
                new Task{ TaskName="Yo Daddy's Task", StartDate=DateTime.Now.AddDays(20), EndDate=DateTime.Now.AddDays(30), Resources="missy", PercentComplete=10d },
                new Task{ TaskName="My Task's Task", StartDate=DateTime.Now.AddDays(30), EndDate=DateTime.Now.AddDays(40), Resources="missy", PercentComplete=40d },
                new Task{ TaskName="Your Next Task", StartDate=DateTime.Now.AddDays(40), EndDate=DateTime.Now.AddDays(50), Resources="missy", PercentComplete=50d },
                new Task{ TaskName="The Big Task", StartDate=DateTime.Now.AddDays(50), EndDate=DateTime.Now.AddDays(60), Resources="missy", PercentComplete=60d },
                new Task{ TaskName="The Small Task", StartDate=DateTime.Now.AddDays(60), EndDate=DateTime.Now.AddDays(70), Resources="missy", PercentComplete=70d },
                new Task{ TaskName="Nothing Special", StartDate=DateTime.Now.AddDays(70), EndDate=DateTime.Now.AddDays(80), Resources="missy", PercentComplete=80d },
                new Task{ TaskName="Something To Write Home About", StartDate=DateTime.Now.AddDays(80), EndDate=DateTime.Now.AddDays(90), Resources="missy", PercentComplete=90d }
            };

			nodes[0].Children = new List<Task>
            {
                new Task{ TaskName="Sub Task 1", StartDate= nodes[0].StartDate, EndDate=nodes[0].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new Task{ TaskName="Sub Task 2", StartDate= nodes[0].StartDate.AddDays(2), EndDate=nodes[0].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new Task{ TaskName="Sub Task 3", StartDate= nodes[0].StartDate.AddDays(3), EndDate=nodes[0].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new Task{ TaskName="Sub Task 4", StartDate= nodes[0].StartDate.AddDays(5), EndDate=nodes[0].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[1].Children = new List<Task>
            {
                new Task{ TaskName="Sub Task 1", StartDate= nodes[1].StartDate, EndDate=nodes[1].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new Task{ TaskName="Sub Task 2", StartDate= nodes[1].StartDate.AddDays(2), EndDate=nodes[1].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new Task{ TaskName="Sub Task 3", StartDate= nodes[1].StartDate.AddDays(3), EndDate=nodes[1].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new Task{ TaskName="Sub Task 4", StartDate= nodes[1].StartDate.AddDays(5), EndDate=nodes[1].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[2].Children = new List<Task>
            {
                new Task{ TaskName="Sub Task 1", StartDate= nodes[2].StartDate, EndDate=nodes[2].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new Task{ TaskName="Sub Task 2", StartDate= nodes[2].StartDate.AddDays(2), EndDate=nodes[2].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new Task{ TaskName="Sub Task 3", StartDate= nodes[2].StartDate.AddDays(3), EndDate=nodes[2].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new Task{ TaskName="Sub Task 4", StartDate= nodes[2].StartDate.AddDays(5), EndDate=nodes[2].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[3].Children = new List<Task>
            {
                new Task{ TaskName="Sub Task 1", StartDate= nodes[3].StartDate, EndDate=nodes[3].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new Task{ TaskName="Sub Task 2", StartDate= nodes[3].StartDate.AddDays(2), EndDate=nodes[3].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new Task{ TaskName="Sub Task 3", StartDate= nodes[3].StartDate.AddDays(3), EndDate=nodes[3].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new Task{ TaskName="Sub Task 4", StartDate= nodes[3].StartDate.AddDays(5), EndDate=nodes[3].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[4].Children = new List<Task>
            {
                new Task{ TaskName="Sub Task 1", StartDate= nodes[4].StartDate, EndDate=nodes[4].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new Task{ TaskName="Sub Task 2", StartDate= nodes[4].StartDate.AddDays(2), EndDate=nodes[4].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new Task{ TaskName="Sub Task 3", StartDate= nodes[4].StartDate.AddDays(3), EndDate=nodes[4].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new Task{ TaskName="Sub Task 4", StartDate= nodes[4].StartDate.AddDays(5), EndDate=nodes[4].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };


			nodes[5].Children = new List<Task>
            {
                new Task{ TaskName="Sub Task 1", StartDate= nodes[5].StartDate, EndDate=nodes[5].StartDate.AddDays(2), Resources="dave", PercentComplete=25d },
                new Task{ TaskName="Sub Task 2", StartDate= nodes[5].StartDate.AddDays(2), EndDate=nodes[5].StartDate.AddDays(3), Resources="parker", PercentComplete=50d },
                new Task{ TaskName="Sub Task 3", StartDate= nodes[5].StartDate.AddDays(3), EndDate=nodes[5].StartDate.AddDays(5), Resources="dylan", PercentComplete=75d },
                new Task{ TaskName="Sub Task 4", StartDate= nodes[5].StartDate.AddDays(5), EndDate=nodes[5].StartDate.AddDays(1), Resources="missy", PercentComplete=100d }
            };

			return nodes;
		}

		// Add more operations here and mark them with [OperationContract]
	}
}
