using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ProcessRequest.Interfaces
{
	public interface IProcessRequestApiService
	{
		Task<bool> CreateProcessRequestAsync(CreateProcessRequestModel model, CancellationToken token = default);
	}
}
