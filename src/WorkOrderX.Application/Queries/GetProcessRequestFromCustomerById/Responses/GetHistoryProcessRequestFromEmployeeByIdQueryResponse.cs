using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses
{
	/// <summary>
	/// Ответ на запрос получения историй заявок от клиента по идентификатору сотрудника
	/// </summary>
	public class GetHistoryProcessRequestFromEmployeeByIdQueryResponse
	{
		/// <summary>
		/// Список историй заявок от сотрудника
		/// </summary>
		public IEnumerable<ProcessRequestDataDto> ProcessRequests { get; set; }
	}
}
