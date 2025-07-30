using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Services.Interfaces
{
	/// <summary>
	/// Интерфейс для работы с активными заявками на ремонт.
	/// </summary>
	public interface IProcessRequestService
	{
		/// <summary>
		/// Получение активных заявок на ремонт для указанного сотрудника.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetActiveProcessRequestsAsync(Guid employeeId, CancellationToken token = default);


		/// <summary>
		/// Получение истории заявок на ремонт для указанного сотрудника.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ProcessRequest>> GetHistoryProcessRequestsAsync(Guid employeeId, CancellationToken token = default);
	}
}
