using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllApplicationType;
using WorkOrderX.Application.Queries.GetAllApplicationType.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllApplicationTypeQueryHandler : IRequestHandler<GetAllApplicationTypeQuery, GetAllApplicationTypeQueryResponse>
	{
		private readonly IReferenceDataRepository<ApplicationType> _referenceDataRepository;

		public GetAllApplicationTypeQueryHandler(IReferenceDataRepository<ApplicationType> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllApplicationTypeQueryResponse> Handle(GetAllApplicationTypeQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllAsync(cancellationToken);

			var result = new GetAllApplicationTypeQueryResponse
			{
				ApplicationTypeDatas = allStatuses.Select(_ => new ApplicationTypeDataDto
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
