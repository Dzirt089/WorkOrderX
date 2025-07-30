using Microsoft.EntityFrameworkCore;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	/// <summary>
	/// Реализация репозитория для работы с заявками.
	/// </summary>
	public class ProcessRequestRepository : IProcessRequestRepository
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public ProcessRequestRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		/// <summary>
		/// Добавление новой заявки в репозиторий.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task AddAsync(ProcessRequest processRequest, CancellationToken token = default)
		{
			await _workOrderDbContext.ProcessRequests.AddAsync(processRequest, token);
		}

		/// <summary>
		/// Получение активных заявок администратора по идентификатору сотрудника.
		/// </summary>
		/// <param name="adminEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetAdminActiveProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.ApplicationStatusId != ApplicationStatus.Done.Id
					&& _.ApplicationStatusId != ApplicationStatus.Rejected.Id)
					.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение истории заявок администратора по идентификатору сотрудника.
		/// </summary>
		/// <param name="adminEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetAdminHistoryProcessRequestByEmloyeeId(Guid adminEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.ApplicationStatusId == ApplicationStatus.Done.Id
					|| _.ApplicationStatusId == ApplicationStatus.Rejected.Id)
					.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение всех заявок из репозитория.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetAllAsync(CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение заявок по статусу заявки.
		/// </summary>
		/// <param name="applicationStatus"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetByApplicationStatusAsync(ApplicationStatus applicationStatus, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.ApplicationStatusId == applicationStatus.Id)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение заявок по идентификатору сотрудника клиента.
		/// </summary>
		/// <param name="customerEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetByCustomerEmployeeIdAsync(Guid customerEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.CustomerEmployeeId == customerEmployeeId)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение заявок по Виду оборудования.
		/// </summary>
		/// <param name="equipmentKind"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetByEquipmentKindAsync(EquipmentKind equipmentKind, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.EquipmentKindId == equipmentKind.Id)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение заявок по Модели оборудования.
		/// </summary>
		/// <param name="equipmentModel"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetByEquipmentModelAsync(EquipmentModel equipmentModel, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.EquipmentModelId == equipmentModel.Id)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение заявок по Виду оборудования.
		/// </summary>
		/// <param name="equipmentType"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetByEquipmentTypeAsync(EquipmentType equipmentType, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.EquipmentTypeId == equipmentType.Id)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение заявок по идентификатору сотрудника исполнителя.
		/// </summary>
		/// <param name="executorEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetByExecutorEmployeeIdAsync(Guid executorEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.ExecutorEmployeeId == executorEmployeeId)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение заявки по идентификатору.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<ProcessRequest?> GetByIdAsync(Guid id, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.FirstOrDefaultAsync(_ => _.Id == id, token);

			return result;
		}

		/// <summary>
		/// Получение активных заявок клиента по идентификатору сотрудника.
		/// </summary>
		/// <param name="customerEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetCustomerActiveProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.ApplicationStatusId != ApplicationStatus.Done.Id
									&& _.ApplicationStatusId != ApplicationStatus.Rejected.Id
									&& _.CustomerEmployeeId == customerEmployeeId)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение истории заявок клиента по идентификатору сотрудника.
		/// </summary>
		/// <param name="customerEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetCustomerHistoryProcessRequestByEmloyeeId(Guid customerEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => (_.ApplicationStatusId == ApplicationStatus.Done.Id
									|| _.ApplicationStatusId == ApplicationStatus.Rejected.Id)
									&& _.CustomerEmployeeId == customerEmployeeId)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение активных заявок исполнителя по идентификатору сотрудника.
		/// </summary>
		/// <param name="executorEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetExecutorActiveProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => _.ApplicationStatusId != ApplicationStatus.Done.Id
									&& _.ApplicationStatusId != ApplicationStatus.Rejected.Id
									&& _.ExecutorEmployeeId == executorEmployeeId)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Получение истории заявок исполнителя по идентификатору сотрудника.
		/// </summary>
		/// <param name="executorEmployeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetExecutorHistoryProcessRequestByEmloyeeId(Guid executorEmployeeId, CancellationToken token = default)
		{
			var result = await _workOrderDbContext.ProcessRequests
				.AsSplitQuery()
				.Include(_ => _.EquipmentType)
				.Include(_ => _.EquipmentKind)
				.Include(_ => _.TypeBreakdown)
				.Include(_ => _.EquipmentModel)
				.Include(_ => _.ApplicationStatus)
				.Include(_ => _.ApplicationType)
				.Include(_ => _.Importance)
				.Where(_ => (_.ApplicationStatusId == ApplicationStatus.Done.Id
									|| _.ApplicationStatusId == ApplicationStatus.Rejected.Id)
									&& _.ExecutorEmployeeId == executorEmployeeId)
				.ToListAsync(token);
			return result;
		}

		/// <summary>
		/// Обновление заявки в репозитории.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task UpdateAsync(ProcessRequest processRequest, CancellationToken token = default)
		{
			_workOrderDbContext.ProcessRequests.Update(processRequest);
		}
	}
}
