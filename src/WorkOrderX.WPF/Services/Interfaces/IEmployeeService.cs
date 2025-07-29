using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Services.Implementation
{
	public interface IEmployeeService
	{
		/// <summary>
		/// Получение сотрудников по роли.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		Task<IEnumerable<Employee>> GetByRoleEmployeesAsync(string role, CancellationToken token = default);
	}
}
