namespace WorkOrderX.WPF.Models.Model.Global
{
	/// <summary>
	/// Сохранение последних данных об фильтрации, в <see cref="SortableListView"/>
	/// </summary>
	public static class GlobalSettingForApp
	{
		/// <summary>
		/// Последний заголовок колонки при сортировки
		/// </summary>
		public static object? LastHeaderClicked { get; set; }

		/// <summary>
		/// Последний метод сортировки
		/// </summary>
		public static object? LastDirection { get; set; }

		/// <summary>
		/// Последняя сортировка по столбцу
		/// </summary>
		public static object? LastSortPropertyName { get; set; }
	}
}
