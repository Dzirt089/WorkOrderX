using System.Text.Json;

using WorkOrderX.ApiClients.Employees.Interfaces;
using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.Employees.Implementation
{
	public class EmployeeApiService : BaseApiClient, IEmployeeApiService
	{
		public EmployeeApiService(
			IHttpClientFactory httpClientFactory,
			JsonSerializerOptions jsonOptions)
			: base(httpClientFactory.CreateClient("WorkOrderXApi"), jsonOptions)
		{
			_httpClient.BaseAddress = new Uri(_httpClient.BaseAddress, "Employee/");
		}

		public async Task<LoginResponseDataModel> LoginAsync(string account, CancellationToken token = default)
		{
			var response = await GetTJsonTAsync<LoginResponseDataModel>($"Login/{account}", token);
			return response;
		}
	}
}
