using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllTypeBreakdown.Responses
{
	public class GetAllTypeBreakdownQueryResponse
	{
		public IEnumerable<TypeBreakdownDataDto> TypeBreakdownDatas { get; set; }
	}
}
