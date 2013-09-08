using System;

namespace CoderForRent.Charting.Gantt
{
    public class GanttNodeSectionEventArgs : EventArgs
    {
        public GanttNodeSection Section;

        public GanttNodeSectionEventArgs(GanttNodeSection section)
        {
            Section = section;
        }
    }
}
