using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Services.Interfaces
{
	public interface IProcessRequestService
	{
		Task<IEnumerable<ProcessRequest>> GetActiveProcessRequestsAsync(Guid employeeId, CancellationToken token = default);
	}
}
