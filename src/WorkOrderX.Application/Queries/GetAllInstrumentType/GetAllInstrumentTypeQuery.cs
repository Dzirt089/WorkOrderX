using MediatR;

using WorkOrderX.Application.Queries.GetAllInstrumentType.Responses;

namespace WorkOrderX.Application.Queries.GetAllInstrumentType
{
	public record GetAllInstrumentTypeQuery : IRequest<GetAllInstrumentTypeQueryResponse>
	{
	}
}
