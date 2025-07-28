namespace WorkOrderX.Application.Models.DTOs
{
	/// <summary>
	/// DTO для данных заявки с заказчиком и исполнителем
	/// </summary>
	public record ProcessRequestDataDto
	{
		/// <summary>
		/// Идентификатор заявки
		/// </summary>
		public Guid Id { get; init; }

		/// <summary>
		/// Номер заявки
		/// </summary>
		public long ApplicationNumber { get; init; }

		/// <summary>
		/// Тип заявки
		/// </summary>
		public string ApplicationType { get; init; }

		/// <summary>
		/// Дата и время создания заявки
		/// </summary>
		public string CreatedAt { get; init; }

		/// <summary>
		/// Дата и время, когда планируется исполнение заявки
		/// </summary>
		public string PlannedAt { get; init; }

		/// <summary>
		/// Дата и время, когда заявка была обновлена
		/// </summary>
		public string? UpdatedAt { get; init; }

		/// <summary>
		/// Дата и время, когда заявка была исполнена
		/// </summary>
		public string? CompletionAt { get; init; }

		/// <summary>
		/// Наименование оборудования, по которому создана заявка
		/// </summary>
		public string? EquipmentType { get; init; }

		/// <summary>
		/// Наименование вида оборудования, по которому создана заявка
		/// </summary>
		public string? EquipmentKind { get; init; }

		/// <summary>
		/// Модель оборудования, по которому создана заявка
		/// </summary>
		public string? EquipmentModel { get; init; }

		/// <summary>
		/// Серийный номер оборудования, по которому создана заявка
		/// </summary>
		public string? SerialNumber { get; init; }

		/// <summary>
		/// Тип поломки, по которому создана заявка
		/// </summary>
		public string TypeBreakdown { get; init; }

		/// <summary>
		/// Описание поломки, по которому создана заявка
		/// </summary>
		public string DescriptionMalfunction { get; init; }

		/// <summary>
		/// Статус заявки
		/// </summary>
		public string ApplicationStatus { get; init; }

		/// <summary>
		/// Внутренний комментарий к заявке
		/// </summary>
		public string? InternalComment { get; init; }

		/// <summary>
		/// Заказчик, сотрудник создавший заявку
		/// </summary>
		public EmployeeDataDto CustomerEmployee { get; init; }

		/// <summary>
		/// Исполнитель, сотрудник назначенный на исполнение заявки
		/// </summary>
		public EmployeeDataDto ExecutorEmployee { get; init; }

		/// <summary>
		/// Важность заявки
		/// </summary>
		public string Importance { get; init; }
	}
}
