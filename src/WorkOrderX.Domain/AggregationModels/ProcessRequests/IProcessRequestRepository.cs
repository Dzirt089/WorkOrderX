using WorkOrderX.Domain.Contracts;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Репозитория для работы с заявками.
	/// </summary>
	public interface IProcessRequestRepository : IRepository<ProcessRequest>
	{
		/// <summary>
		/// Получение заявки по идентификатору.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<ProcessRequest?> GetByIdAsync(Guid id, CancellationToken token = default);

		/// <summary>
		/// Получение всех заявок из репозитория.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetAllAsync(CancellationToken token = default);

		/// <summary>
		/// Получение активных заявок клиента по идентификатору сотрудника.
		/// </summary>
		/// <param name="customerEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetCustomerActiveProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default);

		/// <summary>
		/// Получение активных заявок исполнителя по идентификатору сотрудника.
		/// </summary>
		/// <param name="executorEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetExecutorActiveProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default);

		/// <summary>
		/// Получение активных заявок администратора по идентификатору сотрудника.
		/// </summary>
		/// <param name="adminEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetAdminActiveProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default);


		/// <summary>
		/// Получение истории заявок клиента по идентификатору сотрудника.
		/// </summary>
		/// <param name="customerEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetCustomerHistoryProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default);

		/// <summary>
		/// Получение истории заявок исполнителя по идентификатору сотрудника.
		/// </summary>
		/// <param name="executorEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetExecutorHistoryProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default);

		/// <summary>
		/// Получение истории заявок администратора по идентификатору сотрудника.
		/// </summary>
		/// <param name="adminEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetAdminHistoryProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default);


		/// <summary>
		/// Добавление новой заявки в репозиторий.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task AddAsync(ProcessRequest processRequest, CancellationToken token = default);

		/// <summary>
		/// Обновление заявки в репозитории.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task UpdateAsync(ProcessRequest processRequest, CancellationToken token = default);

		/// <summary>
		/// Получение заявок по идентификатору сотрудника клиента.
		/// </summary>
		/// <param name="customerEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetByCustomerEmployeeIdAsync(Guid customerEmployeeId, CancellationToken token = default);

		/// <summary>
		/// Получение заявок по идентификатору сотрудника исполнителя.
		/// </summary>
		/// <param name="executorEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetByExecutorEmployeeIdAsync(Guid executorEmployeeId, CancellationToken token = default);

		/// <summary>
		/// Получение заявок по статусу заявки.
		/// </summary>
		/// <param name="applicationStatus"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetByApplicationStatusAsync(ApplicationStatus applicationStatus, CancellationToken token = default);

		/// <summary>
		/// Получение заявок по Виду инструмента.
		/// </summary>
		/// <param name="instrumentType"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetByInstrumentTypeAsync(InstrumentType instrumentType, CancellationToken token = default);

		/// <summary>
		/// Получение заявок по Виду инструмента.
		/// </summary>
		/// <param name="instrumentKind"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetByInstrumentKindAsync(InstrumentKind instrumentKind, CancellationToken token = default);

		/// <summary>
		/// Получение заявок по Модели инструмента.
		/// </summary>
		/// <param name="instrumentModel"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetByInstrumentModelAsync(InstrumentModel instrumentModel, CancellationToken token = default);
	}
}
