using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.Domain.Root.Exceptions;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.DomainService.ProcessRequestServices.Implementation
{
	public class ProcessRequestService(IWorkplaceEmployeesRepository employeeRepository) : IProcessRequestService
	{
		private readonly IWorkplaceEmployeesRepository _employeeRepository = employeeRepository;

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
			InternalComment? internalComment)
		{
			// Обновляем заявку
			processRequest.SetStatusReturnedOrPostponed(
				applicationStatus: applicationStatus,
				internalComment: internalComment);
			return processRequest;
		}


		/// <summary>
		/// Перенаправление заявки другому исполнителю с проверкой, что исполнитель существует и может быть исполнителем, и устанавливает ID исполнителя заявки на ремонт оборудования или хоз. работы.
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
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

			var textDefault = InternalComment.Create($"Заявка перенаправлена Вам от {employeeCustomer.Name.Value}, с номером телефона {employeeCustomer.Phone.Value}");

			// Обновляем заявку
			processRequest.ReassignmentExecutorEmployeeId(
				executorEmployeeId: employeeExecutor.Id,
				applicationStatus: applicationStatus,
				internalComment: internalComment is null ? textDefault : internalComment);
			return processRequest;
		}


		/// <summary>
		/// Установка статуса заявки в работу
		/// </summary>
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
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
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
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
		/// <param name="processRequest">Заявка на ремонт оборудования или хоз. работы</param>
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
		/// <param name="importance">Уровень важности заявки</param>
		/// <param name="location">Местоположение поломки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<ProcessRequest> UpdateProcessRequest(
			ProcessRequest processRequest,
			ApplicationType applicationType,
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			SerialNumber? serialNumber,
			TypeBreakdown? typeBreakdown,
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

			// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
			ValidateRequestTypeAndBreakdown(applicationType, equipmentType,
				equipmentKind, equipmentModel, typeBreakdown, location);

			// Проверяем, что заказчик существует и может быть заказчиком
			await ValidateCustomerAsync(customerEmployeeId, token);

			// Находим исполнителя по типу поломки
			Guid executorEmployeeId = await GetEmployeeSpecializedAsync(typeBreakdown, processRequest.ApplicationType, token) ?? throw new DomainServiceException("Не найден исполнитель!");

			// Обновляем заявку
			processRequest.Update(
				applicationType: applicationType,
				equipmentType: equipmentType,
				equipmentKind: equipmentKind,
				equipmentModel: equipmentModel,
				serialNumber: serialNumber,
				typeBreakdown: typeBreakdown,
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
		/// <param name="importance">Уровень важности заявки</param>
		/// <param name="location">Местоположение поломки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainServiceException"></exception>
		public async Task<ProcessRequest> CreateProcessRequest(
			ApplicationNumber applicationNumber,
			ApplicationType applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			SerialNumber? serialNumber,
			TypeBreakdown? typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Importance importance,
			Location? location,
			Guid customerEmployeeId,
			CancellationToken token)
		{
			// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
			ValidateRequestTypeAndBreakdown(applicationType, equipmentType,
				equipmentKind, equipmentModel, typeBreakdown, location);

			if (applicationStatus.Id != ApplicationStatus.New.Id)
				throw new DomainServiceException("Статус заявки должен быть 'Новая' при создании заявки.");

			Guid executorEmployeeId = await GetBreakdownAndExecutorAsync(applicationType, typeBreakdown, customerEmployeeId, token);

			// Создаем новую заявку
			ProcessRequest? processRequest = ProcessRequest.Create(
				applicationNumber: applicationNumber,
				applicationType: applicationType,
				createdAt: createdAt,
				plannedAt: plannedAt,
				equipmentType: equipmentType,
				equipmentKind: equipmentKind,
				equipmentModel: equipmentModel,
				serialNumber: serialNumber,
				typeBreakdown: typeBreakdown,
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
			if (!(employeeCustomer.Role.Id == Role.Customer.Id || employeeCustomer.Role.Id == Role.Executer.Id || employeeCustomer.Role.Id == Role.Admin.Id))
				throw new DomainServiceException($"Employee with ID {customerEmployeeId} is not a customer.");
		}

		/// <summary>
		/// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
		/// </summary>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="equipmentType">Тип оборудования</param>
		/// <param name="equipmentKind">Вид оборудования</param>
		/// <param name="equipmentModel">Модель оборудования</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <exception cref="DomainServiceException"></exception>
		private static void ValidateRequestTypeAndBreakdown(
			ApplicationType applicationType,
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			TypeBreakdown? typeBreakdown,
			Location? location)
		{

			if (applicationType.Id == ApplicationType.EquipmentRepair.Id &&
					(equipmentType is null || equipmentKind is null ||
					typeBreakdown is null || equipmentModel is null))
				throw new DomainServiceException($"Для заявки на ремонт оборудования должны быть указаны поломка, тип, вид и модель оборудования.");

			// Для хозяйственных работ не должно быть оборудования
			if (applicationType.Id == ApplicationType.HouseholdChores.Id &&
					(equipmentType != null || equipmentKind != null ||
					typeBreakdown != null || equipmentModel != null))
				throw new DomainServiceException($"Для хозяйственных работ не указывается оборудование");

			if (equipmentType != null && typeBreakdown != null &&
					typeBreakdown.EquipmentType.Id != equipmentType.Id)
				throw new DomainServiceException($"Не совпадает Тип выбранного оборудования {equipmentType.Name} с типом выбранной поломки {typeBreakdown.Name}!");

			if (equipmentType != null && equipmentKind != null &&
					equipmentKind.EquipmentType.Id != equipmentType.Id)
				throw new DomainServiceException($"Не совпадает Вид выбранного оборудования {equipmentKind.Name} с типом выбранной оборудования {equipmentType.Name}!");

			if (location is null && applicationType.Id == ApplicationType.HouseholdChores.Id)
				throw new DomainServiceException("Для хозяйственных работ необходимо указать местоположение поломки.");
		}

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
				type.EquipmentType.Id == EquipmentType.ElectricInstrument.Id &&
				applicationType.Id == ApplicationType.EquipmentRepair.Id)

				return await GetIdEmployeeBySpecializedAsync(Specialized.Electrician, token);

			if (type != null &&
				type.EquipmentType.Id != EquipmentType.ElectricInstrument.Id &&
				applicationType.Id == ApplicationType.EquipmentRepair.Id)

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
	}
}
