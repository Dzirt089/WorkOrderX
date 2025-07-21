using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllImportances.Responses
{
	public class GetAllImportancesQueryResponse
	{
		public IEnumerable<ImportancesDataDto> ImportancesDataDtos { get; set; }
	}
}
