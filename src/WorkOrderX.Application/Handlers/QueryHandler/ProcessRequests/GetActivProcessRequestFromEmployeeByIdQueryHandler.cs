using MediatR;

using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.Application.Handlers.QueryHandler.ProcessRequests
{
	/// <summary>
	/// Обработчик запроса для получения активных заявок от клиента по идентификатору сотрудника.
	/// </summary>
	public sealed class GetActivProcessRequestFromEmployeeByIdQueryHandler : IRequestHandler<GetActivProcessRequestFromEmployeeByIdQuery, GetActivProcessRequestFromEmployeeByIdQueryResponse>
	{
		private readonly IProcessRequestRepository _processRequestRepository;
		private readonly IWorkplaceEmployeesRepository _workplaceEmployeesRepository;

		/// <summary>
		/// Конструктор обработчика запроса для получения активных заявок от клиента по идентификатору сотрудника.
		/// </summary>
		/// <param name="processRequestRepository"></param>
		/// <param name="workplaceEmployeesRepository"></param>
		public GetActivProcessRequestFromEmployeeByIdQueryHandler(
			IProcessRequestRepository processRequestRepository,
			IWorkplaceEmployeesRepository workplaceEmployeesRepository)
		{
			_processRequestRepository = processRequestRepository;
			_workplaceEmployeesRepository = workplaceEmployeesRepository;
		}

		/// <summary>
		/// Обработчик запроса для получения активных заявок от клиента по идентификатору сотрудника.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<GetActivProcessRequestFromEmployeeByIdQueryResponse>
			Handle(GetActivProcessRequestFromEmployeeByIdQuery request,
			CancellationToken cancellationToken)
		{
			if (request.Id == Guid.Empty)
				throw new ApplicationException($"Идентификатор сотрудника не может быть пустым {nameof(request.Id)}.");

			var employee = await _workplaceEmployeesRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Сотрудник с идентификатором {request.Id} не найден.");

			return employee.Role.Name switch
			{
				nameof(Role.Customer) or nameof(Role.Manager) => await GetCustomerActiveProcessRequestsAsync(employee, cancellationToken),
				nameof(Role.Executer) => await GetContractorActiveProcessRequestsAsync(employee, cancellationToken),
				nameof(Role.Admin) or nameof(Role.Supervisor) => await GetAdminOrSupervisorActiveProcessRequestsAsync(employee, cancellationToken),
				_ => throw new ApplicationException($"Сотрудник с идентификатором {request.Id} не является менеджером, заказчиком, исполнителем или администратором."),
			};
		}

		/// <summary>
		/// Получает активные заявки от администратора или супервизора по идентификатору сотрудника.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<GetActivProcessRequestFromEmployeeByIdQueryResponse>
			GetAdminOrSupervisorActiveProcessRequestsAsync(
			WorkplaceEmployee employee,
			CancellationToken cancellationToken)
		{
			// Получаем активные заявки для администратора или супервизора
			IEnumerable<ProcessRequest>? processRequests = await _processRequestRepository
				.GetAdminActiveProcessRequestByEmloyeeId(employee.Id, cancellationToken)
			?? throw new ApplicationException($"Активные заявки для сотрудника с идентификатором {employee.Id} не найдены.");

			if (!processRequests.Any()) return new GetActivProcessRequestFromEmployeeByIdQueryResponse();

			// Получаем исполнителей и заказчиков по активным заявкам
			IEnumerable<WorkplaceEmployee> executors = await GetExecutorsByProcessRequestsAsync(processRequests, cancellationToken);
			IEnumerable<WorkplaceEmployee> customers = await GetCustomersByProcessRequestsAsync(processRequests, cancellationToken);

			if (!customers.Any() || !executors.Any())
				return new();
			else
				return BuildProcessRequestResponse(processRequests, customers, executors);
		}

		/// <summary>
		/// Получает активные заявки от исполнителя по идентификатору сотрудника.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<GetActivProcessRequestFromEmployeeByIdQueryResponse>
			GetContractorActiveProcessRequestsAsync(
			WorkplaceEmployee employee,
			CancellationToken cancellationToken)
		{
			// Получаем активные заявки для исполнителя
			IEnumerable<ProcessRequest> processRequests = await _processRequestRepository.GetExecutorActiveProcessRequestByEmloyeeId(employee.Id, cancellationToken)
				?? throw new ApplicationException($"Активные заявки для сотрудника с идентификатором {employee.Id} не найдены.");

			if (!processRequests.Any()) return new GetActivProcessRequestFromEmployeeByIdQueryResponse();

			// Получаем заказчиков по активным заявкам
			IEnumerable<WorkplaceEmployee> customers = await GetCustomersByProcessRequestsAsync(processRequests, cancellationToken);

			if (!customers.Any())
				return new();
			else
				return BuildProcessRequestResponse(processRequests, customers, new List<WorkplaceEmployee> { employee });
		}

		/// <summary>
		/// Получает активные заявки от заказчика по идентификатору сотрудника.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<GetActivProcessRequestFromEmployeeByIdQueryResponse>
			GetCustomerActiveProcessRequestsAsync(
			WorkplaceEmployee employee,
			CancellationToken cancellationToken)
		{
			// Получаем активные заявки для заказчика
			IEnumerable<ProcessRequest> processRequests = await _processRequestRepository
				.GetCustomerActiveProcessRequestByEmloyeeId(employee.Id, cancellationToken)
				?? throw new ApplicationException($"Активные заявки для сотрудника с идентификатором {employee.Id} не найдены.");

			if (!processRequests.Any()) return new GetActivProcessRequestFromEmployeeByIdQueryResponse();

			// Получаем исполнителей по активным заявкам
			IEnumerable<WorkplaceEmployee> executors = await GetExecutorsByProcessRequestsAsync(processRequests, cancellationToken);

			if (!executors.Any())
				return new();
			else
				return BuildProcessRequestResponse(processRequests, new List<WorkplaceEmployee> { employee }, executors);
		}

		/// <summary>
		/// Строит ответ для запроса активных заявок от заказчика по идентификатору.
		/// </summary>
		/// <param name="processRequests"></param>
		/// <param name="customers"></param>
		/// <param name="executors"></param>
		/// <returns></returns>
		private static GetActivProcessRequestFromEmployeeByIdQueryResponse
			BuildProcessRequestResponse(
			IEnumerable<ProcessRequest> processRequests,
			IEnumerable<WorkplaceEmployee> customers,
			IEnumerable<WorkplaceEmployee> executors)
		{
			// Создаем словари для быстрого доступа к заказчикам и исполнителям по их идентификаторам
			var customerDict = customers.ToDictionary(x => x.Id);
			var executorDict = executors.ToDictionary(x => x.Id);

			// Формируем ответ с данными заявок
			return new GetActivProcessRequestFromEmployeeByIdQueryResponse
			{
				ProcessRequests = processRequests.Select(x => new Models.DTOs.ProcessRequestDataDto
				{
					Id = x.Id,
					ApplicationNumber = x.ApplicationNumber.Value,
					ApplicationType = x.ApplicationType.Name,
					CreatedAt = x.CreatedAt.ToString(),
					PlannedAt = x.PlannedAt.ToString(),
					UpdatedAt = x.UpdatedAt.ToString(),
					CompletionAt = x.CompletionAt?.ToString(),
					EquipmentType = x.EquipmentType?.Name,
					EquipmentKind = x.EquipmentKind?.Name,
					EquipmentModel = x.EquipmentModel?.Name,
					SerialNumber = x.SerialNumber?.Value,
					TypeBreakdown = x.TypeBreakdown?.Name,
					DescriptionMalfunction = x.DescriptionMalfunction.Value,
					ApplicationStatus = x.ApplicationStatus.Name,
					InternalComment = x.InternalComment?.Value,
					Importance = x.Importance.Name,
					Location = x.Location?.Value,

					CustomerEmployee = new Models.DTOs.EmployeeDataDto
					{
						Id = customerDict[x.CustomerEmployeeId].Id,
						Name = customerDict[x.CustomerEmployeeId].Name.Value,
						Department = customerDict[x.CustomerEmployeeId].Department.Value,
						Email = customerDict[x.CustomerEmployeeId].Email.Value,
						Phone = customerDict[x.CustomerEmployeeId].Phone.Value,
						Specialized = customerDict[x.CustomerEmployeeId].Specialized?.Name ?? string.Empty,
						Account = customerDict[x.CustomerEmployeeId].Account.Value,
						Role = customerDict[x.CustomerEmployeeId].Role.Name
					},

					ExecutorEmployee = new Models.DTOs.EmployeeDataDto
					{
						Id = executorDict[x.ExecutorEmployeeId].Id,
						Name = executorDict[x.ExecutorEmployeeId].Name.Value,
						Department = executorDict[x.ExecutorEmployeeId].Department.Value,
						Email = executorDict[x.ExecutorEmployeeId].Email.Value,
						Phone = executorDict[x.ExecutorEmployeeId].Phone.Value,
						Specialized = executorDict[x.ExecutorEmployeeId].Specialized?.Name ?? string.Empty,
						Account = executorDict[x.ExecutorEmployeeId].Account.Value,
						Role = executorDict[x.ExecutorEmployeeId].Role.Name
					}
				})
			};
		}

		/// <summary>
		/// Получает заказчиков по активным заявкам.
		/// </summary>
		/// <param name="processRequests"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<IEnumerable<WorkplaceEmployee>> GetCustomersByProcessRequestsAsync(
			IEnumerable<ProcessRequest> processRequests,
			CancellationToken cancellationToken)
		{
			// Получаем уникальные идентификаторы заказчиков из активных заявок
			var idsCustomers = processRequests.Distinct().Select(x => x.CustomerEmployeeId).ToList() ??
				throw new ApplicationException($"Не найдены идентификаторы заказчиков");

			if (idsCustomers.Count == 0)
				throw new ApplicationException($"Идентификаторы заказчиков пусты");

			// Получаем заказчиков по их идентификаторам
			IEnumerable<WorkplaceEmployee> customers = await _workplaceEmployeesRepository.GetByIdsAsync(idsCustomers, cancellationToken)
				?? throw new ApplicationException($"Null или Данные заказчиков по их идентификаторам не найдены в БД.");

			return customers;
		}

		/// <summary>
		/// Получает исполнителей по активным заявкам.
		/// </summary>
		/// <param name="processRequests"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<IEnumerable<WorkplaceEmployee>> GetExecutorsByProcessRequestsAsync(
			IEnumerable<ProcessRequest> processRequests,
			CancellationToken cancellationToken)
		{
			// Получаем уникальные идентификаторы исполнителей из активных заявок
			var idsExecutors = processRequests.Distinct().Select(x => x.ExecutorEmployeeId).ToList()
				?? throw new ApplicationException($"Не найдены идентификаторы исполнителей");

			if (idsExecutors.Count == 0)
				throw new ApplicationException($"Идентификаторы исполнителей пусты");

			// Получаем исполнителей по их идентификаторам
			IEnumerable<WorkplaceEmployee> executors = await _workplaceEmployeesRepository.GetByIdsAsync(idsExecutors, cancellationToken)
				?? throw new ApplicationException($"Null или Данные исполнителей по их идентификаторам не найдены в БД.");

			return executors;
		}
	}
}
