/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System;
using System.Windows.Controls.Primitives;

namespace CoderForRent.Charting.Core
{
    public class TimeUnitScalar
    {
        private static double _scaleFactor = 80;
        public static double ScaleFactor
        {
            get { return _scaleFactor; }
            set { _scaleFactor = value; }
        }

        public static double ConvertToPixels(DateTime CurrentTime, TimeUnits timeUnit)
        {
            


            double TickWidth = 2.38E-11;
            double MinuteWidth = ((TickWidth*8.64E11/24d)/60) * ScaleFactor;

            double HourWidth = MinuteWidth * 60;
            double DayWidth = HourWidth * 24;
           

            switch (timeUnit)
            {
                case TimeUnits.Years:
                    double DaysInYear = 365d;
                    if (DateTime.IsLeapYear(CurrentTime.Year))
                        DaysInYear = 366;
                    double YearWidth = DayWidth * DaysInYear;

                    return YearWidth * Zoom.Value;
                case TimeUnits.Months:
                    double MonthWidth = DayWidth * DateTime.DaysInMonth(CurrentTime.Year, CurrentTime.Month);

                    return MonthWidth * Zoom.Value;
                case TimeUnits.Weeks:
                    double WeekWidth = DayWidth * 7;
                    return WeekWidth * Zoom.Value;
                case TimeUnits.Days:
                    return DayWidth * Zoom.Value;
                case TimeUnits.Hours:
                    return HourWidth * Zoom.Value;
                case TimeUnits.Minutes:
                    return MinuteWidth * Zoom.Value;   
                default:
                    return TickWidth *  Zoom.Value;
    
            }
        }
		public static double GetWidth(DateTime CurrentTime, DateTime time, TimeUnits timeUnit)
		{

			double result = TimeUnitScalar.ConvertToPixels(time, timeUnit);

			if (timeUnit == TimeUnits.Years)
			{
                if (CurrentTime.Year == time.Year)
                {
                    double DaysInYear = 365d;
                    if (DateTime.IsLeapYear(time.Year))
                        DaysInYear = 366;
                    result *= (double)(DaysInYear - time.DayOfYear + 1) / DaysInYear;
                }
			}
			else if (timeUnit == TimeUnits.Months)
			{
				if (CurrentTime.Month == time.Month)
					result *= (double)(DateTime.DaysInMonth(time.Year, time.Month) - time.Day + 1) / (double)DateTime.DaysInMonth(time.Year, time.Month);

			}
            else if (timeUnit == TimeUnits.Weeks)
            {
                int daysInWeek = 7;

                int weekCurrent = GetWeekOfYear(CurrentTime);
                int weekTime = GetWeekOfYear(time);


                if (weekTime == 52)
                {
                    daysInWeek = 7;

                    if (DateTime.IsLeapYear(time.Year))
                        daysInWeek++;

                    result *= (double)daysInWeek / 7d;
                }

                if (weekTime == 53)
                {
                    result = 0;

                }
                else if (weekCurrent == weekTime)
                {
                    daysInWeek = 7 - (CurrentTime.DayOfYear - (7 * (weekCurrent-1))) +2 ;
                    
                    result *= (double)daysInWeek  / 7d;
                }
            }
          

			return result;
		}
        public static int GetWeekOfYear(DateTime dateTime)
        {
            return ((int)Math.Ceiling((double)dateTime.DayOfYear / 7d) );
        }
        public static TimeSpan GetTimespan(DateTime CurrentTime, double distance)
        {
            if (distance == 0)
                return TimeSpan.Zero;

            double UnitWidth = ConvertToPixels(CurrentTime, TimeUnits.Ticks);

            long units = (long)Math.Round(distance / UnitWidth);
            TimeSpan result = new TimeSpan( units );
         
            return result;


        }
		public static double GetPosition(DateTime CurrentTime, DateTime TargetTime)
		{
			double result = 0d;

			TimeSpan ts = TargetTime.Date - CurrentTime.Date;

			double UnitWidth = TimeUnitScalar.ConvertToPixels(TargetTime, TimeUnits.Ticks);
			result = (UnitWidth * ts.Ticks);

			return result;
		}
		public static bool IsEquivolent(DateTime A, DateTime B, TimeUnits Scale)
		{
			TimeSpan diff = (A - B);

			switch (Scale)
			{
				case TimeUnits.Weeks:
					return Math.Abs(diff.TotalDays) < 7;

				case TimeUnits.Months:
					return Math.Abs(diff.TotalDays) < 30;

				case TimeUnits.Hours:
					return Math.Abs(diff.TotalHours) < 1;

                case TimeUnits.Minutes:
                    return Math.Abs(diff.TotalMinutes) < 1;

				default:
				case TimeUnits.Days:
					diff = A.Date - B.Date;
					return Math.Abs(Math.Round(diff.TotalDays)) < 1;
			}

		}
	}
}
