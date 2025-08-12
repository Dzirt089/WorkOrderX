using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllInstrumentModel;
using WorkOrderX.Application.Queries.GetAllInstrumentModel.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllInstrumentModelQueryHandler : IRequestHandler<GetAllInstrumentModelQuery, GetAllInstrumentModelQueryResponse>
	{

		private readonly IReferenceDataRepository<InstrumentModel> _referenceDataRepository;

		public GetAllInstrumentModelQueryHandler(IReferenceDataRepository<InstrumentModel> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllInstrumentModelQueryResponse> Handle(GetAllInstrumentModelQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllAsync(cancellationToken);

			var result = new GetAllInstrumentModelQueryResponse
			{
				InstrumentModelDatas = allStatuses.Select(_ => new InstrumentModelDataDto
				{
					Id = _.Id,
					Name = _.Name,
					Description = _.Descriptions
				})
			};

			return result;
		}
	}
}
