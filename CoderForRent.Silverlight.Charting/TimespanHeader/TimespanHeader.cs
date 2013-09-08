/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoderForRent.Charting.Core;

namespace CoderForRent.Charting.TimespanHeader
{
    [TemplatePart(Name = "RowsPresenter", Type = typeof(TimespanHeaderRowsPresenter))]
    public class TimespanHeader : ContentControl, IMouseWheelObserver
    {
        public static DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(TimespanHeader), new PropertyMetadata(DateTime.Now));
        public static DependencyProperty ZoomFactorProperty = DependencyProperty.Register("ZoomFactor", typeof(double), typeof(TimespanHeader), new PropertyMetadata(1d));

        public event EventHandler CurrentTimeChanged;

        protected void RaiseCurrentTimeChanged(EventArgs e)
        {
            if (CurrentTimeChanged != null)
                CurrentTimeChanged(this, e);
        }

        public event EventHandler ZoomFactorChanged;
        private void RaiseZoomFactorChange(EventArgs e)
        {
            if (ZoomFactorChanged != null)
                ZoomFactorChanged(this, e);
        }
		public TimeUnits LowerUnit
		{
			get
			{
				if (this.RowsPresenter.Children.Count > 0)
					return (this.RowsPresenter.Children[this.RowsPresenter.Children.Count - 1] as TimespanHeaderRow).TimeUnit;
				else
					return TimeUnits.Days;
			}
		}
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set
            {
                if (value != CurrentTime)
                {
                    SetValue(CurrentTimeProperty, value);
                    UpdateRowTimes();
                    InvalidateRowPresenters();
                    RaiseCurrentTimeChanged(EventArgs.Empty);
                }
            }
        }


        public double ZoomFactor
        {
            get
            {
                return Zoom.Value;
            }
            set
            {
                if (value != ZoomFactor)
                {
                    Zoom.Value = value;
                    InvalidateRowPresenters();
                    RaiseZoomFactorChange(EventArgs.Empty);
                }
            }
        }




		internal TimespanHeaderRowsPresenter RowsPresenter { get; set; }


        public TimespanHeader()
        {
            this.DefaultStyleKey = typeof(TimespanHeader);

            this.SizeChanged += this_SizeChanged;
            this.MouseLeftButtonDown += this_MouseLeftButtonDown;
            this.MouseLeftButtonUp += this_MouseLeftButtonUp;
            this.MouseMove += this_MouseMove;
            this.MouseLeave += this_MouseLeave;

#if SILVERLIGHT
            if (!UIHelpers.IsInDesignModeStatic)
                MouseWheelListener.Instance.AddObserver(this);
#endif
        }

        #region Drag and Drop
        internal bool IsMouseDown { get; private set; }
        private Point _LastMouseDownPosition = new Point(0, 0);
        private double _MinimumDragDistance = 15.0d;

        protected void this_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = true;
            _LastMouseDownPosition = e.GetPosition(this);
        }
        protected void this_MouseLeave(object sender, MouseEventArgs e)
        {

            Cursor = Cursors.Arrow;
            IsMouseDown = false;

        }
        protected void this_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                Cursor = Cursors.Hand;

                Point p = e.GetPosition(this);
                double dist = (_LastMouseDownPosition.X - p.X);
                if (_MinimumDragDistance <= Math.Abs(dist))
                {
                    _LastMouseDownPosition = p;

                    TimeUnits tu = (RowsPresenter.Children[RowsPresenter.Children.Count - 1] as TimespanHeaderRow).TimeUnit;
                    if (tu == TimeUnits.Hours)
                    {
                        CurrentTime = CurrentTime.AddType(TimeUnits.Hours, dist / GetWidth(CurrentTime, TimeUnits.Hours));
                    }
                    else
                        CurrentTime = CurrentTime.AddType(TimeUnits.Days, ConvertDistanceToDays(dist));
                }
            }
        }

        private double ConvertDistanceToDays(double dist)
        {
            double unitWidth = GetWidth(CurrentTime, TimeUnits.Days);
            return (dist / unitWidth);
        }
        protected void this_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = false;
            Cursor = Cursors.Arrow;
        }


        #endregion

        protected void this_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateRowPresenters();
        }

        private void InvalidateRowPresenters()
        {
            this.RowsPresenter.InvalidateCells();
        }
        private void UpdateRowTimes()
        {
            this.RowsPresenter.UpdateCurrentTime(CurrentTime);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RowsPresenter = (TimespanHeaderRowsPresenter)GetTemplateChild("RowsPresenter");
            this.RowsPresenter
                .Children
                .OfType<TimespanHeaderRow>()
                .ToList()
                .ForEach(r => r.ParentTimespanHeader = this);
        }

        internal double GetTotalUnits()
        {
            return this.DesiredSize.Width;
        }
        internal double GetWidth(DateTime time, TimeUnits timeUnit)
        {

			return TimeUnitScalar.GetWidth(CurrentTime, time, timeUnit);

          
        }

        #region IMouseWheelObserver Members

        public void OnMouseWheel(MouseWheelArgs args)
        {
            double result = ZoomFactor;
            result += args.Delta * 0.2;
            if (result > 0.2 && result < 2.0)
                ZoomFactor = result;
			//else if (result > 2.0)
			//{
			//    ZoomFactor = 1;
			//    foreach (TimespanHeaderRow row in _RowsPresenter.Children)
			//        row.DecreaseScope();

			//}
			//else if (result < 0.2)
			//{
			//    ZoomFactor = 1;
			//    foreach (TimespanHeaderRow row in _RowsPresenter.Children)
			//        row.IncreaseScope();
			//}

        }

        #endregion

        internal double GetPosition(DateTime d)
        {
			return TimeUnitScalar.GetPosition(CurrentTime, d);
        }
        internal TimeSpan GetTimespan(double distance)
        {
            return TimeUnitScalar.GetTimespan(CurrentTime, distance);

        }
    }
}
