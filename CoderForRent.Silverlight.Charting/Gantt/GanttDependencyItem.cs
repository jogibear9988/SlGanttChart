using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CoderForRent.Charting.Core;
using CoderForRent.Charting.TimespanHeader;

namespace CoderForRent.Charting.Gantt
{
	public class GanttDependencyItem : Control
	{
		public static DependencyProperty LineWidthProperty = DependencyProperty.Register("LineWidth", typeof(double), typeof(GanttDependencyItem), new PropertyMetadata(1d));
		public double LineWidth
		{
			get { return (double)GetValue(LineWidthProperty); }
			set { SetValue(LineWidthProperty, value); }
		}


		private GanttDependency _Dependency;
		public GanttDependency Dependency
		{
			get
			{
				return _Dependency;
			}
			set
			{
				_Dependency = value;

				if (value != null)
				{
					value.TypeChanged += new EventHandler(Dependency_TypeChanged);
				
				}
			}
		}

		public GanttDependenciesPresenter ParentPresenter { get; set; }



		protected internal Canvas LineCanvas { get; private set; }

		public GanttDependencyItem()
		{
			this.DefaultStyleKey = typeof(GanttDependencyItem);
			this.Loaded += new RoutedEventHandler(GanttDependencyItem_Loaded);

		}
		
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			LineCanvas = (Canvas)GetTemplateChild("LineCanvas");

			UpdateDependencyLines();
		}
	
		private void Dependency_TypeChanged(object sender, EventArgs e)
		{
			
		}
		private void GanttDependencyItem_Loaded(object sender, RoutedEventArgs e)
		{
			
		}
		internal void UpdateDependencyLines()
		{
			if ( Dependency == null || LineCanvas == null )
				return;

			switch (Dependency.Type)
			{
				default:
				case DependencyType.ChildBeginsAtParentEnd:

					DrawChildBeginsAtParentEndLines();
					break;
			}


		}

