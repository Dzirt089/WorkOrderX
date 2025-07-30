using Microsoft.EntityFrameworkCore;

using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	/// <summary>
	/// Реализует репозиторий для работы с сотрудниками рабочего места.
	/// </summary>
	public class WorkplaceEmployeesRepository : IWorkplaceEmployeesRepository
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public WorkplaceEmployeesRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		/// <summary>
		/// Добавляет нового сотрудника рабочего места в базу данных.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task AddAsync(WorkplaceEmployee employee, CancellationToken token = default)
		{
			await _workOrderDbContext.WorkplaceEmployees.AddAsync(employee, token);
		}

		/// <summary>
		/// Получает всех сотрудников рабочего места из базы данных.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<WorkplaceEmployee>> GetAllAsync(CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получает сотрудника рабочего места по учетной записи.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<WorkplaceEmployee?> GetByAccountAsync(Account account, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.FirstOrDefaultAsync(_ => _.Account.Value == account.Value, token);

			return result;
		}

		/// <summary>
		/// Получает сотрудников рабочего места по отделу.
		/// </summary>
		/// <param name="department"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<WorkplaceEmployee>> GetByDepartmentAsync(Department department, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.Where(_ => _.Department.Value == department.Value)
				.ToListAsync(token);

			return result;
		}

		/// <summary>
		/// Получает сотрудника рабочего места по электронной почте.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<WorkplaceEmployee?> GetByEmailAsync(Email email, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.FirstOrDefaultAsync(_ => _.Email.Value == email.Value, token);

			return result;
		}

		/// <summary>
		/// Получает сотрудника рабочего места по идентификатору.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="token"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Получает сотрудников рабочего места по списку идентификаторов.
		/// </summary>
		/// <param name="ids"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<WorkplaceEmployee>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.Where(_ => ids.Contains(_.Id))
				.ToListAsync(token);

			return result;
		}

		/// <summary>
		/// Получает сотрудника рабочего места по номеру телефона.
		/// </summary>
		/// <param name="phone"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<WorkplaceEmployee?> GetByPhoneAsync(Phone phone, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.FirstOrDefaultAsync(_ => _.Phone.Value == phone.Value, token);

			return result;
		}

		/// <summary>
		/// Получает сотрудников рабочего места по роли.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<WorkplaceEmployee>> GetByRoleAsync(Role role, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.WorkplaceEmployees
				.Include(_ => _.Role)
				.Include(_ => _.Specialized)
				.Where(_ => _.RoleId == role.Id)
				.ToListAsync(token);

			return result;
		}

		/// <summary>
		/// Получает сотрудника рабочего места по специализации.
		/// </summary>
		/// <param name="specialized"></param>
		/// <param name="token"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Обновляет информацию о сотруднике рабочего места в базе данных.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task UpdateAsync(WorkplaceEmployee employee, CancellationToken token = default)
		{
			_workOrderDbContext.WorkplaceEmployees.Update(employee);
		}
	}
}
