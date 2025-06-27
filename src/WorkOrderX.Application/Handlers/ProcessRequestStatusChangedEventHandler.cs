using MediatR;

using WorkOrderX.Application.Services.Email.Interfaces;
using WorkOrderX.Domain.AggregationModels.Employees;
using WorkOrderX.Domain.AggregationModels.EventStores;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.ProcessRequests.DomainEvents;

namespace WorkOrderX.Application.Handlers
{
	/// <summary>
	/// Обработчик события изменения статуса заявки.
	/// </summary>
	public class ProcessRequestStatusChangedEventHandler : INotificationHandler<ProcessRequestStatusChangedEvent>
	{
		private readonly IProcessRequestRepository _processRequestRepository;
		private readonly IEventStoreEntryRepository _eventStoreEntryRepository;
		private readonly IWorkplaceEmployeesRepository _workplaceEmployeesRepository;
		private readonly IMailService _mailService;

		/// <summary>
		/// Конструктор обработчика события изменения статуса заявки.
		/// </summary>
		/// <param name="processRequestRepository"></param>
		/// <param name="eventStoreEntryRepository"></param>
		/// <param name="workplaceEmployeesRepository"></param>
		/// <param name="mailService"></param>
		public ProcessRequestStatusChangedEventHandler(IProcessRequestRepository processRequestRepository, IEventStoreEntryRepository eventStoreEntryRepository, IWorkplaceEmployeesRepository workplaceEmployeesRepository, IMailService mailService)
		{
			_processRequestRepository = processRequestRepository;
			_eventStoreEntryRepository = eventStoreEntryRepository;
			_workplaceEmployeesRepository = workplaceEmployeesRepository;
			_mailService = mailService;
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
			if (notification == null)
				throw new ApplicationException("Notification cannot be null.");

			var request = await _processRequestRepository.GetByIdAsync(notification.RequestId, cancellationToken) ??
				throw new ApplicationException($"Заявка не найдена по Id {nameof(notification.RequestId)}");

			var customer = await _workplaceEmployeesRepository.GetByIdAsync(notification.CustomerEmployeeId, cancellationToken) ??
				throw new ApplicationException($"Заказчик заявки не найден {nameof(notification.CustomerEmployeeId)}");

			if (notification.ExecutorEmployeeId is null)
				throw new ApplicationException("ExecutorEmployeeId cannot be null.");

			var executor = await _workplaceEmployeesRepository.GetByIdAsync(notification.ExecutorEmployeeId, cancellationToken) ??
				throw new ApplicationException($"Исполнитель заявки не найден {nameof(notification.ExecutorEmployeeId)}");

			// Отправляет уведомления по электронной почте в зависимости от нового статуса заявки.
			await SendStatusChangedNotificationEmailsAsync(notification, request, customer, executor);

			// Добавляем запись в журнал событий
			await _eventStoreEntryRepository.AddEventStoreEntryAsync(new EventStoreEntry
			{
				EventType = nameof(ProcessRequestStatusChangedEvent),
				AggregateId = request.Id,
				OldStatusId = notification?.OldStatus?.Id ?? null,
				NewStatusID = notification.NewStatus.Id,
				ChangedByEmployeeID = notification.ChangedByEmployeeId,
				ExecutorEmployeeId = notification.ExecutorEmployeeId,
				CustomerEmployeeId = notification.CustomerEmployeeId,
				Comment = notification.Comment
			}, cancellationToken);
		}

		/// <summary>
		/// Отправляет уведомления по электронной почте в зависимости от нового статуса заявки.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendStatusChangedNotificationEmailsAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			if (notification.NewStatus == ApplicationStatus.New)
				await SendNewApplicationRegisteredEmailAsync(notification, request, customer, executor);

			if (notification.NewStatus == ApplicationStatus.Changed)
				await SendChangedApplicationNotificationEmailAsync(notification, request, customer, executor);

			if (notification.NewStatus == ApplicationStatus.InWork)
				await SendInWorkApplicationNotificationEmailAsync(notification, request, customer, executor);

			if (notification.NewStatus == ApplicationStatus.Redirected)
				await SendRedirectedApplicationNotificationEmailAsync(notification, request, customer, executor);

			if (notification.NewStatus == ApplicationStatus.Rejected)
				await SendRejectedApplicationNotificationEmailAsync(notification, request, customer, executor);

