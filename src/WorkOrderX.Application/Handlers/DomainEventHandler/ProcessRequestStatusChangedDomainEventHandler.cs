using MediatR;

using WorkOrderX.Application.Commands.StatusChangeEmail;
using WorkOrderX.Domain.AggregationModels.ProcessRequests.DomainEvents;
using WorkOrderX.Domain.Models.EventStores;

namespace WorkOrderX.Application.Handlers.DomainEventHandler
{
	/// <summary>
	/// Обработчик события изменения статуса заявки.
	/// </summary>
	public class ProcessRequestStatusChangedDomainEventHandler : INotificationHandler<ProcessRequestStatusChangedEvent>
	{
		private readonly IMediator _mediator;
		private readonly IEventStoreEntryRepository _eventStoreEntryRepository;

		/// <summary>
		/// Конструктор обработчика события изменения статуса заявки.
		/// </summary>
		public ProcessRequestStatusChangedDomainEventHandler(
			IEventStoreEntryRepository eventStoreEntryRepository, IMediator mediator)
		{
			_eventStoreEntryRepository = eventStoreEntryRepository;
			_mediator = mediator;
		}

		/// <summary>
		/// Обрабатывает событие изменения статуса заявки.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		public async Task Handle(ProcessRequestStatusChangedEvent notification, CancellationToken cancellationToken)
		{
			if (notification is null)
				throw new ApplicationException("Notification cannot be null");

			// Сохраняем событие в Event Store
			await SaveEventStoreAsync(notification, cancellationToken);

			// Отправляем уведомление о смене статуса заявки
			await SendStatusNotificationAsync(notification, cancellationToken);
		}

		/// <summary>
		/// Отправляет уведомление о смене статуса заявки.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task SendStatusNotificationAsync(ProcessRequestStatusChangedEvent notification, CancellationToken cancellationToken)
		{
			var emailParams = new StatusChangeEmailParamsCommand
			{
				RequestId = notification.RequestId,
				NewStatus = notification.NewStatus,
				Comment = notification.Comment,
				OldStatus = notification.OldStatus,
				CustomerEmployeeId = notification.CustomerEmployeeId,
				ExecutorEmployeeId = notification.ExecutorEmployeeId,
				ChangedByEmployeeId = notification.ChangedByEmployeeId
			};

			await _mediator.Send(emailParams, cancellationToken);
		}

		/// <summary>
		/// Сохраняет событие изменения статуса заявки в Event Store.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task SaveEventStoreAsync(ProcessRequestStatusChangedEvent notification, CancellationToken cancellationToken)
		{
			var entry = new EventStoreEntry
			{
				EventType = nameof(ProcessRequestStatusChangedEvent),
				AggregateId = notification.RequestId,
				OldStatusId = notification?.OldStatus?.Id ?? null,
				NewStatusId = notification.NewStatus.Id,
				ChangedByEmployeeId = notification.ChangedByEmployeeId,
				ExecutorEmployeeId = notification.ExecutorEmployeeId,
				CustomerEmployeeId = notification.CustomerEmployeeId,
				Comment = notification.Comment,
				OccurredAt = DateTime.Now
			};

			await _eventStoreEntryRepository.AddEventStoreEntryAsync(entry: entry, cancellationToken);
		}
	}
}
