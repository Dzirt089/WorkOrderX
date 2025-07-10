using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.DomainService.ProcessRequestServices.Interfaces
{
	public interface IProcessRequestService
	{
		/// <summary>
		/// Создает заявку на ремонт оборудования или хоз. работы в зависимости от типа заявки.
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
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		Task<ProcessRequest> CreateProcessRequest(
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
			Guid customerEmployeeId,
			CancellationToken token);

		/// <summary>
		/// Обновляет заявку на ремонт оборудования или хоз. работы в зависимости от типа заявки.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="equipmentType">Тип оборудования</param>
		/// <param name="equipmentKind">Вид оборудования</param>
		/// <param name="equipmentModel">Модель оборудования</param>
		/// <param name="serialNumber">Серийный номер оборудования</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<ProcessRequest> UpdateProcessRequest(
			ProcessRequest processRequest,
			ApplicationType applicationType,
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			SerialNumber? serialNumber,
			TypeBreakdown typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Guid customerEmployeeId,
			CancellationToken token);

		/// <summary>
		/// Установка завершенной или отклоненной заявки и статуса с комментарием
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
		/// <param name="completionAt">Дата завершения заявки</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <returns></returns>
		ProcessRequest SetRequestDoneOrRejected(
		   ProcessRequest processRequest,
		   DateTime completionAt,
		   ApplicationStatus applicationStatus,
		   InternalComment? internalComment);

		/// <summary>
		/// Установка статуса заявки в работу
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <returns></returns>
		ProcessRequest SetStatusInWork(
			ProcessRequest processRequest,
			InternalComment? internalComment,
			ApplicationStatus applicationStatus);

		/// <summary>
		/// Проверяет, что исполнитель существует и может быть исполнителем, и устанавливает ID исполнителя заявки на ремонт оборудования или хоз. работы.
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
		/// <param name="executerEmployeeID">ID исполнителя заявки</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий к заявке при перенаправлении. По умолчанию сделать в доменном сервисе индентификацию, кто из исполнителей кому перенаправил, если комментарий был пустым.</param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		Task<ProcessRequest> GetReassignmentExecutorEmployeeIdAsync(
			ProcessRequest processRequest,
			Guid executerEmployeeID,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			CancellationToken token);

		/// <summary>
		/// Устанавливает статус заявки на ремонт оборудования или хоз. работы в "Возвращена заказчику" или "Отложена" с комментарием.
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <returns></returns>
		public ProcessRequest GetSetStatusReturnedOrPostponed(
			ProcessRequest processRequest,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment);
	}
}
