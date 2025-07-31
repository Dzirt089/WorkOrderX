using MediatR;

using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.Application.Handlers.QueryHandler.ProcessRequests
{
	/// <summary>
	/// Обработчик запроса для получения историй заявок от клиента по идентификатору сотрудника.
	/// </summary>
	public sealed class GetHistoryProcessRequestFromEmployeeByIdQueryHandler :
			IRequestHandler<GetHistoryProcessRequestFromEmployeeByIdQuery,
							GetHistoryProcessRequestFromEmployeeByIdQueryResponse>
	{
		private readonly IProcessRequestRepository _processRequestRepository;
		private readonly IWorkplaceEmployeesRepository _workplaceEmployeesRepository;

		/// <summary>
		/// Конструктор обработчика запроса для получения историй заявок от клиента по идентификатору сотрудника.
		/// </summary>
		/// <param name="processRequestRepository"></param>
		/// <param name="workplaceEmployeesRepository"></param>
		public GetHistoryProcessRequestFromEmployeeByIdQueryHandler(
			IProcessRequestRepository processRequestRepository,
			IWorkplaceEmployeesRepository workplaceEmployeesRepository)
		{
			_processRequestRepository = processRequestRepository;
			_workplaceEmployeesRepository = workplaceEmployeesRepository;
		}

		public async Task<GetHistoryProcessRequestFromEmployeeByIdQueryResponse> Handle(GetHistoryProcessRequestFromEmployeeByIdQuery request, CancellationToken cancellationToken)
		{
			if (request.Id == Guid.Empty)
				throw new ApplicationException($"Идентификатор сотрудника не может быть пустым {nameof(request.Id)}.");

			var employee = await _workplaceEmployeesRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Сотрудник с идентификатором {request.Id} не найден.");

			return employee.Role.Name switch
			{
				nameof(Role.Customer) => await GetCustomerHistoryProcessRequestsAsync(employee, cancellationToken),
				nameof(Role.Executer) => await GetContractorHistoryProcessRequestsAsync(employee, cancellationToken),
				nameof(Role.Admin) or nameof(Role.Supervisor) => await GetAdminOrSupervisorHistoryProcessRequestsAsync(employee, cancellationToken),
				_ => throw new ApplicationException($"Сотрудник с идентификатором {request.Id} не является заказчиком, исполнителем или администратором."),
			};
		}

		/// <summary>
		/// Получает историю заявок от администратора или супервизора по идентификатору сотрудника.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<GetHistoryProcessRequestFromEmployeeByIdQueryResponse>
			GetAdminOrSupervisorHistoryProcessRequestsAsync(
			WorkplaceEmployee employee,
			CancellationToken cancellationToken)
		{
			// Получает историю заявок для администратора или супервизора
			IEnumerable<ProcessRequest>? processRequests = await _processRequestRepository
				.GetAdminHistoryProcessRequestByEmloyeeId(employee.Id, cancellationToken)
			?? throw new ApplicationException($"Активные заявки для сотрудника с идентификатором {employee.Id} не найдены.");

			if (!processRequests.Any()) return new GetHistoryProcessRequestFromEmployeeByIdQueryResponse();

			// Получаем исполнителей и заказчиков по истории заявок
			IEnumerable<WorkplaceEmployee> executors = await GetExecutorsByProcessRequestsAsync(processRequests, cancellationToken);
			IEnumerable<WorkplaceEmployee> customers = await GetCustomersByProcessRequestsAsync(processRequests, cancellationToken);

			if (!customers.Any() || !executors.Any())
				return new();
			else
				return BuildProcessRequestResponse(processRequests, customers, executors);
		}

		/// <summary>
		/// Получает историю заявок от исполнителя по идентификатору сотрудника.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<GetHistoryProcessRequestFromEmployeeByIdQueryResponse>
			GetContractorHistoryProcessRequestsAsync(
			WorkplaceEmployee employee,
			CancellationToken cancellationToken)
		{
			// Получаем истории заявок для исполнителя
			IEnumerable<ProcessRequest> processRequests = await _processRequestRepository.GetExecutorHistoryProcessRequestByEmloyeeId(employee.Id, cancellationToken)
				?? throw new ApplicationException($"Активные заявки для сотрудника с идентификатором {employee.Id} не найдены.");

			if (!processRequests.Any()) return new GetHistoryProcessRequestFromEmployeeByIdQueryResponse();

			// Получаем заказчиков по историям заявок.
			IEnumerable<WorkplaceEmployee> customers = await GetCustomersByProcessRequestsAsync(processRequests, cancellationToken);

			if (!customers.Any())
				return new();
			else
				return BuildProcessRequestResponse(processRequests, customers, new List<WorkplaceEmployee> { employee });
		}

		/// <summary>
		/// Получает заказчиков по истории заявок.
		/// </summary>
		/// <param name="processRequests"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<IEnumerable<WorkplaceEmployee>> GetCustomersByProcessRequestsAsync(
			IEnumerable<ProcessRequest> processRequests,
			CancellationToken cancellationToken)
		{
			// Получаем уникальные идентификаторы заказчиков по истории заявок
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
		/// Получает истории заявок от заказчика по идентификатору сотрудника.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<GetHistoryProcessRequestFromEmployeeByIdQueryResponse>
			GetCustomerHistoryProcessRequestsAsync(
			WorkplaceEmployee employee,
			CancellationToken cancellationToken)
		{
			// Получаем истории заявок для заказчика.
			IEnumerable<ProcessRequest> processRequests = await _processRequestRepository
				.GetCustomerHistoryProcessRequestByEmloyeeId(employee.Id, cancellationToken)
				?? throw new ApplicationException($"Истории заявок для сотрудника с идентификатором {employee.Id} не найдены.");

			if (!processRequests.Any()) return new GetHistoryProcessRequestFromEmployeeByIdQueryResponse();

			// Получаем исполнителей по историям заявок.
			IEnumerable<WorkplaceEmployee> executors = await GetExecutorsByProcessRequestsAsync(processRequests, cancellationToken);

			if (!executors.Any())
				return new();
			else
				return BuildProcessRequestResponse(processRequests, new List<WorkplaceEmployee> { employee }, executors);
		}

		/// <summary>
		/// Получает исполнителей по историям заявок.
		/// </summary>
		/// <param name="processRequests"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationException"></exception>
		private async Task<IEnumerable<WorkplaceEmployee>> GetExecutorsByProcessRequestsAsync(
			IEnumerable<ProcessRequest> processRequests,
			CancellationToken cancellationToken)
		{
			// Получаем уникальные идентификаторы исполнителей по историям заявок
			var idsExecutors = processRequests.Distinct().Select(x => x.ExecutorEmployeeId).ToList()
				?? throw new ApplicationException($"Не найдены идентификаторы исполнителей");

			if (idsExecutors.Count == 0)
				throw new ApplicationException($"Идентификаторы исполнителей пусты");

			// Получаем исполнителей по их идентификаторам
			IEnumerable<WorkplaceEmployee> executors = await _workplaceEmployeesRepository.GetByIdsAsync(idsExecutors, cancellationToken)
				?? throw new ApplicationException($"Null или Данные исполнителей по их идентификаторам не найдены в БД.");

			return executors;
		}

		/// <summary>
		/// Строит ответ для запроса по истории заявок от заказчика по идентификатору.
		/// </summary>
		/// <param name="processRequests"></param>
		/// <param name="customers"></param>
		/// <param name="executors"></param>
		/// <returns></returns>
		private static GetHistoryProcessRequestFromEmployeeByIdQueryResponse
			BuildProcessRequestResponse(
			IEnumerable<ProcessRequest> processRequests,
			IEnumerable<WorkplaceEmployee> customers,
			IEnumerable<WorkplaceEmployee> executors)
		{
			// Создаем словари для быстрого доступа к заказчикам и исполнителям по их идентификаторам
			var customerDict = customers.ToDictionary(x => x.Id);
			var executorDict = executors.ToDictionary(x => x.Id);

			// Формируем ответ с данными заявок
			return new GetHistoryProcessRequestFromEmployeeByIdQueryResponse
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
					TypeBreakdown = x.TypeBreakdown.Name,
					DescriptionMalfunction = x.DescriptionMalfunction.Value,
					ApplicationStatus = x.ApplicationStatus.Name,
					InternalComment = x.InternalComment?.Value,
					Importance = x.Importance.Name,

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
	}
}
