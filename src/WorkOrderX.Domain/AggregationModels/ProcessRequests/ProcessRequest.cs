using WorkOrderX.Domain.AggregationModels.ProcessRequests.DomainEvents;
using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Заявка
	/// </summary>
	public class ProcessRequest : Entity, IAggregationRoot
	{
		/// <summary>
		/// Конструктор для EF Core
		/// </summary>
		private ProcessRequest() { }

		/// <summary>
		/// Создает новую заявку на Хозяйственные работы
		/// </summary>
		/// <param name="id">Id</param>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Даиа создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>		
		/// <param name="completionAt">Дата завершения заявки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Важность заявки</param>
		/// <param name="location">Локация в заявке хоз.работ</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="executorEmployeeId">ID исполнителя заявки</param>
		private ProcessRequest(
			Guid id,
			ApplicationNumber applicationNumber,
			ApplicationType applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			DateTime? completionAt,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Location? location,
			Guid customerEmployeeId,
			Guid executorEmployeeId)
		{
			Id = id;
			ApplicationNumber = applicationNumber;
			ApplicationType = applicationType;
			CreatedAt = createdAt;
			PlannedAt = plannedAt;
			CompletionAt = completionAt;
			DescriptionMalfunction = descriptionMalfunction;
			ApplicationStatus = applicationStatus;
			InternalComment = internalComment;
			Importance = importance;
			Location = location;
			CustomerEmployeeId = customerEmployeeId;
			ExecutorEmployeeId = executorEmployeeId;
		}

		/// <summary>
		/// Создает новую заявку на Хозяйственные работы
		/// </summary>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Дата создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>		
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Важность заявки</param>
		/// <param name="location">Локация в заявке хоз.работ</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="executorEmployeeId">ID исполнителя заявки</param>
		/// <returns></returns>
		public static ProcessRequest CreateHouseholdChores(
			ApplicationNumber applicationNumber,
			ApplicationType applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Location? location,
			Guid customerEmployeeId,
			Guid executorEmployeeId)
		{
			var newProcessRequest = new ProcessRequest(
				id: Guid.NewGuid(),
				applicationNumber: applicationNumber,
				applicationType: applicationType,
				createdAt: createdAt,
				plannedAt: plannedAt,
				completionAt: null,
				descriptionMalfunction: descriptionMalfunction,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				importance: importance,
				location: location,
				customerEmployeeId: customerEmployeeId,
				executorEmployeeId: executorEmployeeId);

			newProcessRequest.UpdatedAt = createdAt;

			newProcessRequest.AddDomainEvent(new ProcessRequestStatusChangedEvent
			{
				ChangedByEmployeeId = customerEmployeeId,
				CustomerEmployeeId = customerEmployeeId,
				ExecutorEmployeeId = executorEmployeeId,
				OldStatus = null,
				NewStatus = applicationStatus,
				Comment = descriptionMalfunction?.Value, //При создании новой заявки, есть только описание неисправности
				RequestId = newProcessRequest.Id,
				Importance = importance,
			});

			newProcessRequest.AddIntegrationEvent(new ProcessRequestChangedEvent
			{
				RequestId = newProcessRequest.Id
			});

			return newProcessRequest;
		}

		/// <summary>
		/// Создает новую заявку на ремонт инструмента
		/// </summary>
		/// <param name="id">Id</param>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Даиа создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>		
		/// <param name="completionAt">Дата завершения заявки</param>
		/// <param name="instrumentType">Тип инструмента</param>
		/// <param name="instrumentKind">Вид инструмента</param>
		/// <param name="instrumentModel">Модель инструмента</param>
		/// <param name="serialNumber">Серийный номер инструмента</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Важность заявки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="executorEmployeeId">ID исполнителя заявки</param>
		private ProcessRequest(
			Guid id,
			ApplicationNumber applicationNumber,
			ApplicationType applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			DateTime? completionAt,
			InstrumentType? instrumentType,
			InstrumentKind? instrumentKind,
			InstrumentModel? instrumentModel,
			SerialNumber? serialNumber,
			TypeBreakdown? typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Guid customerEmployeeId,
			Guid executorEmployeeId)
		{
			Id = id;
			ApplicationNumber = applicationNumber;
			ApplicationType = applicationType;
			CreatedAt = createdAt;
			PlannedAt = plannedAt;
			CompletionAt = completionAt;
			InstrumentType = instrumentType;
			InstrumentKind = instrumentKind;
			InstrumentModel = instrumentModel;
			SerialNumber = serialNumber;
			TypeBreakdown = typeBreakdown;
			DescriptionMalfunction = descriptionMalfunction;
			ApplicationStatus = applicationStatus;
			InternalComment = internalComment;
			Importance = importance;
			CustomerEmployeeId = customerEmployeeId;
			ExecutorEmployeeId = executorEmployeeId;
		}

		/// <summary>
		/// Создает новую заявку на ремонт инструмента
		/// </summary>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Дата создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>		
		/// <param name="instrumentType">Тип инструмента</param>
		/// <param name="instrumentKind">Вид инструмента</param>
		/// <param name="instrumentModel">Модель инструмента</param>
		/// <param name="serialNumber">Серийный номер инструмента</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Важность заявки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="executorEmployeeId">ID исполнителя заявки</param>
		/// <returns></returns>
		public static ProcessRequest CreateInstrumentRepair(
			ApplicationNumber applicationNumber,
			ApplicationType applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			InstrumentType? instrumentType,
			InstrumentKind? instrumentKind,
			InstrumentModel? instrumentModel,
			SerialNumber? serialNumber,
			TypeBreakdown? typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Guid customerEmployeeId,
			Guid executorEmployeeId)
		{
			var newProcessRequest = new ProcessRequest(
				id: Guid.NewGuid(),
				applicationNumber: applicationNumber,
				applicationType: applicationType,
				createdAt: createdAt,
				plannedAt: plannedAt,
				completionAt: null,
				instrumentType: instrumentType,
				instrumentKind: instrumentKind,
				instrumentModel: instrumentModel,
				serialNumber: serialNumber,
				typeBreakdown: typeBreakdown,
				descriptionMalfunction: descriptionMalfunction,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				importance: importance,
				customerEmployeeId: customerEmployeeId,
				executorEmployeeId: executorEmployeeId);

			newProcessRequest.UpdatedAt = createdAt;

			newProcessRequest.AddDomainEvent(new ProcessRequestStatusChangedEvent
			{
				ChangedByEmployeeId = customerEmployeeId,
				CustomerEmployeeId = customerEmployeeId,
				ExecutorEmployeeId = executorEmployeeId,
				OldStatus = null,
				NewStatus = applicationStatus,
				Comment = descriptionMalfunction?.Value, //При создании новой заявки, есть только описание неисправности
				RequestId = newProcessRequest.Id,
				Importance = importance,
			});

			newProcessRequest.AddIntegrationEvent(new ProcessRequestChangedEvent
			{
				RequestId = newProcessRequest.Id
			});

			return newProcessRequest;
		}

		/// <summary>
		/// Установка завершенной или отклоненной заявки и статуса с комментарием
		/// </summary>
		/// <param name="completionAt">Дата завершения заявки</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <exception cref="DomainException"></exception>
		public void SetRequestDoneOrRejected(
			DateTime completionAt,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment = null)
		{
			ValidateRequestIsActive();

			if (applicationStatus.Id == ApplicationStatus.Done.Id || applicationStatus.Id == ApplicationStatus.Rejected.Id)
			{
				if (completionAt < CreatedAt)
					throw new DomainException("Дата завершения заявки не может быть раньше даты создания заявки");

				AddDomainEvent(new ProcessRequestStatusChangedEvent
				{
					ChangedByEmployeeId = ExecutorEmployeeId,
					CustomerEmployeeId = CustomerEmployeeId,
					ExecutorEmployeeId = ExecutorEmployeeId,
					OldStatus = ApplicationStatus,
					NewStatus = applicationStatus,
					Comment = internalComment?.Value,
					RequestId = Id,
					Importance = Importance,
				});

				AddIntegrationEvent(new ProcessRequestChangedEvent
				{
					RequestId = Id
				});

				CompletionAt = completionAt;
				ApplicationStatus = applicationStatus;
				InternalComment = internalComment;
				UpdatedAt = DateTime.Now;
			}
			else
			{
				throw new DomainException("Статус на завершение заявки должен быть Done или Rejected");
			}
		}

		/// <summary>
		/// Проверка, что заявка активна (не завершена и не отклонена)
		/// </summary>
		/// <exception cref="DomainException"></exception>
		private void ValidateRequestIsActive()
		{
			if (ApplicationStatus.Id == ApplicationStatus.Done.Id || ApplicationStatus.Id == ApplicationStatus.Rejected.Id)
				throw new DomainException("Уже завершенная или отклоненная заявка не может быть изменена");
		}

		/// <summary>
		/// Установка статуса заявки в работу (при нажатии кнопки принять в работу в программе)
		/// </summary>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <exception cref="DomainException"></exception>
		public void SetStatusInWork(
			ApplicationStatus applicationStatus,
			InternalComment? internalComment)
		{
			ValidateRequestIsActive();

			if (ApplicationStatus.Id == ApplicationStatus.InWork.Id)
				throw new DomainException("Заявка уже в работе");

			if (applicationStatus.Id != ApplicationStatus.InWork.Id)
				throw new DomainException("Статус заявки должен быть InWork");

			AddDomainEvent(new ProcessRequestStatusChangedEvent
			{
				ChangedByEmployeeId = ExecutorEmployeeId,
				CustomerEmployeeId = CustomerEmployeeId,
				ExecutorEmployeeId = ExecutorEmployeeId,
				OldStatus = ApplicationStatus,
				NewStatus = applicationStatus,
				Comment = internalComment?.Value,
				RequestId = Id,
				Importance = Importance,
			});

			AddIntegrationEvent(new ProcessRequestChangedEvent
			{
				RequestId = Id
			});

			InternalComment = internalComment;
			ApplicationStatus = applicationStatus;
			UpdatedAt = DateTime.Now;
		}

		/// <summary>
		/// Перенаправление заявки другому исполнителю
		/// </summary>
		/// <param name="executorEmployeeId">ID исполнителя заявки</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий к заявке при перенаправлении. По умолчанию сделать в доменном сервисе индентификацию, кто из исполнителей кому перенаправил, если комментарий был пустым.</param>
		/// <exception cref="DomainException"></exception>
		public void ReassignmentExecutorEmployeeId(
			Guid executorEmployeeId,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment = null)
		{
			ValidateRequestIsActive();

			if (executorEmployeeId == Guid.Empty)
				throw new DomainException("ID исполнителя не может быть пустым");

			if (ExecutorEmployeeId == executorEmployeeId)
				throw new DomainException("Исполнитель не может быть переназначен на себя");

			if (applicationStatus.Id != ApplicationStatus.Redirected.Id)
				throw new DomainException("Статус заявки должен быть Redirected");

			AddDomainEvent(new ProcessRequestStatusChangedEvent
			{
				ChangedByEmployeeId = ExecutorEmployeeId,
				CustomerEmployeeId = CustomerEmployeeId,
				ExecutorEmployeeId = executorEmployeeId,
				OldStatus = ApplicationStatus,
				NewStatus = applicationStatus,
				Comment = internalComment?.Value,
				RequestId = Id,
				Importance = Importance,
			});

			AddIntegrationEvent(new ProcessRequestChangedEvent
			{
				RequestId = Id
			});

			ExecutorEmployeeId = executorEmployeeId;
			ApplicationStatus = applicationStatus;
			InternalComment = internalComment;
			UpdatedAt = DateTime.Now;
		}

		/// <summary>
		/// Установка статуса заявки на возврат или отложение с комментарием
		/// </summary>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <exception cref="DomainException"></exception>
		public void SetStatusReturnedOrPostponed(
			ApplicationStatus applicationStatus,
			DateTime? plannedAt,
			InternalComment? internalComment = null)
		{
			ValidateRequestIsActive();

			if (ApplicationStatus.Id == ApplicationStatus.Returned.Id && applicationStatus.Id == ApplicationStatus.Returned.Id)
				throw new DomainException("Уже возвращенная заявка не может быть изменена");

			if (ApplicationStatus.Id == ApplicationStatus.Postponed.Id && applicationStatus.Id == ApplicationStatus.Postponed.Id)
				throw new DomainException("Уже отложенная заявка не может быть изменена");

			if (applicationStatus.Id == ApplicationStatus.Returned.Id || applicationStatus.Id == ApplicationStatus.Postponed.Id)
			{
				AddDomainEvent(new ProcessRequestStatusChangedEvent
				{
					ChangedByEmployeeId = ExecutorEmployeeId,
					CustomerEmployeeId = CustomerEmployeeId,
					ExecutorEmployeeId = ExecutorEmployeeId,
					OldStatus = ApplicationStatus,
					NewStatus = applicationStatus,
					Comment = internalComment?.Value,
					RequestId = Id,
					Importance = Importance,
				});

				AddIntegrationEvent(new ProcessRequestChangedEvent
				{
					RequestId = Id
				});

				ApplicationStatus = applicationStatus;
				InternalComment = internalComment;
				UpdatedAt = DateTime.Now;

				if (plannedAt != null)
					PlannedAt = (DateTime)plannedAt;
			}
			else
			{
				throw new DomainException("Статус на возврат или отложение заявки должен быть Returned или Postponed");
			}
		}

		/// <summary>
		/// Обновление заявки на ремонт инструмента
		/// </summary>
		public void UpdateInstrumentRepair(
			ApplicationType applicationType,
			InstrumentType? instrumentType,
			InstrumentKind? instrumentKind,
			InstrumentModel? instrumentModel,
			SerialNumber? serialNumber,
			TypeBreakdown? typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Guid customerEmployeeId,
			Guid executorEmployeeId)
		{
			// Проверка, что заявка активна (не завершена и не отклонена)
			ValidateRequestIsActive();

			AddDomainEvent(new ProcessRequestStatusChangedEvent
			{
				ChangedByEmployeeId = CustomerEmployeeId,
				CustomerEmployeeId = CustomerEmployeeId,
				ExecutorEmployeeId = ExecutorEmployeeId,
				OldStatus = ApplicationStatus,
				NewStatus = applicationStatus,
				Comment = internalComment?.Value,
				RequestId = Id,
				Importance = importance,
			});

			AddIntegrationEvent(new ProcessRequestChangedEvent
			{
				RequestId = Id
			});

			ApplicationType = applicationType;
			InstrumentType = instrumentType;
			InstrumentKind = instrumentKind;
			InstrumentModel = instrumentModel;
			SerialNumber = serialNumber;
			TypeBreakdown = typeBreakdown;
			DescriptionMalfunction = descriptionMalfunction;
			ApplicationStatus = applicationStatus;
			InternalComment = internalComment;
			CustomerEmployeeId = customerEmployeeId;
			ExecutorEmployeeId = executorEmployeeId;
			Importance = importance;
			UpdatedAt = DateTime.Now;
		}

		/// <summary>
		/// Обновление заявки на Хоз. работы
		/// </summary>
		public void UpdateHouseholdChores(
			ApplicationType applicationType,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Location? location,
			Guid customerEmployeeId,
			Guid executorEmployeeId)
		{
			// Проверка, что заявка активна (не завершена и не отклонена)
			ValidateRequestIsActive();

			AddDomainEvent(new ProcessRequestStatusChangedEvent
			{
				ChangedByEmployeeId = CustomerEmployeeId,
				CustomerEmployeeId = CustomerEmployeeId,
				ExecutorEmployeeId = ExecutorEmployeeId,
				OldStatus = ApplicationStatus,
				NewStatus = applicationStatus,
				Comment = internalComment?.Value,
				RequestId = Id,
				Importance = importance,
			});

			AddIntegrationEvent(new ProcessRequestChangedEvent
			{
				RequestId = Id
			});

			ApplicationType = applicationType;
			DescriptionMalfunction = descriptionMalfunction;
			ApplicationStatus = applicationStatus;
			InternalComment = internalComment;
			CustomerEmployeeId = customerEmployeeId;
			ExecutorEmployeeId = executorEmployeeId;
			Importance = importance;
			Location = location;
			UpdatedAt = DateTime.Now;
		}

		/// <summary>
		/// Сохранение комментария специалиста (при нажатии кнопки сохранить коммент. в программе)
		/// </summary>
		/// <exception cref="DomainException"></exception>
		public void UpdateInternalComment(InternalComment? internalComment)
		{
			ValidateRequestIsActive();

			AddIntegrationEvent(new ProcessRequestChangedEvent
			{
				RequestId = Id
			});

			InternalComment = internalComment;
			UpdatedAt = DateTime.Now;
		}

		/// <summary>
		/// Номер заявки
		/// </summary>
		public ApplicationNumber ApplicationNumber { get; private set; }

		/// <summary>
		/// Тип заявки
		/// </summary>
		public ApplicationType ApplicationType { get; private set; }

		/// <summary>
		/// Дата создания заявки
		/// </summary>
		public DateTime CreatedAt { get; private set; }

		/// <summary>
		/// Дата завершения заявки
		/// </summary>
		public DateTime? CompletionAt { get; private set; }

		/// <summary>
		/// Плановая дата завершения
		/// </summary>
		public DateTime PlannedAt { get; private set; }

		/// <summary>
		/// Дата обновления заявки
		/// </summary>
		public DateTime? UpdatedAt { get; private set; }

		/// <summary>
		/// Тип инструмента
		/// </summary>
		public InstrumentType? InstrumentType { get; private set; }

		/// <summary>
		/// Вид инструмента
		/// </summary>
		public InstrumentKind? InstrumentKind { get; private set; }

		/// <summary>
		/// Модель инструмента
		/// </summary>
		public InstrumentModel? InstrumentModel { get; private set; }

		/// <summary>
		/// Серийный номер
		/// </summary>
		public SerialNumber? SerialNumber { get; private set; }//TODO: Если пришлют список - сделать списком. Иначе строка

		/// <summary>
		/// Тип поломки
		/// </summary>
		public TypeBreakdown? TypeBreakdown { get; private set; }

		/// <summary>
		/// Описание неисправности
		/// </summary>
		public DescriptionMalfunction DescriptionMalfunction { get; private set; }

		/// <summary>
		/// Статус заявки
		/// </summary>
		public ApplicationStatus ApplicationStatus { get; private set; }

		/// <summary>
		/// Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.
		/// </summary>
		public InternalComment? InternalComment { get; private set; }

		/// <summary>
		/// Важность заявки
		/// </summary>
		public Importance Importance { get; private set; }

		/// <summary>
		/// Локация в заявке хоз. работ
		/// </summary>
		public Location? Location { get; private set; }

		/// <summary>
		/// Заказчик заявки (кто создал)
		/// </summary>
		public Guid CustomerEmployeeId { get; private set; }

		/// <summary>
		/// Исполнитель заявки (кому прислали на исполнение заявки)
		/// </summary>
		public Guid ExecutorEmployeeId { get; private set; }




		public int? InstrumentTypeId { get; private set; }
		public int? InstrumentKindId { get; private set; }
		public int? TypeBreakdownId { get; private set; }
		public int? InstrumentModelId { get; private set; }
		public int? ApplicationStatusId { get; private set; }
		public int? ApplicationTypeId { get; private set; }
		public int? ImportanceId { get; private set; }
	}
}