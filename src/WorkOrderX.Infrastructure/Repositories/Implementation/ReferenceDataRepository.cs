using Microsoft.EntityFrameworkCore;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.Root;
using WorkOrderX.EFCoreDb.DbContexts;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class ReferenceDataRepository<T> : IReferenceDataRepository<T> where T : Enumeration
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public ReferenceDataRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
		{
			var result = await _workOrderDbContext.Set<T>()
				.AsNoTracking()
				.ToListAsync(token);

			return result;
		}

		public async Task<IEnumerable<EquipmentKind>> GetAllEquipmentKindAsync(CancellationToken token)
		{
			var result = await _workOrderDbContext.Set<EquipmentKind>()
				.AsNoTracking()
				.Include(x => x.EquipmentType)
				.ToListAsync(token);

			return result;
		}

		public async Task<IEnumerable<TypeBreakdown>> GetAllTypeBreakdownAsync(CancellationToken token)
		{
			var result = await _workOrderDbContext.Set<TypeBreakdown>()
				.AsNoTracking()
				.Include(x => x.EquipmentType)
				.ToListAsync(token);

			return result;
		}
	}
}
