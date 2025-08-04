using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model.Global
{
	/// <summary>
	/// Класс, представляющий глобального сотрудника для приложения.
	/// </summary>
	public partial class GlobalEmployeeForApp : ViewModelBase
	{
		/// <summary>
		/// Свойство для хранения текущего сотрудника приложения.
		/// </summary>
		[ObservableProperty]
		private Employee _employee = new();

		/// <summary>
		/// Свойство для хранения токена аутентификации.
		/// </summary>
		[ObservableProperty]
		private string _token;
	}

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
