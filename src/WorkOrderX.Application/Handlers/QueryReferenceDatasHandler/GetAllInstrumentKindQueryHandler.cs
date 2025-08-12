using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllInstrumentKind;
using WorkOrderX.Application.Queries.GetAllInstrumentKind.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllInstrumentKindQueryHandler : IRequestHandler<GetAllInstrumentKindQuery, GetAllInstrumentKindQueryResponse>
	{
		private readonly IReferenceDataRepository<InstrumentKind> _referenceDataRepository;

		public GetAllInstrumentKindQueryHandler(IReferenceDataRepository<InstrumentKind> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllInstrumentKindQueryResponse> Handle(GetAllInstrumentKindQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllInstrumentKindAsync(cancellationToken);

			var result = new GetAllInstrumentKindQueryResponse
			{
				InstrumentKindDatas = allStatuses.Select(_ => new InstrumentKindDataDto
				{
					Id = _.Id,
					Name = _.Name,
					Description = _.Descriptions,
					Type = new InstrumentTypeDataDto
					{
						Id = _.InstrumentType.Id,
						Name = _.InstrumentType.Name,
						Description = _.InstrumentType.Descriptions
					}
				})
			};

			return result;
		}
	}
}
