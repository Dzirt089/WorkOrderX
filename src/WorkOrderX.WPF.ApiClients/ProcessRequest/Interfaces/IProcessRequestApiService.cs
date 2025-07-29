using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ProcessRequest.Interfaces
{
	public interface IProcessRequestApiService
	{
		Task<bool> CreateProcessRequestAsync(CreateProcessRequestModel model, CancellationToken token = default);
		Task<IEnumerable<ProcessRequestDataModel>> GetActivProcessRequestsAsync(Guid employeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequestDataModel>> GetHistoryProcessRequestsAsync(Guid employeeId, CancellationToken token = default);
		Task<bool> UpdateProcessRequestAsync(UpdateProcessRequestModel model, CancellationToken token = default);
		Task<bool> UpdateStatusDoneOrRejectedAsync(UpdateStatusDoneOrRejectedModel model, CancellationToken token = default);
		Task<bool> UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(UpdateStatusInWorkOrReturnedOrPostponedRequestModel model, CancellationToken token = default);
		Task<bool> UpdateStatusRedirectedRequestAsync(UpdateStatusRedirectedRequestModel model, CancellationToken token = default);
		Task<bool> UpdateInternalCommentRequestAsync(UpdateInternalCommentRequestModel model, CancellationToken token = default);
	}
}
