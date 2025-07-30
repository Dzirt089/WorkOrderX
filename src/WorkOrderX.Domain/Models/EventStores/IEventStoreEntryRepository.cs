namespace WorkOrderX.Domain.Models.EventStores
{
	/// <summary>
	/// Репозиторий для работы с записями в хранилище событий.
	/// </summary>
	public interface IEventStoreEntryRepository
	{
		/// <summary>
		/// Добавление новой записи в хранилище событий.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		Task AddEventStoreEntryAsync(EventStoreEntry entry, CancellationToken token);
	}
}
