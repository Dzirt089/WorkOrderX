using MediatR;

using WorkOrderX.Application.Queries.GetAllEquipmentModel.Responses;

namespace WorkOrderX.Application.Queries.GetAllEquipmentModel
{
	public record GetAllEquipmentModelQuery : IRequest<GetAllEquipmentModelQueryResponse>
	{
	}
}
