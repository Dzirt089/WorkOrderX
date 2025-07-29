using MediatR;

using WorkOrderX.Application.Queries.GetByRoleEmployees.Responses;

namespace WorkOrderX.Application.Queries.GetByRoleEmployees
{
	public record GetByRoleEmployeesQuery : IRequest<GetByRoleEmployeesQueryResponse>
	{
		/// <summary>
		/// Идентификатор роли, по которой нужно получить сотрудников.
		/// </summary>
		public string? Role { get; init; }
	}
}
