using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.Employees.Interfaces
{
	/// <summary>
	/// Интерфейс для работы с API сотрудников.
	/// </summary>
	public interface IEmployeeApiService : IWorkOrderXApi
	{
		/// <summary>
		/// Метод для авторизации и получения информации об сотруднике.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<LoginResponseDataModel> LoginAndAuthorizationAsync(string account, CancellationToken token = default);

		/// <summary>
		/// Метод для получения списка сотрудников по роли.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<EmployeeDataModel>> GetByRoleEmployeesAsync(string role, CancellationToken token = default);
	}
}
