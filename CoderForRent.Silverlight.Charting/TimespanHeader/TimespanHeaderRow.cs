/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CoderForRent.Charting.Core;

namespace CoderForRent.Charting.TimespanHeader
{
    [TemplatePart(Name = "CellsPresenter", Type = typeof(TimespanHeaderCellsPresenter))]
    public class TimespanHeaderRow : Control
    {
        public static DependencyProperty CellHorizontalAlignmentProperty = DependencyProperty.Register("CellHorizontalAlignment", typeof(HorizontalAlignment), typeof(TimespanHeaderRow), new PropertyMetadata(HorizontalAlignment.Center));
        public static DependencyProperty CellVerticalAlignmentProperty = DependencyProperty.Register("CellVerticalAlignment", typeof(VerticalAlignment), typeof(TimespanHeaderRow), new PropertyMetadata(VerticalAlignment.Center));
        public static DependencyProperty CellBorderThicknessProperty = DependencyProperty.Register("CellBorderThickness", typeof(Thickness), typeof(TimespanHeaderRow), new PropertyMetadata(new Thickness(1)));
        public static DependencyProperty CellBorderBrushProperty = DependencyProperty.Register("CellBorderBrush", typeof(Brush), typeof(TimespanHeaderRow), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public static DependencyProperty CellBackgroundProperty = DependencyProperty.Register("CellBackground", typeof(Brush), typeof(TimespanHeaderRow), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static DependencyProperty TimeUnitProperty = DependencyProperty.Register("TimeUnit", typeof(TimeUnits), typeof(TimespanHeaderRow), new PropertyMetadata(TimeUnits.Months));
        public static DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(TimespanHeaderRow), new PropertyMetadata(DateTime.Now));
        public static DependencyProperty CellFormatProperty = DependencyProperty.Register("CellFormat", typeof(string), typeof(TimespanHeaderRow), new PropertyMetadata(null));


        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set {
				if (CurrentTime != value)
				{
					SetValue(CurrentTimeProperty, value);
					CellsValid = false;
					InvalidateMeasure();
				}
            }
        }
        public TimeUnits TimeUnit
        {
            get { return (TimeUnits)GetValue(TimeUnitProperty); }
            set { SetValue(TimeUnitProperty, value); ResetFormatting(); }
        }
        public Thickness CellBorderThickness { get { return (Thickness)this.GetValue(CellBorderThicknessProperty); } set { this.SetValue(CellBorderThicknessProperty, value); } }
        public Brush CellBorderBrush { get { return (Brush)this.GetValue(CellBorderBrushProperty); } set { this.SetValue(CellBorderBrushProperty, value); } }
        public Brush CellBackground { get { return (Brush)this.GetValue(CellBackgroundProperty); } set { this.SetValue(CellBackgroundProperty, value); } }
        public string CellFormat { get { return (string)this.GetValue(CellFormatProperty); } set { this.SetValue(CellFormatProperty, value); } }
        public HorizontalAlignment CellHorizontalAlignment { get { return (HorizontalAlignment)this.GetValue(CellHorizontalAlignmentProperty); } set { this.SetValue(CellHorizontalAlignmentProperty, value); } }
        public VerticalAlignment CellVerticalAlignment { get { return (VerticalAlignment)this.GetValue(CellVerticalAlignmentProperty); } set { this.SetValue(CellVerticalAlignmentProperty, value); } }

        public bool CellsValid { get; set; }

        public TimespanHeader ParentTimespanHeader { get; set; }

        private TimespanHeaderCellsPresenter _CellsPresenter;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _CellsPresenter = (TimespanHeaderCellsPresenter)GetTemplateChild("CellsPresenter");
            _CellsPresenter.ParentRow = this;

        }

        public TimespanHeaderRow()
        {
            this.DefaultStyleKey = this.GetType();
            this.UseLayoutRounding = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            GenerateCells();
            return base.ArrangeOverride(finalSize);
        }

        private void GenerateCells()
        {
            if (!CellsValid)
            {
                CellsValid = true;
                _CellsPresenter.Children.Clear();
                
                double location = 0d;
                double totalUnits = ParentTimespanHeader.GetTotalUnits();
                DateTime time = CurrentTime;

                while (location < totalUnits)
                {
                    TimespanHeaderCell cell = GetCell( time);

                    location +=  cell.Width ;
  
                    _CellsPresenter.Children.Add(cell);

                    //if (TimeUnit == TimeUnits.Weeks && TimeUnitScalar.GetWeekOfYear(time) == 52)
                    //{
                    //    time = time.AddDays(2);
                    //    cell = GetCell(time);
                    //    location += cell.Width;

                    //    _CellsPresenter.Children.Add(cell);
                    //}
                    //else
                    //{
                        time = time.AddType(TimeUnit, 1d);
                    //}

                }


            }
        }

        private TimespanHeaderCell GetCell(DateTime time)
        {
            TimespanHeaderCell cell = new TimespanHeaderCell();
            cell.ParentRow = this;
            cell.DateTime = time;
            cell.Width = ParentTimespanHeader.GetWidth(time, TimeUnit);
            cell.Format = CellFormat;
            cell.Background = CellBackground;
            cell.BorderBrush = CellBorderBrush;
            cell.BorderThickness = CellBorderThickness;
            cell.HorizontalContentAlignment = CellHorizontalAlignment;
            cell.VerticalContentAlignment = CellVerticalAlignment;
            return cell;
        }


        //internal void IncreaseScope()
        //{
        //    if (this.TimeUnit != TimeUnits.Years)
        //    {
               
        //        if (Visibility == Visibility.Visible)
        //            this.TimeUnit++;

        //        //this.Visibility = Visibility.Visible;
        //        ResetFormatting();
        //    }

        //}

        private void ResetFormatting()
        {
            switch (TimeUnit)
            {
                case TimeUnits.Years:
                    CellFormat = "yyyy";
                    break;
                case TimeUnits.Months:
                    if (ParentTimespanHeader != null && ParentTimespanHeader.RowsPresenter.Children.IndexOf(this) == 0)
                        CellFormat = "MMMM yyyy";
                    else
                        CellFormat = "MMM";

                    break;
                case TimeUnits.Weeks:
                    CellFormat = "WEEK";
                    break;
                case TimeUnits.Days:
                    if (ParentTimespanHeader != null && ParentTimespanHeader.RowsPresenter.Children.IndexOf(this) == 0)
                        CellFormat = "ddd MMM dd, yyyy";
                    else
                        CellFormat = "dd";
                    break;
                case TimeUnits.Hours:
                    CellFormat = "HH";
                    break;
                
            }
        }

        //internal void DecreaseScope()
        //{
        //    if (this.TimeUnit != TimeUnits.Days)
        //    {
               
        //        if(Visibility == Visibility.Visible)
        //            this.TimeUnit--;    

        //        //this.Visibility = Visibility.Visible;
        //        ResetFormatting();
        //    }

            
        //}
    }



}
