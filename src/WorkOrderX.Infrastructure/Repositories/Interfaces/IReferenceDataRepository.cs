using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.Root;

namespace WorkOrderX.Infrastructure.Repositories.Interfaces
{
	/// <summary>
	/// Репозиторий для работы с справочными данными. Ограничен типом <see cref="Enumeration"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IReferenceDataRepository<T> where T : Enumeration
	{
		/// <summary>
		/// Получает справочные данные по имени.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<T> GetReferenceDataByNameAsync(string name, CancellationToken token);

		/// <summary>
		/// Получает все справочные данные.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<T>> GetAllAsync(CancellationToken token);

		/// <summary>
		/// Получает все виды оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<EquipmentKind>> GetAllEquipmentKindAsync(CancellationToken token);

		/// <summary>
		/// Получает все виды разбивки типов оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<TypeBreakdown>> GetAllTypeBreakdownAsync(CancellationToken token);
	}
}
