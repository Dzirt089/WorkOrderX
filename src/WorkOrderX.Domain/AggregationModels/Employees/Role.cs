using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	/// <summary>
	/// Роль сотрудника
	/// </summary>
	public class Role : Enumeration
	{
		/// <summary>
		/// Роль заказчика
		/// </summary>
		public static Role Customer = new(1, nameof(Customer));

		/// <summary>
		/// Роль исполнителя
		/// </summary>
		public static Role Contractor = new(2, nameof(Contractor));

		/// <summary>
		/// Роль администратора
		/// </summary>
		public static Role Admin = new(3, nameof(Admin));

		/// <summary>
		/// Роль куратора
		/// </summary>
		public static Role Supervisor = new(4, nameof(Supervisor));

		public Role(int id, string name) : base(id, name)
		{
		}

		public static Role Parse(string name) => name?.ToLower() switch
		{
			"customer" => Customer,
			"contractor" => Contractor,
			"admin" => Admin,
			"supervisor" => Supervisor,
			_ => throw new EnumerationValueNotFoundException($"Unknown role name {nameof(name)}")
		};
	}
}
