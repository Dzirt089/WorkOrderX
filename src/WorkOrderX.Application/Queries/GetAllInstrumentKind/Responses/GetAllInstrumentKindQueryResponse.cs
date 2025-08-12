using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllInstrumentKind.Responses
{
	public class GetAllInstrumentKindQueryResponse
	{
		public IEnumerable<InstrumentKindDataDto> InstrumentKindDatas { get; set; }
	}
}
