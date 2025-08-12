using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.Domain.Root.Exceptions;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.DomainService.ProcessRequestServices.Implementation
{
	public class ProcessRequestService(IWorkplaceEmployeesRepository employeeRepository) : IProcessRequestService
	{
		private readonly IWorkplaceEmployeesRepository _employeeRepository = employeeRepository;

		#region "Хоз. работы"

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
		public async Task<ProcessRequest> CreateHouseholdChoresRequest(
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
			CancellationToken token)
		{
			// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
			ValidateHouseholdChoresRequest(applicationType, location);

			if (applicationStatus.Id != ApplicationStatus.New.Id)
				throw new DomainServiceException("Статус заявки должен быть 'Новая' при создании заявки.");

			Guid executorEmployeeId = await GetBreakdownAndExecutorAsync(applicationType, null, customerEmployeeId, token);

			// Создаем новую заявку
			ProcessRequest? processRequest = ProcessRequest.CreateHouseholdChores(
				applicationNumber: applicationNumber,
				applicationType: applicationType,
				createdAt: createdAt,
				plannedAt: plannedAt,
				descriptionMalfunction: descriptionMalfunction,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				importance: importance,
				location: location,
				customerEmployeeId: customerEmployeeId,
				executorEmployeeId: executorEmployeeId);

			return processRequest;
		}

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
		public async Task<ProcessRequest> UpdateHouseholdChoresRequest(
			ProcessRequest processRequest,
			ApplicationType applicationType,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Location? location,
			Guid customerEmployeeId,
			CancellationToken token)
		{
			var employeeCustomer = await _employeeRepository.GetByIdAsync(customerEmployeeId, token)
				?? throw new DomainServiceException($"Employee with ID {customerEmployeeId} not found.");

			if (processRequest.ApplicationStatus.Id != ApplicationStatus.Returned.Id)
				throw new DomainServiceException("Заявка должна быть в статусе 'Возвращена заказчику' для обновления.");

			if (applicationStatus.Id != ApplicationStatus.Changed.Id)
				throw new DomainServiceException("Статус заявки должен быть 'Изменена заказчиком после возврата' для обновления.");

			// Валидация типа заявки и локации поломки
			ValidateHouseholdChoresRequest(applicationType, location);

			// Проверяем, что заказчик существует и может быть заказчиком
			await ValidateCustomerAsync(customerEmployeeId, token);

			// Находим исполнителя по типу поломки
			Guid executorEmployeeId = await GetEmployeeSpecializedAsync(null, processRequest.ApplicationType, token) ?? throw new DomainServiceException("Не найден исполнитель!");

			// Обновляем заявку
			processRequest.UpdateHouseholdChores(
				applicationType: applicationType,
				descriptionMalfunction: descriptionMalfunction,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				importance: importance,
				location: location,
				customerEmployeeId: customerEmployeeId,
				executorEmployeeId: executorEmployeeId);

			return processRequest;
		}


		/// <summary>
		/// Проверяет тип заявки c локацией поломки, чтобы убедиться, что они соответствуют друг другу.
		/// </summary>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="location">Местоположение поломки</param>
		/// <exception cref="DomainServiceException"></exception>
		private static void ValidateHouseholdChoresRequest(
			ApplicationType applicationType,
			Location? location)
		{
			// Для хозяйственных работ не должно быть инструмента
			if (applicationType.Id != ApplicationType.HouseholdChores.Id)
				throw new DomainServiceException($"Для хозяйственных работ необходимо указать тип заявки как 'Хоз. Работы'.");

			if (location is null && applicationType.Id == ApplicationType.HouseholdChores.Id)
				throw new DomainServiceException("Для хозяйственных работ необходимо указать местоположение поломки.");
		}
		#endregion

		#region "Ремонт инструмента"


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
		public async Task<ProcessRequest> CreateInstrumentRepairRequest(
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
			CancellationToken token)
		{
			// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
			ValidateInstrumentRepairRequest(applicationType, instrumentType,
				instrumentKind, instrumentModel, typeBreakdown);

			if (applicationStatus.Id != ApplicationStatus.New.Id)
				throw new DomainServiceException("Статус заявки должен быть 'Новая' при создании заявки на ремонт инструмента.");

			Guid executorEmployeeId = await GetBreakdownAndExecutorAsync(applicationType, typeBreakdown, customerEmployeeId, token);

			// Создаем новую заявку
			ProcessRequest? processRequest = ProcessRequest.CreateInstrumentRepair(
				applicationNumber: applicationNumber,
				applicationType: applicationType,
				createdAt: createdAt,
				plannedAt: plannedAt,
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

			return processRequest;
		}

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
		public async Task<ProcessRequest> UpdateInstrumentRepairRequest(
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
			CancellationToken token)
		{
			var employeeCustomer = await _employeeRepository.GetByIdAsync(customerEmployeeId, token)
				?? throw new DomainServiceException($"Employee with ID {customerEmployeeId} not found.");

			if (processRequest.ApplicationStatus.Id != ApplicationStatus.Returned.Id)
				throw new DomainServiceException("Заявка должна быть в статусе 'Возвращена заказчику' для обновления.");

			if (applicationStatus.Id != ApplicationStatus.Changed.Id)
				throw new DomainServiceException("Статус заявки должен быть 'Изменена заказчиком после возврата' для обновления.");

			// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
			ValidateInstrumentRepairRequest(applicationType, instrumentType,
				instrumentKind, instrumentModel, typeBreakdown);

			// Проверяем, что заказчик существует и может быть заказчиком
			await ValidateCustomerAsync(customerEmployeeId, token);

			// Находим исполнителя по типу поломки
			Guid executorEmployeeId = await GetEmployeeSpecializedAsync(typeBreakdown, processRequest.ApplicationType, token) ?? throw new DomainServiceException("Не найден исполнитель!");

			// Обновляем заявку
			processRequest.UpdateInstrumentRepair(
				applicationType: applicationType,
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

			return processRequest;
		}

		/// <summary>
		/// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
		/// </summary>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="instrumentType">Тип инструмента</param>
		/// <param name="instrumentKind">Вид инструмента</param>
		/// <param name="instrumentModel">Модель инструмента</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <exception cref="DomainServiceException"></exception>
		private static void ValidateInstrumentRepairRequest(
			ApplicationType applicationType,
			InstrumentType? instrumentType,
			InstrumentKind? instrumentKind,
			InstrumentModel? instrumentModel,
			TypeBreakdown? typeBreakdown)
		{

			if (applicationType.Id == ApplicationType.InstrumentRepair.Id &&
					(instrumentType is null || instrumentKind is null ||
					typeBreakdown is null || instrumentModel is null))
				throw new DomainServiceException($"Для заявки на ремонт инструмента должны быть указаны поломка, тип, вид и модель инструмента.");

			if (instrumentType != null && typeBreakdown != null && typeBreakdown.InstrumentType.Id != instrumentType.Id)
				throw new DomainServiceException($"Не совпадает Тип выбранного инструмента {instrumentType.Name} с типом выбранной поломки {typeBreakdown.Name}!");

			if (instrumentType != null && instrumentKind != null &&
					instrumentKind.InstrumentType.Id != instrumentType.Id)
				throw new DomainServiceException($"Не совпадает Вид выбранного инструмента {instrumentKind.Name} с типом выбранной инструмента {instrumentType.Name}!");

			if (applicationType.Id != ApplicationType.InstrumentRepair.Id)
				throw new DomainServiceException($"Для заявки на ремонт инструмента необходимо указать тип заявки как 'Ремонт инструмента'.");
		}
		#endregion

		#region Общие методы для заявок на ремонт инструмента и хоз. работы

		/// <summary>
		/// По типу поломки и по типу заявки находит исполнителя
		/// </summary>
		/// <exception cref="DomainServiceException"></exception>
		private async Task<Guid?> GetEmployeeSpecializedAsync(
			TypeBreakdown? type,
			ApplicationType applicationType,
			CancellationToken token)
		{
			if (type != null &&
				type.InstrumentType.Id == InstrumentType.ElectricInstrument.Id &&
				applicationType.Id == ApplicationType.InstrumentRepair.Id)

				return await GetIdEmployeeBySpecializedAsync(Specialized.Electrician, token);

			if (type != null &&
				type.InstrumentType.Id != InstrumentType.ElectricInstrument.Id &&
				applicationType.Id == ApplicationType.InstrumentRepair.Id)

				return await GetIdEmployeeBySpecializedAsync(Specialized.Mechanic, token);

			if (type is null &&
				applicationType.Id == ApplicationType.HouseholdChores.Id)

				return await GetIdEmployeeBySpecializedAsync(Specialized.SupplyManager, token);

			throw new DomainServiceException("Исполнитель не определен для заявки");
		}

		/// <summary>
		/// Получает ID исполнителя по типу его специальности.
		/// </summary>
		/// <param name="specialized"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		private async Task<Guid> GetIdEmployeeBySpecializedAsync(Specialized specialized, CancellationToken token)
		{
			var result = await _employeeRepository.GetBySpecializedAsync(specialized, token)
											?? throw new DomainServiceException("Не найден исполнитель по специальности.");
			if (result.Role.Id != Role.Executer.Id)
				throw new DomainServiceException($"Employee with ID {result.Id} is not an executor.");

			return result.Id;
		}

		/// <summary>
		/// Проверяет заказчика и получает тип поломки и исполнителя.
		/// </summary>
		/// <param name="applicationType">Номер заявки</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		private async Task<Guid> GetBreakdownAndExecutorAsync(
			ApplicationType applicationType,
			TypeBreakdown? typeBreakdown,
			Guid customerEmployeeId,
			CancellationToken token)
		{
			// Проверяем, что заказчик существует и может быть заказчиком
			await ValidateCustomerAsync(customerEmployeeId, token);

			// Находим исполнителя по типу поломки
			Guid executorEmployeeId = await GetEmployeeSpecializedAsync(typeBreakdown, applicationType, token)
				?? throw new DomainException("Не найден исполнитель!");

			return executorEmployeeId;
		}

		/// <summary>
		/// Проверяет, что заказчик существует и может быть заказчиком.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		private async Task ValidateCustomerAsync(Guid customerEmployeeId, CancellationToken token)
		{
			// Проверяем, что customerEmployeeId существует и может являться заказчиком
			var employeeCustomer = await _employeeRepository.GetByIdAsync(customerEmployeeId, token)
				?? throw new DomainServiceException($"Employee with ID {customerEmployeeId} not found.");

			// Проверяем, что сотрудник имеет возможность создать заявку имея роль: заказчик, исполнитель или администратор.
			if (!(employeeCustomer.Role.Id == Role.Customer.Id || employeeCustomer.Role.Id == Role.Executer.Id || employeeCustomer.Role.Id == Role.Manager.Id || employeeCustomer.Role.Id == Role.Admin.Id))
				throw new DomainServiceException($"Employee with ID {customerEmployeeId} is not a customer.");
		}

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
			DateTime? plannedAt)
		{
			// Обновляем заявку
			processRequest.SetStatusReturnedOrPostponed(
				applicationStatus: applicationStatus,
				plannedAt: plannedAt,
				internalComment: internalComment);
			return processRequest;
		}


		/// <summary>
		/// Перенаправление заявки другому исполнителю с проверкой, что исполнитель существует и может быть исполнителем, и устанавливает ID исполнителя заявки на ремонт инструмента или хоз. работы.
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
		/// <param name="executerEmployeeID">ID исполнителя заявки</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий к заявке при перенаправлении. По умолчанию сделать в доменном сервисе индентификацию, кто из исполнителей кому перенаправил, если комментарий был пустым.</param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		public async Task<ProcessRequest> GetReassignmentExecutorEmployeeIdAsync(
			ProcessRequest processRequest,
			Guid executerEmployeeID,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			CancellationToken token)
		{
			var employeeCustomer = await _employeeRepository.GetByIdAsync(processRequest.CustomerEmployeeId, token)
				?? throw new DomainServiceException($"Employee with ID {processRequest.CustomerEmployeeId} not found.");

			// Проверяем, что исполнитель существует и может быть исполнителем
			var employeeExecutor = await _employeeRepository.GetByIdAsync(executerEmployeeID, token)
				?? throw new DomainServiceException($"Employee with ID {executerEmployeeID} not found.");

			if (employeeExecutor.Role.Id != Role.Executer.Id)
				throw new DomainServiceException($"Employee with ID {executerEmployeeID} is not an executor.");

			// Обновляем заявку
			processRequest.ReassignmentExecutorEmployeeId(
				executorEmployeeId: employeeExecutor.Id,
				applicationStatus: applicationStatus,
				internalComment: internalComment);
			return processRequest;
		}


		/// <summary>
		/// Установка статуса заявки в работу
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <returns></returns>
		public ProcessRequest SetStatusInWork(
			ProcessRequest processRequest,
			InternalComment? internalComment,
			ApplicationStatus applicationStatus)
		{
			// Обновляем заявку
			processRequest.SetStatusInWork(applicationStatus, internalComment);
			return processRequest;
		}


		/// <summary>
		/// Сохранение комментария специалиста (при нажатии кнопки сохранить коммент. в программе)
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
		/// <param name="internalComment">комментария специалиста</param>
		/// <returns></returns>
		public ProcessRequest UpdateInternalComment(
			ProcessRequest processRequest,
			InternalComment? internalComment)
		{
			// Обновляем заявку
			processRequest.UpdateInternalComment(internalComment);
			return processRequest;
		}

		/// <summary>
		/// Установка завершенной или отклоненной заявки и статуса с комментарием
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт инструмента или хоз. работы</param>
		/// <param name="completionAt">Дата завершения заявки</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <returns></returns>
		public ProcessRequest SetRequestDoneOrRejected(
			ProcessRequest processRequest,
			DateTime completionAt,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment)
		{
			// Обновляем заявку
			processRequest.SetRequestDoneOrRejected(
				completionAt: completionAt,
				applicationStatus: applicationStatus,
				internalComment: internalComment);
			return processRequest;
		}
		#endregion

	}
}
