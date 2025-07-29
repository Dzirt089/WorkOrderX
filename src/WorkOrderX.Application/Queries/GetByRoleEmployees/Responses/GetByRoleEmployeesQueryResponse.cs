using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetByRoleEmployees.Responses
{
	public class GetByRoleEmployeesQueryResponse
	{
		public IEnumerable<EmployeeDataDto> Employees { get; set; }
	}
}
