using MediatR;

using WorkOrderX.Application.Models.DTOs;
using WorkOrderX.Application.Queries.GetAllApplicationStatus;
using WorkOrderX.Application.Queries.GetAllApplicationStatus.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.QueryReferenceDatasHandler
{
	public class GetAllApplicationStatusQueryHandler : IRequestHandler<GetAllApplicationStatusQuery, GetAllApplicationStatusQueryResponse>
	{
		private readonly IReferenceDataRepository<ApplicationStatus> _referenceDataRepository;

		public GetAllApplicationStatusQueryHandler(IReferenceDataRepository<ApplicationStatus> referenceDataRepository)
		{
			_referenceDataRepository = referenceDataRepository;
		}

		public async Task<GetAllApplicationStatusQueryResponse> Handle(GetAllApplicationStatusQuery request, CancellationToken cancellationToken)
		{
			var allStatuses = await _referenceDataRepository.GetAllAsync(cancellationToken);

			var result = new GetAllApplicationStatusQueryResponse
			{
				ApplicationStatusDatas = allStatuses.Select(_ => new ApplicationStatusDataDto
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