			if (notification.NewStatus == ApplicationStatus.Done)
				await SendDoneApplicationNotificationEmailAsync(notification, request, customer, executor);

			if (notification.NewStatus == ApplicationStatus.Postponed)
				await SendPostponedApplicationNotificationEmailAsync(notification, request, customer, executor);

			if (notification.NewStatus == ApplicationStatus.Returned)
				await SendReturnedApplicationNotificationEmailAsync(notification, request, customer, executor);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о том, что заявка была возвращена для исправлений или уточнений.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendReturnedApplicationNotificationEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { customer.Email.Value };
			var recipientsCopy = new List<string> { executor.Email.Value };
			var subject = "Заявка возвращена";
			var text = @$"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была Вам возвращена для исправлений \ уточнений.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о том, что заявка была отложена.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendPostponedApplicationNotificationEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { customer.Email.Value };
			var recipientsCopy = new List<string> { executor.Email.Value };
			var subject = "Заявка отложена";
			var text = @$"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была отложена.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о том, что заявка была выполнена.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendDoneApplicationNotificationEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { customer.Email.Value };
			var recipientsCopy = new List<string> { executor.Email.Value };
			var subject = "Заявка выполнена";
			var text = @$"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была успешно выполнена.
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о том, что заявка была отклонена.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendRejectedApplicationNotificationEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { customer.Email.Value };
			var recipientsCopy = new List<string> { executor.Email.Value };
			var subject = "Заявка отклонена";
			var text = @$"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была отклонена.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о том, что заявка была перенаправлена другому исполнителю.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendRedirectedApplicationNotificationEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { executor.Email.Value };
			var recipientsCopy = new List<string> { customer.Email.Value };
			var subject = "Заявка перенаправлена";
			var text = @$"Уважаемый {executor.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была перенаправлена Вам.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Заказчик: {customer.Name.Value}
						   Номер телефона заказчика: {customer.Phone.Value}
						   С уважением, Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о том, что заявка взята в работу.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendInWorkApplicationNotificationEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { customer.Email.Value };
			var recipientsCopy = new List<string> { executor.Email.Value };
			var subject = "Заявка взята в работу";
			var text = @$"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} взята в работу.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о том, что заявка была изменена и отправлена на выполнение.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendChangedApplicationNotificationEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { executor.Email.Value };
			var recipientsCopy = new List<string> { customer.Email.Value };
			var subject = "Заявку изменили и отправили на выполнение";
			var text = @$"Уважаемый {executor.Name.Value},
						   Статус заявки №{request.ApplicationNumber.Value} изменен на {notification.NewStatus}.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Заказчик: {customer.Name.Value}
						   Номер телефона заказчика: {customer.Phone.Value}
						   С уважением,Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о регистрации новой заявки.
		/// </summary>
		/// <param name="notification"></param>
		/// <param name="request"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private async Task SendNewApplicationRegisteredEmailAsync(ProcessRequestStatusChangedEvent notification, ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			var recipients = new List<string> { executor.Email.Value };
			var recipientsCopy = new List<string> { customer.Email.Value };
			var subject = "Зарегистрирована новая заявка на выполнение";
			var text = @$"Уважаемый {executor.Name.Value},
						   Появилась новая заявка №{request.ApplicationNumber.Value} на выполнение.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Заказчик: {customer.Name.Value}
						   Номер телефона заказчика: {customer.Phone.Value}
						   С уважением,Команда WorkOrderX";

			await SendMailRequestNotificationAsync(recipients, recipientsCopy, subject, text);
		}

		/// <summary>
		/// Отправляет уведомление по электронной почте о статусе заявки.
		/// </summary>
		/// <param name="recipients"></param>
		/// <param name="recipientsCopy"></param>
		/// <param name="subject"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		private async Task SendMailRequestNotificationAsync(List<string> recipients, List<string> recipientsCopy, string subject, string text)
		{
			await _mailService.SendMailAsync(new MailerVKT.MailParameters
			{
				Recipients = recipients,
				RecipientsCopy = recipientsCopy,
				Subject = subject,
				Text = text,
				RecipientsBcc = ["progto@vkt-vent.ru"],
				SenderName = "WorkOrderX",
			});
		}
	}
}
