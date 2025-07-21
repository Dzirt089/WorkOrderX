using MediatR;

using WorkOrderX.Application.Queries.GetAllImportances.Responses;

namespace WorkOrderX.Application.Queries.GetAllImportances
{
	public record GetAllImportancesQuery : IRequest<GetAllImportancesQueryResponse>
	{
	}
}
