/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoderForRent.Silverlight.Charting.Core;

namespace CoderForRent.Silverlight.Charting.TimespanHeader
{
    public class TimespanHeaderRowsPresenter : Panel
    {
		
        protected override Size ArrangeOverride(Size finalSize)
        {
            int visibleCount = 0;
            visibleCount = base.Children.Where<UIElement>(ui => ui.Visibility == Visibility.Visible).Count();

            double y = 0.0;
            double height = finalSize.Height / ((double)visibleCount);
            foreach (TimespanHeaderRow row2 in base.Children)
            {
                if (row2.Visibility == Visibility.Visible)
                {
                    if (row2.Height > height)
                        height = row2.Height;

                    row2.Arrange(new Rect(0.0, y, finalSize.Width, height));
                    y += height;
                }
                else
                {
                    row2.Arrange(new Rect(0.0, 0.0, finalSize.Width, height));
                }
            }
            return finalSize;

        }
        protected override Size MeasureOverride(Size availableSize)
        {
            double totalHeight;
            int visibleCount = 0;
            bool isPositiveInfinity = false;

            visibleCount = base.Children.Where<UIElement>(ui => ui.Visibility == Visibility.Visible).Count();

            if (double.IsPositiveInfinity(availableSize.Height))
            {
                totalHeight = double.PositiveInfinity;
                isPositiveInfinity = true;
            }
            else
                totalHeight = availableSize.Height / ((double)visibleCount);

            double height = 0d;
            double width = 0d;
            foreach (TimespanHeaderRow row2 in base.Children)
            {
                if (row2.Height > totalHeight)
                    totalHeight = row2.Height;

                row2.Measure(new Size(availableSize.Width, totalHeight));
                if (row2.Visibility == Visibility.Visible)
                {
                    if (isPositiveInfinity)
                        height += row2.DesiredSize.Height;
                    else
                        height += totalHeight;
                }
                width = Math.Max(width, row2.DesiredSize.Width);
            }

            if(!double.IsPositiveInfinity(availableSize.Width))
                width = availableSize.Width;

            return new Size(width, height);

        }
        public void InvalidateCells()
        {
            this.Children.OfType<TimespanHeaderRow>().ToList<TimespanHeaderRow>().ForEach(tshr =>{
                tshr.CellsValid = false;
                tshr.InvalidateArrange();
                tshr.InvalidateMeasure();
                
            });
        }

        internal void UpdateCurrentTime(DateTime CurrentTime)
        {
            this.Children.OfType<TimespanHeaderRow>().ToList().ForEach(r => r.CurrentTime = CurrentTime);

        }
    }
}
