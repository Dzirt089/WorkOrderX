using WorkOrderX.Domain.AggregationModels.Employees;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
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
		/// Проверяет, что исполнитель существует и может быть исполнителем, и устанавливает ID исполнителя заявки на ремонт оборудования или хоз. работы.
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

			if (employeeExecutor.Role != Role.Contractor && employeeExecutor.Role != Role.Admin)
				throw new DomainServiceException($"Employee with ID {executerEmployeeID} is not an executor.");

			var textDefault = InternalComment.Create($"Заявка перенаправлена Вам от {employeeCustomer.Name.Value}, с номером телефона {employeeCustomer.Phone.Value}");

			// Обновляем заявку
			processRequest.ReassignmentExecutorEmployeeId(
				executorEmployeeId: executerEmployeeID,
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
			ApplicationStatus applicationStatus)
		{
			// Обновляем заявку
			processRequest.SetStatusInWork(applicationStatus);
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
		public ProcessRequest SetRequestDoneOrRejectedAndStatusWithComment(
			ProcessRequest processRequest,
			DateTime completionAt,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment)
		{
			// Обновляем заявку
			processRequest.SetRequestDoneOrRejectedAndStatusWithComment(
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
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<ProcessRequest> UpdateProcessRequest(
			ProcessRequest processRequest,
			ApplicationType applicationType,
			EquipmentType? equipmentType,
			EquipmentKind? equipmentKind,
			EquipmentModel? equipmentModel,
			string? serialNumber,
			TypeBreakdown typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Guid customerEmployeeId,
			CancellationToken token)
		{
			var employeeCustomer = await _employeeRepository.GetByIdAsync(customerEmployeeId, token)
				?? throw new DomainServiceException($"Employee with ID {customerEmployeeId} not found.");

			if (processRequest.ApplicationStatus != ApplicationStatus.Returned)
				throw new DomainServiceException("Заявка должна быть в статусе 'Возвращена заказчику' для обновления.");

			if (applicationStatus != ApplicationStatus.Changed)
				throw new DomainServiceException("Статус заявки должен быть 'Изменена заказчиком после возврата' для обновления.");

			// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
			ValidateRequestTypeAndBreakdown(applicationType, equipmentType, equipmentKind, equipmentModel, typeBreakdown);

			// Проверяем, что заказчик существует и может быть заказчиком
			await ValidateCustomerAsync(customerEmployeeId, token);

			// Находим исполнителя по типу поломки
			Guid? executorEmployeeId = await GetEmployeeSpecializedAsync(typeBreakdown, processRequest.ApplicationType, token) ?? throw new DomainServiceException("Не найден исполнитель!");

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
			string? serialNumber,
			TypeBreakdown typeBreakdown,
			DescriptionMalfunction descriptionMalfunction,
			ApplicationStatus applicationStatus,
			InternalComment? internalComment,
			Guid customerEmployeeId,
			CancellationToken token)
		{
			// Проверяет тип заявки и тип поломки с оборудованием, чтобы убедиться, что они соответствуют друг другу.
			ValidateRequestTypeAndBreakdown(applicationType, equipmentType, equipmentKind, equipmentModel, typeBreakdown);

			if (applicationStatus != ApplicationStatus.New)
				throw new DomainServiceException("Статус заявки должен быть 'Новая' при создании заявки.");

			Guid? executorEmployeeId = await GetBreakdownAndExecutorAsync(applicationType, typeBreakdown, customerEmployeeId, token);

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
			if (employeeCustomer.Role != Role.Customer && employeeCustomer.Role != Role.Contractor && employeeCustomer.Role != Role.Admin)
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
		private static void ValidateRequestTypeAndBreakdown(ApplicationType applicationType, EquipmentType? equipmentType, EquipmentKind? equipmentKind, EquipmentModel? equipmentModel, TypeBreakdown typeBreakdown)
		{
			if (applicationType == ApplicationType.EquipmentRepair && typeBreakdown == TypeBreakdown.РouseholdСhores)
				throw new DomainServiceException("Для заявки на ремонт оборудования не может быть указан тип поломки 'Хоз. работы'.");

			if (applicationType == ApplicationType.HouseholdChores && typeBreakdown != TypeBreakdown.РouseholdСhores)
				throw new DomainServiceException("Для заявки на хоз. работы должен быть указан тип поломки 'Хоз. работы'.");

			if (applicationType == ApplicationType.EquipmentRepair && (equipmentType is null || equipmentKind is null || equipmentModel is null))
				throw new DomainServiceException("Для заявки на ремонт оборудования должны быть указаны тип, вид и модель оборудования.");

			// Для хозяйственных работ не должно быть оборудования
			if (applicationType == ApplicationType.HouseholdChores && (equipmentType != null || equipmentKind != null || equipmentModel != null))
				throw new DomainServiceException("Для хозяйственных работ не указывается оборудование");

		}

		/// <summary>
		/// По типу поломки и по типу заявки находит исполнителя
		/// </summary>
		/// <exception cref="DomainServiceException"></exception>
		private async Task<Guid?> GetEmployeeSpecializedAsync(TypeBreakdown type, ApplicationType applicationType, CancellationToken token)
		{
			if (type == TypeBreakdown.Electrical && applicationType == ApplicationType.EquipmentRepair)
				return await GetIdEmployeeBySpecializedAsync(Specialized.Electrician, token);

			if (type == TypeBreakdown.Mechanical && applicationType == ApplicationType.EquipmentRepair)
				return await GetIdEmployeeBySpecializedAsync(Specialized.Mechanic, token);

			if (type == TypeBreakdown.РouseholdСhores && applicationType == ApplicationType.HouseholdChores)
				return await GetIdEmployeeBySpecializedAsync(Specialized.Plumber, token);

			throw new DomainServiceException("Not type of breakdown.");
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
											?? throw new DomainServiceException("No electrician available for this request.");
			if (result.Role != Role.Contractor)
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
		private async Task<Guid?> GetBreakdownAndExecutorAsync(
			ApplicationType applicationType,
			TypeBreakdown typeBreakdown,
			Guid customerEmployeeId,
			CancellationToken token)
		{
			// Проверяем, что заказчик существует и может быть заказчиком
			await ValidateCustomerAsync(customerEmployeeId, token);

			// Находим исполнителя по типу поломки
			Guid? executorEmployeeId = await GetEmployeeSpecializedAsync(typeBreakdown, applicationType, token) ?? null;

			return executorEmployeeId;
		}
	}
}
