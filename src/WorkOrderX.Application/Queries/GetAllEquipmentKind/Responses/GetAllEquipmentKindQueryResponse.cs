using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllEquipmentKind.Responses
{
	public class GetAllEquipmentKindQueryResponse
	{
		public IEnumerable<EquipmentKindDataDto> EquipmentKindDatas { get; set; }
	}
}
