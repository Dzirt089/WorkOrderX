using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllEquipmentType;
using WorkOrderX.Application.Queries.GetAllEquipmentType.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllEquipmentTypeQueryHandler : IRequestHandler<GetAllEquipmentTypeQuery, GetAllEquipmentTypeQueryResponse>
	{

		private readonly IReferenceDataRepository<EquipmentType> _referenceDataRepository;

		public GetAllEquipmentTypeQueryHandler(IReferenceDataRepository<EquipmentType> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllEquipmentTypeQueryResponse> Handle(GetAllEquipmentTypeQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllAsync(cancellationToken);

			var result = new GetAllEquipmentTypeQueryResponse
			{
				EquipmentTypeDatas = allStatuses.Select(_ => new EquipmentTypeDataDto
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
