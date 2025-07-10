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
		public static Role Customer = new(1, nameof(Customer), "Заказчик");

		/// <summary>
		/// Роль исполнителя
		/// </summary>
		public static Role Executer = new(2, nameof(Executer), "Исполнитель");

		/// <summary>
		/// Роль администратора
		/// </summary>
		public static Role Admin = new(3, nameof(Admin), "Администратор");

		/// <summary>
		/// Роль куратора
		/// </summary>
		public static Role Supervisor = new(4, nameof(Supervisor), "Куратор");

		public Role(int id, string name, string descriptions) : base(id, name)
		{
			Descriptions = descriptions;
		}

		public string Descriptions { get; }
	}
}
