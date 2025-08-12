using MediatR;

using WorkOrderX.Application.Queries.GetAllInstrumentKind.Responses;

namespace WorkOrderX.Application.Queries.GetAllInstrumentKind
{
	public record GetAllInstrumentKindQuery : IRequest<GetAllInstrumentKindQueryResponse>
	{
	}
}
