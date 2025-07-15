using Microsoft.EntityFrameworkCore;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class ProcessRequestRepository : IProcessRequestRepository
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public ProcessRequestRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task AddAsync(ProcessRequest processRequest, CancellationToken token = default)
		{
			await _workOrderDbContext.ProcessRequests.AddAsync(processRequest, token);
		}

		public async Task<IEnumerable<ProcessRequest>> GetAdminActiveProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.ApplicationStatusId != ApplicationStatus.Done.Id
					&& _.ApplicationStatusId != ApplicationStatus.Rejected.Id)
					.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetAdminHistoryProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.ApplicationStatusId == ApplicationStatus.Done.Id
					|| _.ApplicationStatusId == ApplicationStatus.Rejected.Id)
					.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetAllAsync(CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetByApplicationStatusAsync(ApplicationStatus applicationStatus, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.ApplicationStatusId == applicationStatus.Id)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetByCustomerEmployeeIdAsync(Guid customerEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.CustomerEmployeeId == customerEmployeeId)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetByEquipmentKindAsync(EquipmentKind equipmentKind, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.EquipmentKindId == equipmentKind.Id)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetByEquipmentModelAsync(EquipmentModel equipmentModel, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.EquipmentModelId == equipmentModel.Id)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetByEquipmentTypeAsync(EquipmentType equipmentType, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.EquipmentTypeId == equipmentType.Id)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetByExecutorEmployeeIdAsync(Guid executorEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.ExecutorEmployeeId == executorEmployeeId)
				.ToListAsync(token);
			return result;
		}

		public async Task<ProcessRequest?> GetByIdAsync(Guid id, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.FirstOrDefaultAsync(_ => _.Id == id, token);

			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetCustomerActiveProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.ApplicationStatusId != ApplicationStatus.Done.Id
									&& _.ApplicationStatusId != ApplicationStatus.Rejected.Id
									&& _.CustomerEmployeeId == customerEmployeeId)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetCustomerHistoryProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => (_.ApplicationStatusId == ApplicationStatus.Done.Id
									|| _.ApplicationStatusId == ApplicationStatus.Rejected.Id)
									&& _.CustomerEmployeeId == customerEmployeeId)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetExecutorActiveProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => _.ApplicationStatusId != ApplicationStatus.Done.Id
									&& _.ApplicationStatusId != ApplicationStatus.Rejected.Id
									&& _.ExecutorEmployeeId == executorEmployeeId)
				.ToListAsync(token);
			return result;
		}

		public async Task<IEnumerable<ProcessRequest>> GetExecutorHistoryProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.CustomerEmployeeId)
				.Include(_ => _.ExecutorEmployeeId)
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Where(_ => (_.ApplicationStatusId == ApplicationStatus.Done.Id
									|| _.ApplicationStatusId == ApplicationStatus.Rejected.Id)
									&& _.ExecutorEmployeeId == executorEmployeeId)
				.ToListAsync(token);
			return result;
		}

		public async Task UpdateAsync(ProcessRequest processRequest, CancellationToken token = default)
		{
			_workOrderDbContext.ProcessRequests.Update(processRequest);
		}
	}
}
