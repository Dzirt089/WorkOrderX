using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	/// <summary>
	/// Сотрудник
	/// </summary>
	public class Employee : Entity, IAggregationRoot
	{
		private Employee(
			Guid id,
			Account account,
			Role role,
			Name name,
			Department department,
			Email email,
			Phone phone,
			Specialized? specialized)
		{
			Id = id;
			Account = account;
			Role = role;
			Name = name;
			Department = department;
			Email = email;
			Phone = phone;
			Specialized = specialized;
		}


		public static Employee Create(
			Account account,
			Role role,
			Name name,
			Department department,
			Email email,
			Phone phone,
			Specialized? specialized)
		{
			var newEmployee =
				new Employee(
				id: Guid.NewGuid(),
				account: account,
				role: role,
				name: name,
				department: department,
				email: email,
				phone: phone,
				specialized: specialized);

			return newEmployee;
		}

		/// <summary>
		/// Учетная запись сотрудника/рабочего места
		/// </summary>
		public Account Account { get; }

		/// <summary>
		/// Роль сотрудника
		/// </summary>
		public Role Role { get; }

		/// <summary>
		/// Тут будет "Мастер станочного участка"/"Мастер 049 уч."
		/// </summary>
		public Name Name { get; }

		/// <summary>
		/// Участок на котором сотрудник работает
		/// </summary>
		public Department Department { get; }

		/// <summary>
		/// Почта сотрудника/рабочего места
		/// </summary>
		public Email Email { get; }

		/// <summary>
		/// Номер телефона сотрудника/рабочего места
		/// </summary>
		public Phone Phone { get; }

		/// <summary>
		/// Специализация сотрудника с ролью Исполнитель, например "Механик", "Электрик", "Слесарь" 
		/// </summary>
		public Specialized? Specialized { get; private set; }
	}
}
