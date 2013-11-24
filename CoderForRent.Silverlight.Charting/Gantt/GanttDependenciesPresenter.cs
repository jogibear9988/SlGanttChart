using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CoderForRent.Charting.Gantt
{
	public class GanttDependenciesPresenter : Panel
	{
		#region Properties
		protected internal GanttPanel ParentPanel { get; set; }
		protected bool ItemsValid { get; set; }
		#endregion

		#region Constructors and overrides
		public GanttDependenciesPresenter()
		{
			this.UseLayoutRounding = false;
			this.Loaded += GanttDependenciesPresenter_Loaded;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
            Debug.WriteLine("GanttDependenciesPresenter.ArrangeOverride()");
			foreach (GanttDependencyItem item in Children)
			{			
			    item.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
			}

			return base.ArrangeOverride(finalSize);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
            Debug.WriteLine("GanttDependenciesPresenter.MeasureOverride()");
			foreach (GanttDependencyItem item in Children)
			{			
				item.Measure(availableSize);
			}

			return base.MeasureOverride(availableSize);
		}
		#endregion

		#region Event handling functions
		private void GanttDependenciesPresenter_Loaded(object sender, RoutedEventArgs e)
		{
			ParentPanel.Dependencies.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Dependencies_CollectionChanged);
		}
		private void Dependencies_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Invalidate();
		}
		#endregion

		#region Internal functions
		protected internal void Invalidate()
		{
            Debug.WriteLine("GanttDependenciesPresenter.Invalidate()");
			this.Children.Clear();

			foreach (GanttDependency gd in ParentPanel.Dependencies)
			{
				if (gd.ParentNode.ParentNode.Expanded && gd.ChildNode.ParentNode.Expanded)
					this.Children.Add(new GanttDependencyItem { Dependency = gd, ParentPresenter=this });
			}
		}
		#endregion
	}
}
