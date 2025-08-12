using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllInstrumentType.Responses
{
	public class GetAllInstrumentTypeQueryResponse
	{
		public IEnumerable<InstrumentTypeDataDto> InstrumentTypeDatas { get; set; }
	}
}
