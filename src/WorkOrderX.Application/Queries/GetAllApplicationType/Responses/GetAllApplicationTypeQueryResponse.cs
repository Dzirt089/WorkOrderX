using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllApplicationType.Responses
{
	public class GetAllApplicationTypeQueryResponse
	{
		public IEnumerable<ApplicationTypeDataDto> ApplicationTypeDatas { get; set; }
	}
}
