using WorkOrderX.Domain.Contracts;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	public interface IWorkplaceEmployeesRepository : IRepository<WorkplaceEmployees>
	{
		Task<WorkplaceEmployees?> GetByAccountAsync(Account account, CancellationToken token = default);
		Task<WorkplaceEmployees?> GetByEmailAsync(Email email, CancellationToken token = default);
		Task<WorkplaceEmployees?> GetByPhoneAsync(Phone phone, CancellationToken token = default);
		Task<IEnumerable<WorkplaceEmployees>> GetByRoleAsync(Role role, CancellationToken token = default);
		Task<IEnumerable<WorkplaceEmployees>> GetByDepartmentAsync(Department department, CancellationToken token = default);
		Task<WorkplaceEmployees> GetByIdAsync(Guid? id, CancellationToken token = default);
		Task<WorkplaceEmployees> GetBySpecializedAsync(Specialized specialized, CancellationToken token = default);


		Task<IEnumerable<WorkplaceEmployees>> GetAllAsync(CancellationToken token = default);
		Task AddAsync(WorkplaceEmployees employee, CancellationToken token = default);
		Task UpdateAsync(WorkplaceEmployees employee, CancellationToken token = default);
	}
}
