using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Заявка
	/// </summary>
	public class ProcessRequest : Entity, IAggregationRoot
	{

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
			string? serialNumber,
			TypeBreakdown typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Guid customerEmployeeId,
			Guid? executorEmployeeId)
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
			CustomerEmployeeId = customerEmployeeId;
			ExecutorEmployeeId = executorEmployeeId;
		}

		/// <summary>
		/// Создает новую заявку на ремонт оборудования
		/// </summary>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Даиа создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>
		/// <param name="equipmentType">Тип оборудования</param>
		/// <param name="equipmentKind">Вид оборудования</param>
		/// <param name="equipmentModel">Модель оборудования</param>
		/// <param name="serialNumber">Серийный номер оборудования</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
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
			string? serialNumber,
			TypeBreakdown typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Guid customerEmployeeId,
			Guid? executorEmployeeId)
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
				customerEmployeeId: customerEmployeeId,
				executorEmployeeId: executorEmployeeId);

			return newProcessRequest;
		}



		/// <summary>
		/// Номер заявки
		/// </summary>
		public ApplicationNumber ApplicationNumber { get; }

		/// <summary>
		/// Тип заявки
		/// </summary>
		public ApplicationType ApplicationType { get; }

		/// <summary>
		/// Дата создания заявки
		/// </summary>
		public DateTime CreatedAt { get; }

		/// <summary>
		/// Дата завершения заявки
		/// </summary>
		public DateTime? CompletionAt { get; private set; }

		/// <summary>
		/// Плановая дата завершения
		/// </summary>
		public DateTime PlannedAt { get; }

		/// <summary>
		/// Тип оборудования
		/// </summary>
		public EquipmentType? EquipmentType { get; }

		/// <summary>
		/// Вид оборудования
		/// </summary>
		public EquipmentKind? EquipmentKind { get; }

		/// <summary>
		/// Модель оборудования
		/// </summary>
		public EquipmentModel? EquipmentModel { get; }

		/// <summary>
		/// Серийный номер
		/// </summary>
		public string? SerialNumber { get; }//TODO: Если пришлют список - сделать списком. Иначе строка

		/// <summary>
		/// Тип поломки
		/// </summary>
		public TypeBreakdown TypeBreakdown { get; }

		/// <summary>
		/// Описание неисправности
		/// </summary>
		public DescriptionMalfunction DescriptionMalfunction { get; }

		/// <summary>
		/// Статус заявки
		/// </summary>
		public ApplicationStatus ApplicationStatus { get; }

		/// <summary>
		/// Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.
		/// </summary>
		public InternalComment? InternalComment { get; }

		/// <summary>
		/// Заказчик заявки (кто создал)
		/// </summary>
		public Guid CustomerEmployeeId { get; }

		/// <summary>
		/// Исполнитель заявки (кому прислали на исполнение заявки)
		/// </summary>
		public Guid? ExecutorEmployeeId { get; }
	}
}
