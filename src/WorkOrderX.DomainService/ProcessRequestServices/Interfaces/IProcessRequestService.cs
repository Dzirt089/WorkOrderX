using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.DomainService.ProcessRequestServices.Interfaces
{
	public interface IProcessRequestService
	{
		/// <summary>
		/// Создает заявку на хоз. работы.
		/// </summary>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Даиа создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>
		/// <param name="instrumentType">Тип инструмента</param>
		/// <param name="instrumentKind">Вид инструмента</param>
		/// <param name="instrumentModel">Модель инструмента</param>
		/// <param name="serialNumber">Серийный номер инструмента</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Уровень важности заявки</param>
		/// <param name="location">Местоположение поломки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		Task<ProcessRequest> CreateHouseholdChoresRequest(
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
			CancellationToken token);

		/// <summary>
		/// Обновляет заявку на хоз.работы.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Уровень важности заявки</param>
		/// <param name="location">Местоположение поломки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<ProcessRequest> UpdateHouseholdChoresRequest(
			ProcessRequest processRequest,
			ApplicationType applicationType,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Location? location,
			Guid customerEmployeeId,
			CancellationToken token);

		/// <summary>
		/// Создает заявку на ремонт инструмента.
		/// </summary>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Даиа создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>
		/// <param name="instrumentType">Тип инструмента</param>
		/// <param name="instrumentKind">Вид инструмента</param>
		/// <param name="instrumentModel">Модель инструмента</param>
		/// <param name="serialNumber">Серийный номер инструмента</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Уровень важности заявки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		Task<ProcessRequest> CreateInstrumentRepairRequest(
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
			CancellationToken token);

		/// <summary>
		/// Обновляет заявку на ремонт инструмента.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="instrumentType">Тип инструмента</param>
		/// <param name="instrumentKind">Вид инструмента</param>
		/// <param name="instrumentModel">Модель инструмента</param>
		/// <param name="serialNumber">Серийный номер инструмента</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="importance">Уровень важности заявки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<ProcessRequest> UpdateInstrumentRepairRequest(
			ProcessRequest processRequest,
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
			CancellationToken token);

		/// <summary>
		/// Установка завершенной или отклоненной заявки и статуса с комментарием
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
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
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <returns></returns>
		ProcessRequest SetStatusInWork(
			ProcessRequest processRequest,
			InternalComment? internalComment,
			ApplicationStatus applicationStatus);

		/// <summary>
		/// Проверяет, что исполнитель существует и может быть исполнителем, и устанавливает ID исполнителя заявки на ремонт инструмента или хоз. работы.
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
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
		/// Устанавливает статус заявки на ремонт инструмента или хоз. работы в "Возвращена заказчику" или "Отложена" с комментарием.
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <returns></returns>
		public ProcessRequest GetSetStatusReturnedOrPostponed(
			ProcessRequest processRequest,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			DateTime? plannedAt);

		/// <summary>
		/// Сохранение комментария специалиста (при нажатии кнопки сохранить коммент. в программе)
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
		/// <param name="internalComment">комментария специалиста</param>
		ProcessRequest UpdateInternalComment(
			ProcessRequest processRequest,
			InternalComment? internalComment);
	}
}
