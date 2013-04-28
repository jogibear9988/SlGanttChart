using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace CoderForRent.Silverlight.Charting.Gantt
{
    public class GanttNodeSection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event EventHandler<PropertyChangedEventArgs> PropertyChanging;
        protected virtual void RaisePropertyChanging(string prop)
        {
            RaisePropertyChanging(new PropertyChangedEventArgs(prop));
        }
        protected virtual void RaisePropertyChanging(PropertyChangedEventArgs e)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, e);
        }



        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                if (_StartDate != value)
                {

                    RaisePropertyChanging("StartDate");
                    _StartDate = value;
                    RaisePropertyChanged("StartDate");
                }
            }
        }


        private DateTime _EndDate;
        public DateTime EndDate {
            get { return _EndDate; }
            set
            {
                if (_EndDate != value)
                {
                    RaisePropertyChanging("EndDate");

                    _EndDate = value;
                    RaisePropertyChanged("EndDate");
                }

            }
        }

        private double _PercentComplete;
        public double PercentComplete
        {
            get { return _PercentComplete; }
            set
            {
                if (_PercentComplete != value)
                {
                    RaisePropertyChanging("PercentComplete");
                    _PercentComplete = value;
                    RaisePropertyChanged("PercentComplete");
                }
            }
        }

        //TODO: Find a way to implement this
        public Brush BackgroundBrush { get; set; }



    }
}
