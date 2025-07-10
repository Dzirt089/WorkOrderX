using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class WorkplaceEmployeesRepository : IWorkplaceEmployeesRepository
	{
		public Task AddAsync(WorkplaceEmployee employee, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WorkplaceEmployee>> GetAllAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<WorkplaceEmployee?> GetByAccountAsync(Account account, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WorkplaceEmployee>> GetByDepartmentAsync(Department department, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<WorkplaceEmployee?> GetByEmailAsync(Email email, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<WorkplaceEmployee> GetByIdAsync(Guid? id, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WorkplaceEmployee>> GetByIdsAsync(IEnumerable<Guid> idEmployees, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<WorkplaceEmployee?> GetByPhoneAsync(Phone phone, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<WorkplaceEmployee>> GetByRoleAsync(Role role, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<WorkplaceEmployee> GetBySpecializedAsync(Specialized specialized, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(WorkplaceEmployee employee, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}
	}
}
