using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetEmployeeByAccount.Responses
{
	public class GetEmployeeByAccountQueryResponse
	{
		/// <summary>
		/// Данные о сотруднике, которые мы возвращаем по запросу
		/// </summary>
		public EmployeeDataDto EmployeeDataDto { get; set; }
	}
}
