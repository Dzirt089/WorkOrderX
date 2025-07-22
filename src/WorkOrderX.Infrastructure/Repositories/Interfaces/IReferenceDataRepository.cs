using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.Root;

namespace WorkOrderX.Infrastructure.Repositories.Interfaces
{
	public interface IReferenceDataRepository<T> where T : Enumeration
	{
		Task<T> GetReferenceDataByNameAsync(string name, CancellationToken token);
		Task<IEnumerable<T>> GetAllAsync(CancellationToken token);

		Task<IEnumerable<EquipmentKind>> GetAllEquipmentKindAsync(CancellationToken token);

		Task<IEnumerable<TypeBreakdown>> GetAllTypeBreakdownAsync(CancellationToken token);
	}
}
