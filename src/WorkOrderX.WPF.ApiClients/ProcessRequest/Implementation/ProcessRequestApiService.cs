using System.Text.Json;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ProcessRequest.Implementation
{
	/// <summary>
	/// Сервис для работы с API заявок.
	/// </summary>
	public class ProcessRequestApiService : BaseApiClient, IProcessRequestApiService
	{
		public ProcessRequestApiService(
			IHttpClientFactory httpClientFactory,
			JsonSerializerOptions jsonOptions)
			: base(httpClientFactory.CreateClient("WorkOrderXApi"), jsonOptions)
		{
			_httpClient.BaseAddress = new Uri(_httpClient.BaseAddress, "ProcessRequest/");
		}

		/// <summary>
		/// Создает новую заявку.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<bool> CreateProcessRequestAsync(CreateProcessRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("CreateProcessRequest", model, token);

		/// <summary>
		/// Получает активные заявки для указанного сотрудника.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequestDataModel>> GetActivProcessRequestsAsync(Guid employeeId,
			CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<ProcessRequestDataModel>>($"GetActivProcessRequests/{employeeId}", token);

		/// <summary>
		/// Получает историю заявок для указанного сотрудника.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequestDataModel>> GetHistoryProcessRequestsAsync(Guid employeeId,
			CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<ProcessRequestDataModel>>($"GetHistoryProcessRequests/{employeeId}", token);

		/// <summary>
		/// Обновляет заявку.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<bool> UpdateProcessRequestAsync(UpdateProcessRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateProcessRequest", model, token);

		/// <summary>
		/// Обновляет статус заявки на "Выполнено" или "Отклонено".
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<bool> UpdateStatusDoneOrRejectedAsync(UpdateStatusDoneOrRejectedModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateStatusDoneOrRejected", model, token);

		/// <summary>
		/// Обновляет статус заявки на "В работе", "Возвращено" или "Отложено".
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<bool> UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(UpdateStatusInWorkOrReturnedOrPostponedRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateStatusInWorkOrReturnedOrPostponedRequest", model, token);

		/// <summary>
		/// Обновляет статус заявки на "Перенаправлено".
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<bool> UpdateStatusRedirectedRequestAsync(UpdateStatusRedirectedRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateStatusRedirectedRequest", model, token);

		/// <summary>
		/// Обновляет внутренний комментарий заявки.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<bool> UpdateInternalCommentRequestAsync(UpdateInternalCommentRequestModel model, CancellationToken token = default) =>
			await PostTJsonTAsync<bool>("UpdateInternalCommentRequest", model, token);
	}
}
