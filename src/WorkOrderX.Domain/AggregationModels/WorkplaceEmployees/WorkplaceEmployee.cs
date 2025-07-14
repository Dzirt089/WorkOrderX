using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	/// <summary>
	/// Рабочее место сотрудников. По Бизнес-логике, это рабочее место участка (их много) на предприятии, 
	/// за которым по сменно работают два мастера участка. Мастера максимально "абстрактны" для программы, чтобы не привязываться к человеку отдельно. За рабочем местом один комп., одна учётная запись для компа, и телефон.
	/// </summary>
	public class WorkplaceEmployee : Entity, IAggregationRoot
	{
		/// <summary>
		/// Конструктор для EF Core
		/// </summary>
		private WorkplaceEmployee() { }

		/// <summary>
		/// Конструктор для создания нового сотрудника.
		/// </summary>
		/// <param name="id">Индентификатор сотрудника</param>
		/// <param name="account">Учетная запись рабочего места сотрудника</param>
		/// <param name="role">Роль у сотрудника (Заказчик, исполнитель, админ, куратор)</param>
		/// <param name="name">Наименование рабочего места сотрудника</param>
		/// <param name="department">Участок. за которым закреплено рабочее места сотрудника</param>
		/// <param name="email">Почта рабочего места сотрудника</param>
		/// <param name="phone">Телефон рабочего места сотрудника</param>
		/// <param name="specialized">Специальность сотрудника (для исполнителей)</param>
		private WorkplaceEmployee(
			Guid id,
			Account account,
			Role role,
			Name name,
			Department? department,
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

		/// <summary>
		/// Создание нового сотрудника. 
		/// </summary>
		/// <param name="account"></param>
		/// <param name="role"></param>
		/// <param name="name"></param>
		/// <param name="department"></param>
		/// <param name="email"></param>
		/// <param name="phone"></param>
		/// <param name="specialized"></param>
		/// <returns></returns>
		public static WorkplaceEmployee Create(
			Account account,
			Role role,
			Name name,
			Department? department,
			Email email,
			Phone phone,
			Specialized? specialized)
		{

			if (account is null)
				throw new DomainException($"Учетная запись сотрудника не может быть null. {nameof(account)}");

			if (role is null)
				throw new DomainException($"Роль сотрудника не может быть null. {nameof(role)}");

			if (name is null)
				throw new DomainException($"Имя сотрудника не может быть null. {nameof(name)}");

			if (department is null)
				throw new DomainException($"Участок сотрудника не может быть null. {nameof(department)}");

			if (email is null)
				throw new DomainException($"Почта сотрудника не может быть null. {nameof(email)}");

			if (phone is null)
				throw new DomainException($"Телефон сотрудника не может быть null. {nameof(phone)}");

			var newEmployee =
				new WorkplaceEmployee(
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

		public void SetSpecialized(Specialized specialized)
		{
			if (specialized is null)
				throw new DomainException($"Специализация сотрудника не может быть null. {nameof(specialized)}");
			Specialized = specialized;
		}

		/// <summary>
		/// Учетная запись сотрудника/рабочего места
		/// </summary>
		public Account Account { get; private set; }

		/// <summary>
		/// Роль сотрудника
		/// </summary>
		public Role Role { get; private set; }

		/// <summary>
		/// Тут будет "Мастер станочного участка"/"Мастер 049 уч."
		/// </summary>
		public Name Name { get; private set; }

		/// <summary>
		/// Участок на котором сотрудник работает
		/// </summary>
		public Department Department { get; private set; }

		/// <summary>
		/// Почта сотрудника/рабочего места
		/// </summary>
		public Email Email { get; private set; }

		/// <summary>
		/// Номер телефона сотрудника/рабочего места
		/// </summary>
		public Phone Phone { get; private set; }

		/// <summary>
		/// Специализация сотрудника с ролью Исполнитель, например "Механик", "Электрик", "Слесарь" 
		/// </summary>
		public Specialized? Specialized { get; private set; }

		public int RoleId { get; private set; }
		public int? SpecializedId { get; private set; }
	}
}