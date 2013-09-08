/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */
using System;
using System.ComponentModel;
using CoderForRent.Charting.Gantt;

namespace CoderForRent.Silverlight.GanttExample
{
    public class ExampleGanttNode : GanttNode
    {
        [GanttColumn(ColumnName="MyTask Name",ColumnIndex=0)]
        public override string TaskName
        {
            get { return base.TaskName; }
            set { base.TaskName = value; }
        }

        [GanttColumn(ColumnName = "Duration", ColumnIndex = 1)]
        public override string Duration
        {
            get
            {
                return base.Duration;
            }
        }

        [GanttColumn(ColumnName = "% Complete", ColumnIndex = 2)]
        public string PercentCompleteFormatted
        {
            get
            {
                return (PercentComplete / 100).ToString("##0.#%");

            }
            set
            {
                PercentComplete = double.Parse(value.Replace("%", ""));
            }
        }

		[GanttColumn(ColumnName = "Start Date2", ColumnIndex = 3, ColumnType = GanttColumnType.DateTime)]
        public string StartDateFormatted
        {
            get
            {
                return StartDate.ToShortDateString();
            }
            set
            {
                DateTime result;

                if (DateTime.TryParse(value, out result))
                    StartDate = result;

                RaisePropertyChanged(new PropertyChangedEventArgs("StartDateFormatted"));
            }
        }

		[GanttColumn(ColumnName = "End Date2", ColumnIndex = 4, ColumnType = GanttColumnType.DateTime)]
        public string EndDateFormatted
        {
            get
            {
                return EndDate.ToShortDateString();
            }
            set
            {
                DateTime result;

                if (DateTime.TryParse(value, out result))
                    EndDate = result;

                RaisePropertyChanged(new PropertyChangedEventArgs("EndDateFormatted"));

            }
        }



        public override double PercentComplete
        {
            get { return base.PercentComplete; }
            set { base.PercentComplete = value; }
        }
        public override DateTime StartDate
        {
            get { return base.StartDate; }
            set
            {
                base.StartDate = value; RaisePropertyChanged(new PropertyChangedEventArgs("StartDateFormatted"));
            }
        }
        public override DateTime EndDate
        {
            get { return base.EndDate; }
            set { base.EndDate = value;

            RaisePropertyChanged(new PropertyChangedEventArgs("EndDateFormatted"));
            }
        }

      

    }
}
