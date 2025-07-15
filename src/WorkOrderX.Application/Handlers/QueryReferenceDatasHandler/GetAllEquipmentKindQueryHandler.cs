using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllEquipmentKind;
using WorkOrderX.Application.Queries.GetAllEquipmentKind.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllEquipmentKindQueryHandler : IRequestHandler<GetAllEquipmentKindQuery, GetAllEquipmentKindQueryResponse>
	{
		private readonly IReferenceDataRepository<EquipmentKind> _referenceDataRepository;

		public GetAllEquipmentKindQueryHandler(IReferenceDataRepository<EquipmentKind> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllEquipmentKindQueryResponse> Handle(GetAllEquipmentKindQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllEquipmentKindAsync(cancellationToken);

			var result = new GetAllEquipmentKindQueryResponse
			{
				EquipmentKindDatas = allStatuses.Select(_ => new EquipmentKindDataDto
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
