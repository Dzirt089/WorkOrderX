using MediatR;

using WorkOrderX.Application.Queries.GetAllEquipmentKind.Responses;

namespace WorkOrderX.Application.Queries.GetAllEquipmentKind
{
	public record GetAllEquipmentKindQuery : IRequest<GetAllEquipmentKindQueryResponse>
	{
	}
}
