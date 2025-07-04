namespace WorkOrderX.Application.Models.DTOs
{
	/// <summary>
	/// DTO для данных о сотруднике
	/// </summary>
	public record EmployeeDataDto
	{
		/// <summary>
		/// Идентификатор сотрудника
		/// </summary>
		public Guid Id { get; init; }

		/// <summary>
		/// Учетная запись компьютера, с которого запрашивают инфу об сотруднике, у которого имеется эта учётная запись
		/// </summary>
		public string Account { get; init; }

		/// <summary>
		/// Роль у сотрудника (Заказчик, исполнитель, админ, куратор)
		/// </summary>
		public string Role { get; init; }

		/// <summary>
		/// Наименование рабочего места сотрудника
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// Участок. за которым закреплено рабочее места сотрудника
		/// </summary>
		public string Department { get; init; }

		/// <summary>
		/// Почта рабочего места сотрудника
		/// </summary>
		public string Email { get; init; }

		/// <summary>
		/// Телефон рабочего места сотрудника
		/// </summary>
		public string Phone { get; init; }

		/// <summary>
		/// Специальность сотрудника (для исполнителей)
		/// </summary>
		public string Specialized { get; init; }
	}
}
