using Microsoft.EntityFrameworkCore;

using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class WorkplaceEmployeesRepository : IWorkplaceEmployeesRepository
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public WorkplaceEmployeesRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task AddAsync(WorkplaceEmployee employee, CancellationToken token = default)
		{
			await _workOrderDbContext.WorkplaceEmployees.AddAsync(employee, token);
		}

		public async Task<IEnumerable<WorkplaceEmployee>> GetAllAsync(CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees.ToListAsync(token);
			return result;
		}

		public async Task<WorkplaceEmployee?> GetByAccountAsync(Account account, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.FirstOrDefaultAsync(_ => _.Account.Value == account.Value, token);

			return result;
		}

		public async Task<IEnumerable<WorkplaceEmployee>> GetByDepartmentAsync(Department department, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.Where(_ => _.Department.Value == department.Value)
				.ToListAsync(token);

			return result;
		}

		public async Task<WorkplaceEmployee?> GetByEmailAsync(Email email, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.FirstOrDefaultAsync(_ => _.Email.Value == email.Value, token);

			return result;
		}

		public async Task<WorkplaceEmployee?> GetByIdAsync(Guid? id, CancellationToken token = default)
		{
			var result = id is null
				? null
				: await _workOrderDbContext.WorkplaceEmployees
					.Include(_ => _.Role)
					.Include(_ => _.Specialized)
					.FirstOrDefaultAsync(_ => _.Id == id, token);

			return result;
		}

		public async Task<IEnumerable<WorkplaceEmployee>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.Where(_ => ids.Contains(_.Id))
				.ToListAsync(token);

			return result;
		}

		public async Task<WorkplaceEmployee?> GetByPhoneAsync(Phone phone, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.FirstOrDefaultAsync(_ => _.Phone.Value == phone.Value, token);

			return result;
		}

		public async Task<IEnumerable<WorkplaceEmployee>> GetByRoleAsync(Role role, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.Where(_ => _.RoleId == role.Id)
				.ToListAsync(token);

			return result;
		}

		public async Task<WorkplaceEmployee?> GetBySpecializedAsync(Specialized specialized, CancellationToken token = default)
		{
			var result = specialized is null
				? null :
				await _workOrderDbContext.WorkplaceEmployees
					.Include(_ => _.Role)
					.Include(_ => _.Specialized)
					.FirstOrDefaultAsync(_ => _.SpecializedId == specialized.Id, token);

			return result;
		}

		public async Task UpdateAsync(WorkplaceEmployee employee, CancellationToken token = default)
		{
			_workOrderDbContext.WorkplaceEmployees.Update(employee);
		}
	}
}
