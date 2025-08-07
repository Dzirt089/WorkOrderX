using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	/// <summary>
	/// Роль сотрудника
	/// </summary>
	public class Role : Enumeration
	{
		/// <summary>
		/// Роль заказчика
		/// </summary>
		public readonly static Role Customer = new(1, nameof(Customer), "Заказчик");

		/// <summary>
		/// Роль исполнителя
		/// </summary>
		public readonly static Role Executer = new(2, nameof(Executer), "Исполнитель");

		/// <summary>
		/// Роль администратора
		/// </summary>
		public readonly static Role Admin = new(3, nameof(Admin), "Администратор");

		/// <summary>
		/// Роль куратора
		/// </summary>
		public readonly static Role Supervisor = new(4, nameof(Supervisor), "Куратор");

		/// <summary>
		/// Отдельная роль для менеджеров, которые могут создавать заявки на Хоз. Работы
		/// </summary>
		public readonly static Role Manager = new(5, nameof(Manager), "Менеджер");

		// Приватный конструктор без параметров для EF
		private Role() { }

		public Role(int id, string name, string descriptions) : base(id, name, descriptions) { }
	}
}
