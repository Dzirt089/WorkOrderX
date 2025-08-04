namespace WorkOrderX.WPF.Models.Model.Global
{
	public static class GlobalSettingForApp
	{
		/// <summary>
		/// Последний заголовок колонки при сортировки
		/// </summary>

		public static object? LastHeaderClicked { get; set; }

		/// <summary>
		/// Последняя сортировка
		/// </summary>
		public static object? LastDirection { get; set; }

		public static object? LastSortPropertyName { get; set; }
	}
}
