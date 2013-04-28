/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace CoderForRent.Silverlight.Charting.Core
{
    public static class DependencyObjectExtender
    {
        public static List<DependencyObject> GetAllChildren(this DependencyObject instance)
        {
            return GetAllChildren(instance, true);
        }
        public static List<DependencyObject> GetAllChildren(this DependencyObject instance, bool Recursive)
        {
            int count = VisualTreeHelper.GetChildrenCount(instance);
            List<DependencyObject> result = new List<DependencyObject>(count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(instance, i);
                    result.Add(child);

                    if (Recursive)
                        result.AddRange(child.GetAllChildren(Recursive));
                }
            }

            return result;
        }
    }
}
