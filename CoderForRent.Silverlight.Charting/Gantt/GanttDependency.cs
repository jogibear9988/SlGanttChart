using System;

namespace CoderForRent.Charting.Gantt
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
