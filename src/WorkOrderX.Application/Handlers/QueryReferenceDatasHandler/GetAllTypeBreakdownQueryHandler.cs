using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllTypeBreakdown;
using WorkOrderX.Application.Queries.GetAllTypeBreakdown.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllTypeBreakdownQueryHandler : IRequestHandler<GetAllTypeBreakdownQuery, GetAllTypeBreakdownQueryResponse>
	{

		private readonly IReferenceDataRepository<TypeBreakdown> _referenceDataRepository;

		public GetAllTypeBreakdownQueryHandler(IReferenceDataRepository<TypeBreakdown> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllTypeBreakdownQueryResponse> Handle(GetAllTypeBreakdownQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllTypeBreakdownAsync(cancellationToken);

			var result = new GetAllTypeBreakdownQueryResponse
			{
				TypeBreakdownDatas = allStatuses.Select(_ => new TypeBreakdownDataDto
				{
					Id = _.Id,
					Name = _.Name,
					Description = _.Descriptions,
					Type = new EquipmentTypeDataDto
					{
						Id = _.EquipmentType.Id,
						Name = _.EquipmentType.Name,
						Description = _.EquipmentType.Descriptions
					}
				})
			};

			return result;
		}
	}
}
