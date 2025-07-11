namespace WorkOrderX.Domain.Models.EventStores
{
	public class EventStoreEntry
	{
		/// <summary>
		/// Уникальный идентификатор записи в журнале событий
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Тип события, например "ProcessRequestStatusChanged"
		/// </summary>
		public string EventType { get; set; }

		/// <summary>
		/// Уникальный идентификатор агрегата, к которому относится событие
		/// </summary>
		public Guid AggregateId { get; set; }

		/// <summary>
		/// Дата и время, когда событие произошло
		/// </summary>
		public DateTime OccurredAt { get; set; } = DateTime.Now;

		/// <summary>
		/// Старый статус заявки, если применимо
		/// </summary>
		public int? OldStatusId { get; set; }

		/// <summary>
		/// Новый статус заявки, если применимо
		/// </summary>
		public int NewStatusId { get; set; }

		/// <summary>
		/// Идентификатор сотрудника, который изменил статус заявки
		/// </summary>
		public Guid? ChangedByEmployeeId { get; set; }

		/// <summary>
		/// Идентификатор исполнителя заявки, если применимо
		/// </summary>
		public Guid? ExecutorEmployeeId { get; set; }

		/// <summary>
		/// Идентификатор заказчика заявки
		/// </summary>
		public Guid CustomerEmployeeId { get; set; }

		/// <summary>
		/// Комментарий к событию, если применимо
		/// </summary>
		public string? Comment { get; set; }
	}
}
