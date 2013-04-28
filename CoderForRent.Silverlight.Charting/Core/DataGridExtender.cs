/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace CoderForRent.Silverlight.Charting.Core
{
    public static class DataGridExtender
    {
        public static DataGridRow GetDataItemRow(this DataGrid instance, object item)
        {
            return instance.GetRows().FirstOrDefault<DataGridRow>(dg => dg.DataContext == item);
        }

        public static List<DataGridRow> GetRows(this DataGrid instance)
        {          			
            Grid g = (Grid)VisualTreeHelper.GetChild(instance, 0);
			DataGridRowsPresenter rp = g.GetAllChildren().OfType<DataGridRowsPresenter>().First();
            return rp.GetAllChildren(false).Cast<DataGridRow>().ToList();
        }
    }
}
