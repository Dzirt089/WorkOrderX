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
		/// Создает новую заявку на ремонт оборудования
		/// </summary>
		/// <param name="id">Id</param>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Даиа создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>		
		/// <param name="completionAt">Дата завершения заявки</param>
		/// <param name="equipmentType">Тип оборудования</param>
		/// <param name="equipmentKind">Вид оборудования</param>
		/// <param name="equipmentModel">Модель оборудования</param>
		/// <param name="serialNumber">Серийный номер оборудования</param>
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
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			SerialNumber? serialNumber,
			TypeBreakdown typeBreakdown,
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
			EquipmentType = equipmentType;
			EquipmentKind = equipmentKind;
			EquipmentModel = equipmentModel;
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
		/// Создает новую заявку на ремонт оборудования
		/// </summary>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Дата создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>		
		/// <param name="equipmentType">Тип оборудования</param>
		/// <param name="equipmentKind">Вид оборудования</param>
		/// <param name="equipmentModel">Модель оборудования</param>
		/// <param name="serialNumber">Серийный номер оборудования</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Важность заявки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="executorEmployeeId">ID исполнителя заявки</param>
		/// <returns></returns>
		public static ProcessRequest Create(
			ApplicationNumber applicationNumber,
			ApplicationType applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			SerialNumber? serialNumber,
			TypeBreakdown typeBreakdown,
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
				equipmentType: equipmentType,
				equipmentKind: equipmentKind,
				equipmentModel: equipmentModel,
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
		/// Тип оборудования
		/// </summary>
		public EquipmentType? EquipmentType { get; private set; }

		/// <summary>
		/// Вид оборудования
		/// </summary>
		public EquipmentKind? EquipmentKind { get; private set; }

		/// <summary>
		/// Модель оборудования
		/// </summary>
		public EquipmentModel? EquipmentModel { get; private set; }

		/// <summary>
		/// Серийный номер
		/// </summary>
		public SerialNumber? SerialNumber { get; private set; }//TODO: Если пришлют список - сделать списком. Иначе строка

		/// <summary>
		/// Тип поломки
		/// </summary>
		public TypeBreakdown TypeBreakdown { get; private set; }

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
		/// Заказчик заявки (кто создал)
		/// </summary>
		public Guid CustomerEmployeeId { get; private set; }

		/// <summary>
		/// Исполнитель заявки (кому прислали на исполнение заявки)
		/// </summary>
		public Guid ExecutorEmployeeId { get; private set; }

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

			if (ExecutorEmployeeId == CustomerEmployeeId)
				throw new DomainException("Исполнитель не может быть заказчиком заявки");

			if (ExecutorEmployeeId == executorEmployeeId)
				throw new DomainException("Исполнитель не может быть переназначен на себя");

			if (applicationStatus.Id != ApplicationStatus.Redirected.Id)
				throw new DomainException("Статус заявки должен быть Redirected");

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
			InternalComment? internalComment = null)
		{
			ValidateRequestIsActive();

			if (ApplicationStatus.Id == ApplicationStatus.Returned.Id || ApplicationStatus.Id == ApplicationStatus.Postponed.Id)
				throw new DomainException("Уже возвращенная или отложенная заявка не может быть изменена");

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
			}
			else
			{
				throw new DomainException("Статус на возврат или отложение заявки должен быть Returned или Postponed");
			}
		}

		/// <summary>
		/// Обновление заявки
		/// </summary>
		/// <param name="equipmentType"></param>
		/// <param name="equipmentKind"></param>
		/// <param name="equipmentModel"></param>
		/// <param name="serialNumber"></param>
		/// <param name="typeBreakdown"></param>
		/// <param name="descriptionMalfunction"></param>
		/// <param name="applicationStatus"></param>
		/// <param name="internalComment"></param>
		/// <param name="customerEmployeeId"></param>
		/// <param name="executorEmployeeId"></param>
		public void Update(
			ApplicationType applicationType,
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			SerialNumber? serialNumber,
			TypeBreakdown typeBreakdown,
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
			EquipmentType = equipmentType;
			EquipmentKind = equipmentKind;
			EquipmentModel = equipmentModel;
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


		public int? EquipmentTypeId { get; private set; }
		public int? EquipmentKindId { get; private set; }
		public int? TypeBreakdownId { get; private set; }
		public int? EquipmentModelId { get; private set; }
		public int? ApplicationStatusId { get; private set; }
		public int? ApplicationTypeId { get; private set; }
		public int? ImportanceId { get; private set; }
	}
}