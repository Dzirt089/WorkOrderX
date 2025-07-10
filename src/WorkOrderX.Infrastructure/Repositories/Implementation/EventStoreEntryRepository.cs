using WorkOrderX.Domain.Models.EventStores;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class EventStoreEntryRepository : IEventStoreEntryRepository
	{
		public Task AddEventStoreEntryAsync(EventStoreEntry entry, CancellationToken token)
		{
			throw new NotImplementedException();
		}
	}
}
