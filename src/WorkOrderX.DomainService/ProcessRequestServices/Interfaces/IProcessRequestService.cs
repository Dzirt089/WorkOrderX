using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.DomainService.ProcessRequestServices.Interfaces
{
	public interface IProcessRequestService
	{
		Task<ProcessRequest> CreateProcessRequest(
			long applicationNumber,
			string applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			string equipmentType,
			string equipmentKind,
			string equipmentModel,
			string serialNumber,
			string typeBreakdown,
			string descriptionMalfunction,
			string applicationStatus,
			string internalComment,
			Guid customerEmployeeId,
			CancellationToken token);
	}
}
