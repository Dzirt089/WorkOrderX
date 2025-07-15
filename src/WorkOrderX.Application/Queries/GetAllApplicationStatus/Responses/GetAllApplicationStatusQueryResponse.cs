using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllApplicationStatus.Responses
{
	public class GetAllApplicationStatusQueryResponse
	{
		public IEnumerable<ApplicationStatusDataDto> ApplicationStatusDatas { get; set; }
	}
}
