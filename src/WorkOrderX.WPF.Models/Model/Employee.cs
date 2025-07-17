using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	public partial class Employee : ViewModelBase
	{
		/// <summary>
		/// Идентификатор сотрудника
		/// </summary>
		[ObservableProperty]
		private Guid _id;

		/// <summary>
		/// Учетная запись компьютера, с которого запрашивают инфу об сотруднике, у которого имеется эта учётная запись
		/// </summary>
		[ObservableProperty]
		private string _account;

		/// <summary>
		/// Роль у сотрудника (Заказчик, исполнитель, админ, куратор)
		/// </summary>
		[ObservableProperty]
		private string _role;

		/// <summary>
		/// Наименование рабочего места сотрудника
		/// </summary>
		[ObservableProperty]
		private string _name;

		/// <summary>
		/// Участок. за которым закреплено рабочее места сотрудника
		/// </summary>
		[ObservableProperty]
		private string _department;

		/// <summary>
		/// Почта рабочего места сотрудника
		/// </summary>
		[ObservableProperty]
		private string _email;

		/// <summary>
		/// Телефон рабочего места сотрудника
		/// </summary>
		[ObservableProperty]
		private string _phone;

		/// <summary>
		/// Специальность сотрудника (для исполнителей)
		/// </summary>
		[ObservableProperty]
		private string? _specialized;
	}
}
