using WorkOrderX.Domain.Contracts;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	public interface IEmployeeRepository : IRepository<Employee>
	{
		Task<Employee?> GetByAccountAsync(Account account, CancellationToken token = default);
		Task<Employee?> GetByEmailAsync(Email email, CancellationToken token = default);
		Task<Employee?> GetByPhoneAsync(Phone phone, CancellationToken token = default);
		Task<IEnumerable<Employee>> GetByRoleAsync(Role role, CancellationToken token = default);
		Task<IEnumerable<Employee>> GetByDepartmentAsync(Department department, CancellationToken token = default);
		Task<Employee> GetByIdAsync(Guid id, CancellationToken token = default);
		Task<Employee> GetBySpecializedAsync(Specialized specialized, CancellationToken token = default);


		Task<IEnumerable<Employee>> GetAllAsync(CancellationToken token = default);
		Task AddAsync(Employee employee, CancellationToken token = default);
		Task UpdateAsync(Employee employee, CancellationToken token = default);
	}
}
