using WorkOrderX.Domain.Models.EventStores;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class EventStoreEntryRepository : IEventStoreEntryRepository
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public EventStoreEntryRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task AddEventStoreEntryAsync(EventStoreEntry entry, CancellationToken token)
		{
			await _workOrderDbContext.EventStoreEntries.AddAsync(entry, token);
		}
	}
}
