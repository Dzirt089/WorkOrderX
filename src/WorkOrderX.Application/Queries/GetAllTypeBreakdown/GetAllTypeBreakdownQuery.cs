using MediatR;

using WorkOrderX.Application.Queries.GetAllTypeBreakdown.Responses;

namespace WorkOrderX.Application.Queries.GetAllTypeBreakdown
{
	public record GetAllTypeBreakdownQuery : IRequest<GetAllTypeBreakdownQueryResponse>
	{
	}
}
