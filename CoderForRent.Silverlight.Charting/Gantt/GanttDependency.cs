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
	public enum DependencyType
	{
		ChildBeginsAtParentEnd
	}
	public class GanttDependency
	{
		public event EventHandler TypeChanged;
		protected void RaiseTypeChanged(EventArgs e)
		{
			if (TypeChanged != null)
				TypeChanged(this, e);
		}
		public IGanttNode ParentNode { get; set; }
		public IGanttNode ChildNode { get; set; }

		private DependencyType _Type;
		public DependencyType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				if (_Type != value)
				{
					_Type = value;
					RaiseTypeChanged(EventArgs.Empty);
				}
			}
		}

		  
	}
}
