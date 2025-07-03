using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetEmployeeByAccount;
using WorkOrderX.Application.Queries.GetEmployeeByAccount.Responses;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.Application.Handlers.QueryHandler.WorkplaceEmployees
{
	/// <summary>
	/// Обработчик запроса для получения информации о сотруднике по учетной записи рабочего места.
	/// </summary>
	public sealed class GetEmployeeByAccountQueryHandler : IRequestHandler<GetEmployeeByAccountQuery, GetEmployeeByAccountQueryResponse>
	{
		private readonly IWorkplaceEmployeesRepository _workplaceEmployeesRepository;

		public GetEmployeeByAccountQueryHandler(IWorkplaceEmployeesRepository workplaceEmployeesRepository)
		{
			_workplaceEmployeesRepository = workplaceEmployeesRepository;
		}

		/// <summary>
		/// Конструктор обработчика запроса.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<GetEmployeeByAccountQueryResponse> Handle(GetEmployeeByAccountQuery request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(request.UserAccount))
			{
				throw new ArgumentException("Account cannot be null or empty.", nameof(request.UserAccount));
			}

			var account = Account.Create(request.UserAccount) ?? throw new ApplicationException($"Account for WorkplaceEmployee is null, {nameof(request.UserAccount)}");

			WorkplaceEmployee? employee = await _workplaceEmployeesRepository.GetByAccountAsync(account, cancellationToken) ?? throw new ApplicationException($"WorkplaceEmployee is null, {nameof(account)}");


			return new GetEmployeeByAccountQueryResponse
			{
				EmployeeDataDto = new EmployeeDataDto
				{
					Account = employee.Account.Value,
					Role = employee.Role.Name,
					Name = employee.Name.Value,
					Department = employee.Department.Value,
					Email = employee.Email.Value,
					Phone = employee.Phone.Value,
					Specialized = employee.Specialized?.Name ?? string.Empty
				}
			};
		}
	}
}
