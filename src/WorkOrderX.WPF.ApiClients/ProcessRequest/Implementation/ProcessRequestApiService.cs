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

		public async Task<bool> CreateProcessRequestAsync(CreateProcessRequestModel model, CancellationToken token = default)
		{
			var response = await PostTJsonTAsync<bool>("CreateProcessRequest", model, token);
			return response;
		}
	}
}
