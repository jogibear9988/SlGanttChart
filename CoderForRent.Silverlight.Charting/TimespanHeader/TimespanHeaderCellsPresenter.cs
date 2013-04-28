/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoderForRent.Silverlight.Charting.TimespanHeader
{
    public class TimespanHeaderCellsPresenter : Panel
    {


        internal TimespanHeaderRow ParentRow { get; set; }

        public TimespanHeaderCellsPresenter()
        {
            UseLayoutRounding = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if(ParentRow == null)
                return base.ArrangeOverride(finalSize);

            double totalWidth = 0d;

            Children.OfType<TimespanHeaderCell>().ToList().ForEach(cell =>
            {
                double width = cell.DesiredSize.Width;
                double x = totalWidth + ParentRow.CellBorderThickness.Left + ParentRow.CellBorderThickness.Right;

                if (x + width > finalSize.Width)
                {
                    width -= (x + width) - finalSize.Width;

                }
				if (width < 0)
					width = 0;

                cell.Arrange(new Rect(x, 0, width, finalSize.Height));
                totalWidth += width;
            }

            );

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size(double.PositiveInfinity, availableSize.Height);
            Children.OfType<TimespanHeaderCell>().ToList().ForEach(cell =>
            {
                cell.Measure(size);
            });

            return base.MeasureOverride(availableSize);
        }
    }
}
