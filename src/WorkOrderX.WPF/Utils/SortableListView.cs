using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Global;

namespace WorkOrderX.WPF.Utils
{
	// Реализация отсюда: http://thejoyofcode.com/sortable_listview_in_wpf.aspx
	public partial class SortableListView : ListView
	{
		private GridViewColumnHeader? _lastHeaderClicked = null;

		private ListSortDirection _lastDirection = ListSortDirection.Ascending;

		public static readonly DependencyProperty SortPropertyNameProperty =
			DependencyProperty.RegisterAttached("SortPropertyName", typeof(string), typeof(SortableListView));

		public static string GetSortPropertyName(GridViewColumn obj) => (string)obj.GetValue(SortPropertyNameProperty);

		public static void SetSortPropertyName(GridViewColumn obj, string value) => obj.SetValue(SortPropertyNameProperty, value);

		public SortableListView() => AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));


		public void RestoreLastSort(IEnumerable<ActiveHistoryRequestProcess> source)
		{
			if (GlobalSettingForApp.LastHeaderClicked is GridViewColumnHeader columnHeader &&
				GlobalSettingForApp.LastSortPropertyName is string sortBy &&
				GlobalSettingForApp.LastDirection is ListSortDirection sortDirection
				)

				if (!string.IsNullOrEmpty(sortBy))
				{
					Sort(
						sortBy,
						sortDirection,
						source
					);
				}
		}

		private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
		{
			GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

			if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
			{
				ListSortDirection direction;
				if (headerClicked != _lastHeaderClicked)
				{
					direction = ListSortDirection.Ascending;
				}
				else
				{
					direction = _lastDirection == ListSortDirection.Ascending
						? ListSortDirection.Descending
						: ListSortDirection.Ascending;
				}

				string sortBy = GetSortPropertyName(headerClicked.Column);
				if (string.IsNullOrEmpty(sortBy))
					sortBy = headerClicked.Column.Header as string;

				Sort(sortBy, direction);

				_lastHeaderClicked = headerClicked;
				_lastDirection = direction;

				// Сохраняем настройки сортировки в глобальные переменные
				GlobalSettingForApp.LastHeaderClicked = _lastHeaderClicked;
				GlobalSettingForApp.LastDirection = _lastDirection;
				GlobalSettingForApp.LastSortPropertyName = sortBy;
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

		private void Sort(string sortBy, ListSortDirection direction, IEnumerable<ActiveHistoryRequestProcess> itemsSource)
		{
			ICollectionView dataView = CollectionViewSource.GetDefaultView(itemsSource);
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