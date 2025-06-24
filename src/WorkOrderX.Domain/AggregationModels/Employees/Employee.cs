using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	/// <summary>
	/// Сотрудник
	/// </summary>
	public class Employee : Entity
	{
		/// <summary>
		/// Учетная запись сотрудника/рабочего места
		/// </summary>
		public Account Account { get; set; }

		/// <summary>
		/// Роль сотрудника
		/// </summary>
		public Role Role { get; set; }

		/// <summary>
		/// Тут будет "Мастер станочного участка"/"Мастер 049 уч."
		/// </summary>
		public Name Name { get; set; }

		/// <summary>
		/// Участок на котором сотрудник работает
		/// </summary>
		public Department Department { get; set; }

		/// <summary>
		/// Почта сотрудника/рабочего места
		/// </summary>
		public Email Email { get; set; }

		/// <summary>
		/// Номер телефона сотрудника/рабочего места
		/// </summary>
		public Phone Phone { get; set; }
	}
}
