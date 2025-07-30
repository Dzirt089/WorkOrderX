using WorkOrderX.Domain.Models.EventStores;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	/// <summary>
	/// Реализация репозитория для работы с записями в хранилище событий.
	/// </summary>
	public class EventStoreEntryRepository : IEventStoreEntryRepository
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public EventStoreEntryRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		/// <summary>
		/// Добавление новой записи в хранилище событий.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task AddEventStoreEntryAsync(EventStoreEntry entry, CancellationToken token)
		{
			await _workOrderDbContext.EventStoreEntries.AddAsync(entry, token);
		}
	}
}
