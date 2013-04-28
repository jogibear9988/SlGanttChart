/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;

namespace CoderForRent.Silverlight.Charting.Gantt
{
	public class GanttDataGrid : DataGrid
	{

		#region Private variables
		private int _TopIndex = int.MinValue;
		#endregion

		#region Template Parts
		protected ScrollBar VerticalScrollbar { get; private set; }
		protected DataGridRowsPresenter RowsPresenter { get; private set; }
		#endregion

		#region Properties
		IList<IGanttNode> _Nodes;
		public IList<IGanttNode> Nodes
		{
			get { return _Nodes; }
			set
			{
				this.ItemsSource = _Nodes = value;
			}
		}
		private Type _ColumnGeneratingType = typeof(GanttNode);
		public Type ColumnGeneratingType
		{
			get { return _ColumnGeneratingType; }
			set
			{
				_ColumnGeneratingType = value; CreateTaskGridColumns();
			}
		}
		public List<DataGridRow> Rows
		{
			get { return RowsPresenter.Children.Cast<DataGridRow>().ToList(); }
		}
		private int LastExpanderClickedIndex { get; set; }
		#endregion

		#region Events
		public event EventHandler<RowExpandedChangedEventArgs> RowExpandedChanged;
		protected void RaiseRowExpandedChanged(RowExpandedChangedEventArgs e)
		{
			if (RowExpandedChanged != null)
				RowExpandedChanged(this, e);
		}
		#endregion

		#region Constructors and overrides
		public GanttDataGrid()
		{
			this.Loaded += new RoutedEventHandler(GanttDataGrid_Loaded);
			this.LoadingRow += new EventHandler<DataGridRowEventArgs>(GanttDataGrid_LoadingRow);
			this.SelectedIndex = -1;
		}
		public override void OnApplyTemplate()
		{

			base.OnApplyTemplate();

			VerticalScrollbar = (ScrollBar)GetTemplateChild("VerticalScrollbar");
			RowsPresenter = (DataGridRowsPresenter)GetTemplateChild("RowsPresenter");

		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key != Key.Down && e.Key != Key.Up)
			{
				base.OnKeyDown(e);
			}
			else
				e.Handled = true;
		}

		#endregion

		#region Event handling functions
		void GanttDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			IGanttNode node = e.Row.DataContext as IGanttNode;
			int rowIndex = e.Row.GetIndex();

			SimpleExpander ex = ((GanttExpanderColumn)Columns[0]).GetExpander(e.Row);

			if (ex.UseAnimation = LastExpanderClickedIndex == rowIndex)
				LastExpanderClickedIndex = -1;

		}
		void GanttDataGrid_RowExpandedChanged(object sender, RowExpandedChangedEventArgs e)
		{

			IGanttNode node = (e.Row.DataContext as IGanttNode);
			node.Expanded = e.IsExpanded;

			int index = LastExpanderClickedIndex = Nodes.IndexOf(node);

			if (e.IsExpanded)
			{
				index = InsertChildNodes(node, ref index);
			}
			else
			{
				RemoveChildNodes(node);
			}

			RaiseRowExpandedChanged(e);

		}

		private int InsertChildNodes(IGanttNode node, ref  int index)
		{
			foreach (IGanttNode childNode in node.ChildNodes)
			{
				Nodes.Insert(++index, childNode);

				if (childNode.Expanded && childNode.ChildNodes.Count > 0)
					InsertChildNodes(childNode, ref index);

			}
			return index;
		}

		private void RemoveChildNodes(IGanttNode node)
		{
			foreach (IGanttNode childNode in node.ChildNodes)
			{
				if (childNode.ChildNodes.Count > 0)
					RemoveChildNodes(childNode);

				Nodes.Remove(childNode);
			}
		}
		void GanttDataGrid_Loaded(object sender, RoutedEventArgs e)
		{
			HookExpanders();
		}
		#endregion

		#region Other private functions
		private void CreateTaskGridColumns()
		{
			Columns.Clear();

			Type t = ColumnGeneratingType;

			if (t == null)
				return;

			PropertyInfo[] infos = t.GetProperties();
			List<DataGridColumn> columns = new List<DataGridColumn>();

			bool isFirst = true;

			foreach (PropertyInfo info in infos)
			{
				GanttColumnAttribute[] atts = (GanttColumnAttribute[])info.GetCustomAttributes(typeof(GanttColumnAttribute), false);

				if (atts.Length > 0)
				{
					GanttColumnAttribute att = atts[atts.Length - 1];

					DataGridColumn dgc;
					Binding bind = new System.Windows.Data.Binding(info.Name);

					if (isFirst)
					{
						dgc = new GanttExpanderColumn();

						Columns.Add(dgc);
						(dgc as GanttExpanderColumn).RowExpandedChanged += new EventHandler<RowExpandedChangedEventArgs>(GanttDataGrid_RowExpandedChanged);
						(dgc as GanttExpanderColumn).Binding = bind;

						isFirst = false;
					}
					else
					{
						if (att.ColumnType == GanttColumnType.DateTime)
						{
							dgc = new GanttDateColumn();
							Columns.Add(dgc);
							bind.Mode = BindingMode.TwoWay;
							(dgc as GanttDateColumn).Binding = bind;
						}
						else
						{
							dgc = new DataGridTextColumn();
							Columns.Add(dgc);
							(dgc as DataGridTextColumn).Binding = bind;
						}
					}

					dgc.CanUserReorder = true;
					dgc.CanUserResize = true;
					dgc.CanUserSort = false;
					dgc.Header = att.ColumnName;
					dgc.IsReadOnly = !info.CanWrite;
					dgc.DisplayIndex = att.ColumnIndex;



				}
			}

			//columns.Sort((a, b) => a.DisplayIndex.CompareTo(b.DisplayIndex));
			//foreach (DataGridColumn c in columns)
			//{
			//    Columns.Add(c);
			//}


		}
		private void HookExpanders()
		{
			foreach (DataGridColumn col in Columns)
			{
				if (col is GanttExpanderColumn && (col.DisplayIndex > 0 || ColumnGeneratingType == null))
				{
					(col as GanttExpanderColumn).RowExpandedChanged += new EventHandler<RowExpandedChangedEventArgs>(GanttDataGrid_RowExpandedChanged);
				}
			}
		}
		#endregion

		#region Public functions
		public DataGridRow GetRow(object DataItem)
		{
			return Rows.FirstOrDefault(row => row.DataContext == DataItem);
		}
		public void SetTopRow(int TopIndex)
		{

			this.ItemsSource = (from IGanttNode o in Nodes select o).Skip(TopIndex).ToList();

			_TopIndex = TopIndex;

		}
		public DataGridRow GetTopRow()
		{
			if (Rows.Count > 0)
				return Rows[0];

			return null;

		}
		#endregion

	}
}
