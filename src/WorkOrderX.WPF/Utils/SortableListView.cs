using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WorkOrderX.WPF.Utils
{
	// Реализация отсюда: http://thejoyofcode.com/sortable_listview_in_wpf.aspx
	public partial class SortableListView : ListView
	{
		private GridViewColumnHeader _lastHeaderClicked = null;

		private ListSortDirection _lastDirection = ListSortDirection.Ascending;

		public static readonly DependencyProperty SortPropertyNameProperty =
			DependencyProperty.RegisterAttached("SortPropertyName", typeof(string), typeof(SortableListView));

		public static string GetSortPropertyName(GridViewColumn obj) => (string)obj.GetValue(SortPropertyNameProperty);

		public static void SetSortPropertyName(GridViewColumn obj, string value) => obj.SetValue(SortPropertyNameProperty, value);

		public SortableListView() => AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));

		private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
			ListSortDirection direction;
			if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
			{
				if (headerClicked != _lastHeaderClicked) direction = ListSortDirection.Ascending;
				else
				{
					if (_lastDirection == ListSortDirection.Ascending) direction = ListSortDirection.Descending;
					else direction = ListSortDirection.Ascending;
				}
				string sortBy = GetSortPropertyName(headerClicked.Column);
				if (string.IsNullOrEmpty(sortBy)) sortBy = headerClicked.Column.Header as string;
				Sort(sortBy, direction);
				_lastHeaderClicked = headerClicked;
				_lastDirection = direction;
			}
		}

		private void Sort(string sortBy, ListSortDirection direction)
		{
			ICollectionView dataView = CollectionViewSource.GetDefaultView(ItemsSource);
			if (dataView != null)
			{
				dataView.SortDescriptions.Clear();
				SortDescription sd = new SortDescription(sortBy, direction);
				dataView.SortDescriptions.Add(sd);
				dataView.Refresh();
			}
		}
	}
}