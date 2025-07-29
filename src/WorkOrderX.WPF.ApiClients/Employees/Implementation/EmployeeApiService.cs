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

		public async Task<LoginResponseDataModel> LoginAsync(string account, CancellationToken token = default) =>
			await GetTJsonTAsync<LoginResponseDataModel>($"Login/{account}", token);

		public async Task<IEnumerable<EmployeeDataModel>> GetByRoleEmployeesAsync(string role, CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<EmployeeDataModel>>($"GetByRoleEmployees/{role}", token);
	}
}
