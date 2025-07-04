using WorkOrderX.Domain.Contracts;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	public interface IProcessRequestRepository : IRepository<ProcessRequest>
	{
		Task<ProcessRequest?> GetByIdAsync(Guid id, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetAllAsync(CancellationToken token = default);

		Task<IEnumerable<ProcessRequest>> GetCustomerActiveProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetExecutorActiveProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetAdminActiveProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetSupervisorActiveProcessRequestByEmloyeeId(Guid supervisorEmployeeId, CancellationToken token = default);


		Task<IEnumerable<ProcessRequest>> GetCustomerDeActiveProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetExecutorDeActiveProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetAdminDeActiveProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetSupervisorDeActiveProcessRequestByEmloyeeId(Guid supervisorEmployeeId, CancellationToken token = default);


		Task AddAsync(ProcessRequest processRequest, CancellationToken token = default);
		Task UpdateAsync(ProcessRequest processRequest, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetByCustomerEmployeeIdAsync(Guid customerEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetByExecutorEmployeeIdAsync(Guid executorEmployeeId, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetByApplicationStatusAsync(ApplicationStatus applicationStatus, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetByEquipmentTypeAsync(EquipmentType equipmentType, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetByEquipmentKindAsync(EquipmentKind equipmentKind, CancellationToken token = default);
		Task<IEnumerable<ProcessRequest>> GetByEquipmentModelAsync(EquipmentModel equipmentModel, CancellationToken token = default);
	}
}
