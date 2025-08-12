using MediatR;

using WorkOrderX.Application.Queries.GetAllInstrumentModel.Responses;

namespace WorkOrderX.Application.Queries.GetAllInstrumentModel
{
	public record GetAllInstrumentModelQuery : IRequest<GetAllInstrumentModelQueryResponse>
	{
	}
}
