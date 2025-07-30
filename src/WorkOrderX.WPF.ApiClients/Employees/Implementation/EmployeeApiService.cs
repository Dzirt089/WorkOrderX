using System.Text.Json;

using WorkOrderX.ApiClients.Employees.Interfaces;
using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.Employees.Implementation
{
	/// <summary>
	/// Сервис для работы с API сотрудников.
	/// </summary>
	public class EmployeeApiService : BaseApiClient, IEmployeeApiService
	{
		public EmployeeApiService(
			IHttpClientFactory httpClientFactory,
			JsonSerializerOptions jsonOptions)
			: base(httpClientFactory.CreateClient("WorkOrderXApi"), jsonOptions)
		{
			_httpClient.BaseAddress = new Uri(_httpClient.BaseAddress, "Employee/");
		}

		/// <summary>
		/// Метод для авторизации и получения информации об сотруднике.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<LoginResponseDataModel> LoginAndAuthorizationAsync(string account, CancellationToken token = default) =>
			await GetTJsonTAsync<LoginResponseDataModel>($"Login/{account}", token);

		/// <summary>
		/// Метод для получения списка сотрудников по роли.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<EmployeeDataModel>> GetByRoleEmployeesAsync(string role, CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<EmployeeDataModel>>($"GetByRoleEmployees/{role}", token);
	}
}
