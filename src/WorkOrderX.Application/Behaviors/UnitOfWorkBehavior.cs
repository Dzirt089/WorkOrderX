using MediatR;

using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Application.Behaviors
{
	public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{

		private readonly WorkOrderDbContext _workOrderDbContext;

		public UnitOfWorkBehavior(WorkOrderDbContext workOrderDbContext)
		{
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			await using var transaction = await _workOrderDbContext.Database.BeginTransactionAsync(cancellationToken);
			try
			{
				var response = await next();
				await _workOrderDbContext.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);
				return response;
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				throw;
			}
		}
	}
}
