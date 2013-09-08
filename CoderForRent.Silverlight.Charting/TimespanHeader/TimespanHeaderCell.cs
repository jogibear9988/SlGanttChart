/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Windows;
using System.Windows.Controls;
using CoderForRent.Charting.Core;

namespace CoderForRent.Charting.TimespanHeader
{
    public class TimespanHeaderCell : Control
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TimespanHeaderCell), new PropertyMetadata("no data"));
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        internal TimespanHeaderRow ParentRow { get; set; }

        private DateTime _DateTime;
        public DateTime DateTime
        {
            get { return _DateTime; }
            set
            {
                if (_DateTime != value)
                {
                    _DateTime = value;
                    UpdateText();
                }
            }
        }

        private string _Format;
        public string Format
        {
            get { return _Format; }
            set
            {
                if (_Format != value)
                {
                    _Format = value;
                    UpdateText();
                }
            }
        }

        private void UpdateText()
        {
            if (Format == "WEEK")
                Text = "Week " + TimeUnitScalar.GetWeekOfYear(this.DateTime).ToString();
            else
                Text = this.DateTime.ToString(this.Format, System.Globalization.CultureInfo.CurrentCulture);
        }

        public TimespanHeaderCell()
        {
            this.DefaultStyleKey = this.GetType();
            this.UseLayoutRounding = false;
        }

    }
}
