using MediatR;

using WorkOrderX.Application.Queries.GetAllEquipmentType.Responses;

namespace WorkOrderX.Application.Queries.GetAllEquipmentType
{
	public record GetAllEquipmentTypeQuery : IRequest<GetAllEquipmentTypeQueryResponse>
	{
	}
}
