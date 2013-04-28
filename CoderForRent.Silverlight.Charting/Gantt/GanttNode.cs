/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Collections.Specialized;

namespace CoderForRent.Silverlight.Charting.Gantt
{
    /// <summary>
    /// A base class node that meats the requirements for the IGanttNode.
    /// Note that the GanttChart only requires a class that implements IGanttNode,
    /// so this class is not strictly required.
    /// </summary>
    public class GanttNode : IGanttNode
    {
        

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



        private string _TaskName;
        [GanttColumnAttribute(ColumnName = "Task Name", ColumnIndex = 0)]
        public virtual string TaskName
        {
            get { return _TaskName; }
            set
            {
                if (value != _TaskName)
                {
                    RaisePropertyChanging(new PropertyChangedEventArgs("TaskName"));

                    _TaskName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("TaskName"));
                }
            }
        }

        [GanttColumnAttribute(ColumnName = "Duration", ColumnIndex = 1)]
        public virtual string Duration
        {
            get
            {
                TimeSpan ts = EndDate - StartDate;

                return ts.Days.ToString() + "D " + ts.Hours.ToString() + "H " + ((ts.Minutes > 0) ? ts.Minutes + "M" : string.Empty);
            }
        }

        private double _PercentComplete;
        [GanttColumnAttribute(ColumnName = "% Complete", ColumnIndex = 2)]
        public virtual double PercentComplete
        {
            get { AddFirstSectionIfNecessary(); return Sections.Average(s => s.PercentComplete); }
            set
            {
                AddFirstSectionIfNecessary();
                if (value != _PercentComplete)
                {
                    RaisePropertyChanging(new PropertyChangedEventArgs("PercentComplete"));
                    Sections.First().PercentComplete = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("PercentComplete"));
                }
            }
        }

        [GanttColumnAttribute(ColumnName = "Start Date", ColumnIndex = 3, ColumnType = GanttColumnType.DateTime)]
        public virtual DateTime StartDate
        {
            get { AddFirstSectionIfNecessary(); return Sections.First().StartDate; }
            set
            {
                AddFirstSectionIfNecessary();
                if (value != Sections.Min(s => s.StartDate))
                {
                    RaisePropertyChanging(new PropertyChangedEventArgs("StartDate"));
                    RaisePropertyChanging(new PropertyChangedEventArgs("Duration"));
                    Sections.First().StartDate = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("StartDate"));
                    RaisePropertyChanged(new PropertyChangedEventArgs("Duration"));

                }
            }
        }


        [GanttColumnAttribute(ColumnName = "End Date", ColumnIndex = 4, ColumnType = GanttColumnType.DateTime)]
        public virtual DateTime EndDate
        {
            get { AddFirstSectionIfNecessary(); return Sections.Last().EndDate; }
            set
            {
                AddFirstSectionIfNecessary();
                if (value != Sections.Max(s => s.EndDate))
                {
                    RaisePropertyChanging(new PropertyChangedEventArgs("EndDate"));
                    RaisePropertyChanging(new PropertyChangedEventArgs("Duration"));
                    Sections.Last().EndDate = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("EndDate"));
                    RaisePropertyChanged(new PropertyChangedEventArgs("Duration"));
                }
            }
        }

        private ObservableCollection<GanttNodeSection> _Sections;
        public ObservableCollection<GanttNodeSection> Sections
        {
            get
            {
                if (_Sections == null)
                {
                    _Sections = new ObservableCollection<GanttNodeSection>();
                    _Sections.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_Sections_CollectionChanged);
                }

                return _Sections;
            }
            set
            {
                RaisePropertyChanging(new PropertyChangedEventArgs("Sections"));
                _Sections = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("Sections"));
            }
        }

        void _Sections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (GanttNodeSection section in e.NewItems)
                    section.PropertyChanged += new PropertyChangedEventHandler(section_PropertyChanged);
            }
        }

        void section_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GanttNodeSection section = sender as GanttNodeSection;

            if ((e.PropertyName == "StartDate" && section.StartDate <= StartDate)
                || (e.PropertyName == "EndDate" && section.EndDate >= EndDate)
               )
                this.RaisePropertyChanged(e);


        }


        private bool _ShowGap = false;
        public bool ShowGap
        {
            get
            {
                return _ShowGap && Sections.Count == 0;
            }
            set
            {
                if (value != _ShowGap)
                {
                    RaisePropertyChanging(new PropertyChangedEventArgs("ShowGap"));
                    _ShowGap = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("ShowGap"));
                }
            }
        }

        private string _Resources;
        [GanttColumnAttribute(ColumnName = "Resources", ColumnIndex = 5)]
        public virtual string Resources
        {

            get { return _Resources; }
            set
            {
                if (value != _Resources)
                {
                    RaisePropertyChanging(new PropertyChangedEventArgs("Resources"));
                    _Resources = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Resources"));
                }
            }
        }

        private bool _Expanded;
        public bool Expanded
        {
            get { return _Expanded; }
            set
            {
                if (value != _Expanded)
                {
                    RaisePropertyChanging(new PropertyChangedEventArgs("Expanded"));
                    _Expanded = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Expanded"));
                }
            }
        }

        public IGanttNode ParentNode { get; set; }
        public int Level
        {
            get
            {
                int result = 1;
                IGanttNode parent = this.ParentNode;
                while (parent != null)
                {
                    result++;
                    parent = parent.ParentNode;
                }

                return result;

            }
        }

        ObservableCollection<IGanttNode> _ChildNodes;
        public System.Collections.ObjectModel.ObservableCollection<IGanttNode> ChildNodes
        {
            get
            {
                if (_ChildNodes == null)
                {
                    _ChildNodes = new ObservableCollection<IGanttNode>();
                    _ChildNodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_ChildNodes_CollectionChanged);
                }

                return _ChildNodes;
            }
            set
            {
                RaisePropertyChanging(new PropertyChangedEventArgs("ChildNodes"));
                _ChildNodes = value;
                _ChildNodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_ChildNodes_CollectionChanged);

                if (value.Count > 0)
                {
                    _ChildNodes.ToList().ForEach(n =>
                    {
                        n.PropertyChanged += new PropertyChangedEventHandler(ChildNode_PropertyChanged);
                        n.ParentNode = this;
                    });
                    UpdateDatesToChildren();
                }

                RaisePropertyChanged(new PropertyChangedEventArgs("ChildNodes"));

            }
        }

        void ChildNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            IGanttNode node = sender as IGanttNode;

            if (e.PropertyName == "StartDate" || e.PropertyName == "EndDate")
            {
                UpdateDatesToChildren();
            }
        }

        private void UpdateDatesToChildren()
        {
            this.StartDate = new DateTime(_ChildNodes.Min<IGanttNode>(n => n.StartDate.Ticks));
            this.EndDate = new DateTime(_ChildNodes.Max<IGanttNode>(n => n.EndDate.Ticks));
        }

        public void _ChildNodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (IGanttNode node in e.NewItems)
                {
                    node.ParentNode = this;
                    node.PropertyChanged += ChildNode_PropertyChanged;
                }


            }
            else
            {
                foreach (IGanttNode node in e.OldItems)
                {
                    node.PropertyChanged -= ChildNode_PropertyChanged;
                }
            }
            UpdateDatesToChildren();



        }

        private void AddFirstSectionIfNecessary()
        {
            if (Sections.Count == 0)
            {
                Sections.Add(new GanttNodeSection() { StartDate = DateTime.MinValue, EndDate = DateTime.MinValue });
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


    }
}
