using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CoderForRent.Silverlight.Charting.Core;

namespace CoderForRent.Silverlight.Charting.Gantt
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
			this.Loaded += new RoutedEventHandler(GanttDependenciesPresenter_Loaded);
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
