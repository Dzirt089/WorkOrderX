using MediatR;

using WorkOrderX.Application.Queries.GetAllApplicationType.Responses;

namespace WorkOrderX.Application.Queries.GetAllApplicationType
{
	public record GetAllApplicationTypeQuery : IRequest<GetAllApplicationTypeQueryResponse>
	{
	}
}
