using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses
{
	/// <summary>
	/// Ответ на запрос получения активных заявок от клиента по идентификатору сотрудника
	/// </summary>
	public class GetActivProcessRequestFromEmployeeByIdQueryResponse
	{
		/// <summary>
		/// Список активных заявок от сотрудника
		/// </summary>
		public IEnumerable<ProcessRequestDataDto> ProcessRequests { get; set; }
	}
}
