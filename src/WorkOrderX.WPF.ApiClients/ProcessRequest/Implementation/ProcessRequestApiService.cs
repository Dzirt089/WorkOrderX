using System.Text.Json;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ProcessRequest.Implementation
{
	public class ProcessRequestApiService : BaseApiClient, IProcessRequestApiService
	{
		public ProcessRequestApiService(
			IHttpClientFactory httpClientFactory,
			JsonSerializerOptions jsonOptions)
			: base(httpClientFactory.CreateClient("WorkOrderXApi"), jsonOptions)
		{
			_httpClient.BaseAddress = new Uri(_httpClient.BaseAddress, "ProcessRequest/");
		}

		public async Task<bool> CreateProcessRequestAsync(CreateProcessRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("CreateProcessRequest", model, token);

		public async Task<IEnumerable<ProcessRequestDataModel>> GetActivProcessRequestsAsync(Guid employeeId,
			CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<ProcessRequestDataModel>>($"GetActivProcessRequests/{employeeId}", token);

		public async Task<IEnumerable<ProcessRequestDataModel>> GetHistoryProcessRequestsAsync(Guid employeeId,
			CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<ProcessRequestDataModel>>($"GetHistoryProcessRequests/{employeeId}", token);

		public async Task<bool> UpdateProcessRequestAsync(UpdateProcessRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateProcessRequest", model, token);

		public async Task<bool> UpdateStatusDoneOrRejectedAsync(UpdateStatusDoneOrRejectedModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateStatusDoneOrRejected", model, token);

		public async Task<bool> UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(UpdateStatusInWorkOrReturnedOrPostponedRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateStatusInWorkOrReturnedOrPostponedRequest", model, token);

		public async Task<bool> UpdateStatusRedirectedRequestAsync(UpdateStatusRedirectedRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateStatusRedirectedRequest", model, token);

	}
}
