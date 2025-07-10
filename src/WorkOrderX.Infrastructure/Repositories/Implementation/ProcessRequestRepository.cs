using WorkOrderX.Domain.AggregationModels.ProcessRequests;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class ProcessRequestRepository : IProcessRequestRepository
	{
		public Task AddAsync(ProcessRequest processRequest, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetAdminActiveProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetAdminHistoryProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetAllAsync(CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetByApplicationStatusAsync(ApplicationStatus applicationStatus, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetByCustomerEmployeeIdAsync(Guid customerEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetByEquipmentKindAsync(EquipmentKind equipmentKind, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetByEquipmentModelAsync(EquipmentModel equipmentModel, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetByEquipmentTypeAsync(EquipmentType equipmentType, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetByExecutorEmployeeIdAsync(Guid executorEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<ProcessRequest?> GetByIdAsync(Guid id, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetCustomerActiveProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetCustomerHistoryProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetExecutorActiveProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetExecutorHistoryProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetSupervisorActiveProcessRequestByEmloyeeId(Guid supervisorEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<ProcessRequest>> GetSupervisorHistoryProcessRequestByEmloyeeId(Guid supervisorEmployeeId, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(ProcessRequest processRequest, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}
	}
}
