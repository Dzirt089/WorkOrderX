using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllImportances;
using WorkOrderX.Application.Queries.GetAllImportances.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllImportancesQueryHandler : IRequestHandler<GetAllImportancesQuery, GetAllImportancesQueryResponse>
	{
		private readonly IReferenceDataRepository<Importance> _referenceDataRepository;

		public GetAllImportancesQueryHandler(IReferenceDataRepository<Importance> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllImportancesQueryResponse> Handle(GetAllImportancesQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllAsync(cancellationToken);

			var result = new GetAllImportancesQueryResponse
			{
				ImportancesDataDtos = allStatuses.Select(_ => new ImportancesDataDto
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
