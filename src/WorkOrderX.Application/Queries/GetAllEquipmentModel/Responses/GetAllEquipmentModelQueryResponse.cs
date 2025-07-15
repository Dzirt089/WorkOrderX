using WorkOrderX.Application.Models.DTOs;

namespace WorkOrderX.Application.Queries.GetAllEquipmentModel.Responses
{
	public class GetAllEquipmentModelQueryResponse
	{
		public IEnumerable<EquipmentModelDataDto> EquipmentModelDatas { get; set; }
	}
}
