using MediatR;

using WorkOrderX.Application.Commands.Interfaces;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Application.Behaviors
{
	public class UnitOfWorkBehavior<TIn, TOut> : IPipelineBehavior<TIn, TOut>
		where TIn : IRequest<TOut>
	{

		private readonly WorkOrderDbContext _context;

		public UnitOfWorkBehavior(WorkOrderDbContext context)
		{
			_context = context;
		}

		public async Task<TOut> Handle(TIn request, RequestHandlerDelegate<TOut> next, CancellationToken cancellationToken)
		{
			var response = await next();

			if (request is ICommand<TOut>)
				await _context.SaveChangesAsync(cancellationToken);

			return response;

		}
	}
}
