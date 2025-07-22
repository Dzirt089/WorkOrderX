using Microsoft.EntityFrameworkCore;

using WorkOrderX.EFCoreDb.DbContexts;
using WorkOrderX.EFCoreDb.Models;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Infrastructure.Repositories.Implementation
{
	public class AppNumberRepository : IAppNumberRepository
	{
		private readonly WorkOrderDbContext _workOrderDbContext;

		public AppNumberRepository(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task<AppNumber?> GetNumberAsync(CancellationToken token = default)
		{
			AppNumber? result = await _workOrderDbContext.Numbers
				.FirstOrDefaultAsync(_ => _.Id == 1, token);
			return result;
		}

		public async Task<AppNumber> InitializationAsync(CancellationToken token = default)
		{
			AppNumber number = new() { Id = 1, Number = 1 };
			await _workOrderDbContext.Numbers.AddAsync(number, token);
			return number;
		}

		public async Task UpdateNumber(AppNumber number, CancellationToken token = default)
		{
			_workOrderDbContext.Numbers.Update(number);
		}
	}
}
