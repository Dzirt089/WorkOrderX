using WorkOrderX.Domain.Contracts;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	/// <summary>
	/// Реализует репозиторий для работы с сотрудниками рабочего места.
	/// </summary>
	public interface IWorkplaceEmployeesRepository : IRepository<WorkplaceEmployee>
	{
		/// <summary>
		/// Получает сотрудника рабочего места по учетной записи.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<WorkplaceEmployee?> GetByAccountAsync(Account account, CancellationToken token = default);

		/// <summary>
		/// Получает сотрудника рабочего места по электронной почте.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<WorkplaceEmployee?> GetByEmailAsync(Email email, CancellationToken token = default);

		/// <summary>
		/// Получает сотрудника рабочего места по номеру телефона.
		/// </summary>
		/// <param name="phone"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<WorkplaceEmployee?> GetByPhoneAsync(Phone phone, CancellationToken token = default);

		/// <summary>
		/// Получает сотрудников рабочего места по роли.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<WorkplaceEmployee>> GetByRoleAsync(Role role, CancellationToken token = default);

		/// <summary>
		/// Получает сотрудников рабочего места по отделу.
		/// </summary>
		/// <param name="department"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<WorkplaceEmployee>> GetByDepartmentAsync(Department department, CancellationToken token = default);

		/// <summary>
		/// Получает сотрудника рабочего места по идентификатору.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<WorkplaceEmployee?> GetByIdAsync(Guid? id, CancellationToken token = default);

		/// <summary>
		/// Получает сотрудника рабочего места по специализации.
		/// </summary>
		/// <param name="specialized"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<WorkplaceEmployee?> GetBySpecializedAsync(Specialized specialized, CancellationToken token = default);

		/// <summary>
		/// Получает сотрудников рабочего места по списку идентификаторов.
		/// </summary>
		/// <param name="ids"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<WorkplaceEmployee>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default);

		/// <summary>
		/// Получает всех сотрудников рабочего места из базы данных.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<WorkplaceEmployee>> GetAllAsync(CancellationToken token = default);

		/// <summary>
		/// Добавляет нового сотрудника рабочего места в базу данных.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task AddAsync(WorkplaceEmployee employee, CancellationToken token = default);

		/// <summary>
		/// Обновляет информацию о сотруднике рабочего места в базе данных.
		/// </summary>
		/// <param name="employee"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task UpdateAsync(WorkplaceEmployee employee, CancellationToken token = default);
	}
}
