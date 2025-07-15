using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllEquipmentType.Responses
{
	public class GetAllEquipmentTypeQueryResponse
	{
		public IEnumerable<EquipmentTypeDataDto> EquipmentTypeDatas { get; set; }
	}
}
