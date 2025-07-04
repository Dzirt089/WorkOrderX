using WorkOrderX.Domain.Contracts;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	public interface IWorkplaceEmployeesRepository : IRepository<WorkplaceEmployee>
	{
		Task<WorkplaceEmployee?> GetByAccountAsync(Account account, CancellationToken token = default);
		Task<WorkplaceEmployee?> GetByEmailAsync(Email email, CancellationToken token = default);
		Task<WorkplaceEmployee?> GetByPhoneAsync(Phone phone, CancellationToken token = default);
		Task<IEnumerable<WorkplaceEmployee>> GetByRoleAsync(Role role, CancellationToken token = default);
		Task<IEnumerable<WorkplaceEmployee>> GetByDepartmentAsync(Department department, CancellationToken token = default);
		Task<WorkplaceEmployee> GetByIdAsync(Guid? id, CancellationToken token = default);
		Task<WorkplaceEmployee> GetBySpecializedAsync(Specialized specialized, CancellationToken token = default);

		Task<IEnumerable<WorkplaceEmployee>> GetByIdsAsync(IEnumerable<Guid> idEmployees, CancellationToken token = default);

		Task<IEnumerable<WorkplaceEmployee>> GetAllAsync(CancellationToken token = default);
		Task AddAsync(WorkplaceEmployee employee, CancellationToken token = default);
		Task UpdateAsync(WorkplaceEmployee employee, CancellationToken token = default);
	}
}
