using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllInstrumentModel.Responses
{
	public class GetAllInstrumentModelQueryResponse
	{
		public IEnumerable<InstrumentModelDataDto> InstrumentModelDatas { get; set; }
	}
}