		private void DrawChildBeginsAtParentEndLines()
		{
			bool isEquivolent = TimeUnitScalar.IsEquivolent(Dependency.ChildNode.StartDate, Dependency.ParentNode.EndDate, ParentPresenter.ParentPanel.ParentGanttChart.TimespanHeader.RowsPresenter.Children.Cast<TimespanHeaderRow>().Last().TimeUnit);
			bool inverted = isEquivolent || (Dependency.ChildNode.StartDate <= Dependency.ParentNode.EndDate);

			int parentIndex = ParentPresenter.ParentPanel.Nodes.IndexOf(Dependency.ParentNode);
			int childIndex = ParentPresenter.ParentPanel.Nodes.IndexOf(Dependency.ChildNode);

			bool parentAboveChild = parentIndex < childIndex;


			double startX = TimeUnitScalar.GetPosition(ParentPresenter.ParentPanel.CurrentTime, Dependency.ParentNode.EndDate);
			double startY = (parentIndex * ParentPresenter.ParentPanel.RowHeight) + (ParentPresenter.ParentPanel.RowHeight / 2d) - (ParentPresenter.ParentPanel.RowHeight * ParentPresenter.ParentPanel.TopNodeIndex);

			double endX = TimeUnitScalar.GetPosition(ParentPresenter.ParentPanel.CurrentTime, Dependency.ChildNode.StartDate);
			double endY = (childIndex * ParentPresenter.ParentPanel.RowHeight) + (ParentPresenter.ParentPanel.RowHeight / 2d) - (ParentPresenter.ParentPanel.RowHeight * ParentPresenter.ParentPanel.TopNodeIndex);

			LineCanvas.Children.Clear();

			double pip = TimeUnitScalar.ConvertToPixels(ParentPresenter.ParentPanel.CurrentTime, ParentPresenter.ParentPanel.ParentGanttChart.TimespanHeader.LowerUnit) / 2d;
			int direction = 1;

			if (inverted)
			{
				if (parentAboveChild)
					direction = -1;

					Line l = new Line();
					l.Stroke = new SolidColorBrush(Colors.Black);
					l.StrokeThickness = this.LineWidth;
					l.X1 = startX;
					l.Y1 = startY;
					l.X2 = startX + (pip*direction);
					l.Y2 = startY;
					LineCanvas.Children.Add(l);

					Line l2 = new Line();
					l2.Stroke = new SolidColorBrush(Colors.Black);
					l2.StrokeThickness = this.LineWidth;
					l2.X1 = l.X2;
					l2.Y1 = l.Y2;
					l2.X2 = l.X2;
					l2.Y2 = ((endY + startY) / 2d);
					LineCanvas.Children.Add(l2);

					Line l3 = new Line();
					l3.Stroke = new SolidColorBrush(Colors.Black);
					l3.StrokeThickness = this.LineWidth;
					l3.X1 = l2.X2;
					l3.Y1 = l2.Y2;
					l3.X2 = endX - (pip * direction);
					l3.Y2 = l2.Y2;
					LineCanvas.Children.Add(l3);

					Line l4 = new Line();
					l4.Stroke = new SolidColorBrush(Colors.Black);
					l4.StrokeThickness = this.LineWidth;
					l4.X1 = l3.X2;
					l4.Y1 = l3.Y2;
					l4.X2 = l3.X2;
					l4.Y2 = endY;
					LineCanvas.Children.Add(l4);

					Line l5 = new Line();
					l5.Stroke = new SolidColorBrush(Colors.Black);
					l5.StrokeThickness = this.LineWidth;
					l5.X1 = l4.X2;
					l5.Y1 = l4.Y2;
					l5.X2 = endX;
					l5.Y2 = endY;
					LineCanvas.Children.Add(l5);
					
				
			}
			else
			{	Line l = new Line();
				l.Stroke = new SolidColorBrush(Colors.Black);
				l.StrokeThickness = this.LineWidth;
				l.X1 = startX;
				l.Y1 = startY;
				l.X2 = (endX + startX) / 2d;
				l.Y2 = startY;
				LineCanvas.Children.Add(l);

				Line l2 = new Line();
				l2.Stroke = new SolidColorBrush(Colors.Black);
				l2.StrokeThickness = this.LineWidth;
				l2.X1 = l.X2;
				l2.Y1 = l.Y2;
				l2.X2 = l.X2;
				l2.Y2 = endY;
				LineCanvas.Children.Add(l2);

				Line l3 = new Line();
				l3.Stroke = new SolidColorBrush(Colors.Black);
				l3.StrokeThickness = this.LineWidth;
				l3.X1 = l2.X2;
				l3.Y1 = l2.Y2;
				l3.X2 = endX;
				l3.Y2 = endY;
				LineCanvas.Children.Add(l3);

			}
		}
		

		//private void UpdateDependencyLines()
		//{
		//    if (Dependency == null || TopLeft == null)
		//        return;

		//    bool ParentAboveChild = true;

		//    if(ParentAboveChild)
		//    {
		//        switch (Dependency.Type)
		//        {
		//            default:
		//            case DependencyType.ChildBeginsAtParentEnd:

		//                if (Dependency.ParentNode.EndDate > Dependency.ChildNode.StartDate)
		//                {
		//                    TopLeft.Left = TopLeft.Right = true;
		//                    TopCenter.Left = TopCenter.Down = true;
		//                    MiddleCenter.Up = MiddleCenter.Down = true;
		//                    BottomCenter.Up = BottomCenter.Right = true;
		//                    BottomRight.Left = BottomRight.Right = true;

		//                    TopRight.Visibility =
		//                        MiddleLeft.Visibility =
		//                        MiddleRight.Visibility =
		//                        BottomLeft.Visibility = Visibility.Collapsed;

		//                    TopLeft.Visibility =
		//                        TopCenter.Visibility =
		//                        MiddleCenter.Visibility =
		//                        BottomCenter.Visibility =
		//                        BottomRight.Visibility = Visibility.Visible;
		//                }
		//                break;
		//        }

		//    }
		//}
		
	}
}
