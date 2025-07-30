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
}
