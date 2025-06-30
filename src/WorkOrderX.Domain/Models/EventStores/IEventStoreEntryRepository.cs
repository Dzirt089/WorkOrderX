namespace WorkOrderX.Domain.Models.EventStores
{
	public interface IEventStoreEntryRepository
	{
		Task AddEventStoreEntryAsync(EventStoreEntry entry, CancellationToken token);
	}
}
