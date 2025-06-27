namespace WorkOrderX.Domain.AggregationModels.EventStores
{
	public interface IEventStoreEntryRepository
	{
		Task AddEventStoreEntryAsync(EventStoreEntry entry, CancellationToken token);
	}
}
