using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetByRoleEmployees;
using WorkOrderX.Application.Queries.GetByRoleEmployees.Responses;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.Application.Handlers.QueryHandler.WorkplaceEmployees
{
	public sealed class GetByRoleEmployeesQueryHandler : IRequestHandler<GetByRoleEmployeesQuery, GetByRoleEmployeesQueryResponse>
	{
		private readonly IWorkplaceEmployeesRepository _workplaceEmployeesRepository;
		//private readonly IReferenceDataRepository<Role> _referenceDataRepository;

		public GetByRoleEmployeesQueryHandler(IWorkplaceEmployeesRepository workplaceEmployeesRepository)
		{
			_workplaceEmployeesRepository = workplaceEmployeesRepository;
		}

		public async Task<GetByRoleEmployeesQueryResponse> Handle(GetByRoleEmployeesQuery request, CancellationToken cancellationToken)
		{
			if (request is null || string.IsNullOrEmpty(request.Role))
			{
				throw new ArgumentException($"Request cannot be null or Empty {nameof(request.Role)}.", nameof(request));
			}

			var role = Role.FromName<Role>(request.Role)
				?? throw new ApplicationException($"Role is null, {nameof(request.Role)}");

			var employees = await _workplaceEmployeesRepository.GetByRoleAsync(role, cancellationToken)
				?? throw new ApplicationException($"WorkplaceEmployees is null, {nameof(role)}");

			return new GetByRoleEmployeesQueryResponse
			{
				Employees = employees.Select(employee => new EmployeeDataDto
				{
					Account = employee.Account.Value,
					Role = employee.Role.Name,
					Name = employee.Name.Value,
					Department = employee.Department.Value,
					Email = employee.Email.Value,
					Phone = employee.Phone.Value,
					Specialized = employee.Specialized?.Name ?? string.Empty,
					Id = employee.Id
				})
				.ToList()
			};
		}
	}
}
