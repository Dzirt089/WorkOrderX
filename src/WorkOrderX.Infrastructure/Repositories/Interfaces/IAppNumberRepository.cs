using WorkOrderX.EFCoreDb.Models;

namespace WorkOrderX.Infrastructure.Repositories.Interfaces
{
	/// <summary>
	/// Репозиторий для работы с номерами приложений.
	/// </summary>
	public interface IAppNumberRepository
	{
		/// <summary>
		/// Получение текущего номера приложения.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<AppNumber?> GetNumberAsync(CancellationToken token = default);

		/// <summary>
		/// Инициализация номера приложения.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<AppNumber> InitializationAsync(CancellationToken token = default);

		/// <summary>
		/// Обновление номера приложения.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task UpdateNumber(AppNumber number, CancellationToken token = default);
	}
}
