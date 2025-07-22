using WorkOrderX.EFCoreDb.Models;

namespace WorkOrderX.Infrastructure.Repositories.Interfaces
{
	public interface IAppNumberRepository
	{
		Task<AppNumber?> GetNumberAsync(CancellationToken token = default);
		Task<AppNumber> InitializationAsync(CancellationToken token = default);
		Task UpdateNumber(AppNumber number, CancellationToken token = default);
	}
}
