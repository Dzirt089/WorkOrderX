using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllInstrumentType;
using WorkOrderX.Application.Queries.GetAllInstrumentType.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllInstrumentTypeQueryHandler : IRequestHandler<GetAllInstrumentTypeQuery, GetAllInstrumentTypeQueryResponse>
	{

		private readonly IReferenceDataRepository<InstrumentType> _referenceDataRepository;

		public GetAllInstrumentTypeQueryHandler(IReferenceDataRepository<InstrumentType> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllInstrumentTypeQueryResponse> Handle(GetAllInstrumentTypeQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllAsync(cancellationToken);

			var result = new GetAllInstrumentTypeQueryResponse
			{
				InstrumentTypeDatas = allStatuses.Select(_ => new InstrumentTypeDataDto
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
