using MediatR;

using WorkOrderX.Application.Queries.GetAllApplicationStatus.Responses;

namespace WorkOrderX.Application.Queries.GetAllApplicationStatus
{
	public record GetAllApplicationStatusQuery : IRequest<GetAllApplicationStatusQueryResponse>
	{
	}
}
