using MediatR;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests.DomainEvents
{
	public sealed record ProcessRequestStatusChangedEvent : INotification
	{

		/// <summary>
		/// ID заявки
		/// </summary>
		public Guid RequestId { get; init; }

		/// <summary>
		/// Старый статус заявки
		/// </summary>
		public ApplicationStatus? OldStatus { get; init; }

		/// <summary>
		/// Новый статус заявки
		/// </summary>
		public ApplicationStatus NewStatus { get; init; }

		/// <summary>
		/// Комментарий к изменению статуса заявки
		/// </summary>
		public string? Comment { get; init; }

		/// <summary>
		/// Уровень важности заявки
		/// </summary>
		public Importance Importance { get; init; }

		/// <summary>
		/// ID сотрудника, который изменил статус заявки
		/// </summary>
		public Guid? ChangedByEmployeeId { get; init; }

		/// <summary>
		/// ID исполнителя заявки
		/// </summary>
		public Guid? ExecutorEmployeeId { get; init; }

		/// <summary>
		/// ID заказчика заявки
		/// </summary>
		public Guid CustomerEmployeeId { get; init; }
	}
}