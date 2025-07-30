using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ProcessRequest.Interfaces
{
	/// <summary>
	/// Интерфейс для работы с API заявок.
	/// </summary>
	public interface IProcessRequestApiService
	{
		/// <summary>
		/// Создает новую заявку.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<bool> CreateProcessRequestAsync(CreateProcessRequestModel model, CancellationToken token = default);

		/// <summary>
		/// Получает активные заявки для указанного сотрудника.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequestDataModel>> GetActivProcessRequestsAsync(Guid employeeId, CancellationToken token = default);

		/// <summary>
		/// Получает историю заявок для указанного сотрудника.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequestDataModel>> GetHistoryProcessRequestsAsync(Guid employeeId, CancellationToken token = default);

		/// <summary>
		/// Обновляет заявку.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<bool> UpdateProcessRequestAsync(UpdateProcessRequestModel model, CancellationToken token = default);

		/// <summary>
		/// Обновляет статус заявки на "Выполнено" или "Отклонено".
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<bool> UpdateStatusDoneOrRejectedAsync(UpdateStatusDoneOrRejectedModel model, CancellationToken token = default);

		/// <summary>
		/// Обновляет статус заявки на "В работе", "Возвращено" или "Отложено".
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<bool> UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(UpdateStatusInWorkOrReturnedOrPostponedRequestModel model, CancellationToken token = default);

		/// <summary>
		/// Обновляет статус заявки на "Перенаправлено".
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<bool> UpdateStatusRedirectedRequestAsync(UpdateStatusRedirectedRequestModel model, CancellationToken token = default);

		/// <summary>
		/// Обновляет внутренний комментарий заявки.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<bool> UpdateInternalCommentRequestAsync(UpdateInternalCommentRequestModel model, CancellationToken token = default);
	}
}
