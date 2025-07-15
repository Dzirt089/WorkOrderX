using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllEquipmentModel;
using WorkOrderX.Application.Queries.GetAllEquipmentModel.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllEquipmentModelQueryHandler : IRequestHandler<GetAllEquipmentModelQuery, GetAllEquipmentModelQueryResponse>
	{

		private readonly IReferenceDataRepository<EquipmentModel> _referenceDataRepository;

		public GetAllEquipmentModelQueryHandler(IReferenceDataRepository<EquipmentModel> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllEquipmentModelQueryResponse> Handle(GetAllEquipmentModelQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllAsync(cancellationToken);

			var result = new GetAllEquipmentModelQueryResponse
			{
				EquipmentModelDatas = allStatuses.Select(_ => new EquipmentModelDataDto
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
