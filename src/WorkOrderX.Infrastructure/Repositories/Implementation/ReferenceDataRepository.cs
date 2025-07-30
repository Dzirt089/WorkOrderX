using Microsoft.EntityFrameworkCore;

using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.Root;
using WorkOrderX.EFCoreDb.DbContexts;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	/// <summary>
	/// Реализует репозиторий для работы с справочными данными. Ограничен типом <see cref="Enumeration"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ReferenceDataRepository<T> : IReferenceDataRepository<T> where T : Enumeration
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public ReferenceDataRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		/// <summary>
		/// Получает справочные данные по имени.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<T> GetReferenceDataByNameAsync(string name, CancellationToken token)
		{
			var result = await _workOrderDbContext.Set<T>().FirstAsync(_ => _.Name == name, token);
			return result;
		}

		/// <summary>
		/// Получает все справочные данные.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
		{
			var result = await _workOrderDbContext.Set<T>()
				.AsNoTracking()
				.ToListAsync(token);

			return result;
		}

		/// <summary>
		/// Получает все виды оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<EquipmentKind>> GetAllEquipmentKindAsync(CancellationToken token)
		{
			var result = await _workOrderDbContext.Set<EquipmentKind>()
				.AsNoTracking()
				.Include(x => x.EquipmentType)
				.ToListAsync(token);

			return result;
		}

		/// <summary>
		/// Получает все виды разбивки типов оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
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
