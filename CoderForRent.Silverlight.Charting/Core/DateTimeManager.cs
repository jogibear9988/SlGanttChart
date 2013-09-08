/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System;

namespace CoderForRent.Charting.Core
{
    public static class DateTimeManager
    {
        public static DateTime AddType(this DateTime instance, TimeUnits type, double increment)
        {
            DateTime result = instance;

            switch (type)
            {
                case TimeUnits.Days:
                    result = instance.AddDays(increment);
                    break;
                case TimeUnits.Months:
                    increment *= DateTime.DaysInMonth(instance.Year, instance.Month);
                    result = instance.AddDays((int)(Math.Floor(increment)));
                    break;
                case TimeUnits.Weeks:

                    result = instance.AddDays(increment * 7);
                    break;
                case TimeUnits.Years:
                    if (DateTime.IsLeapYear(instance.Year))
                        increment *= 366;
                    else
                        increment *= 365;

                    result = instance.AddDays((int)(Math.Floor(increment)));
                    break;
                case TimeUnits.Hours:
                    result = instance.AddHours(increment);
                    break;
            }

            return result;
        }
    }
}
