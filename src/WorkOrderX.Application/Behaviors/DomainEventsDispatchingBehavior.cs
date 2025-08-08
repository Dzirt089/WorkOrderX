using MediatR;

using WorkOrderX.Application.Commands.Interfaces;
using WorkOrderX.Domain.Root;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Application.Behaviors
{
	public class DomainEventsDispatchingBehavior<TIn, TOut> : IPipelineBehavior<TIn, TOut>
		where TIn : IRequest<TOut>
	{
		private readonly IMediator _mediator;
		private readonly WorkOrderDbContext _context;

		public DomainEventsDispatchingBehavior(IMediator mediator, WorkOrderDbContext context)
		{
			_mediator = mediator;
			_context = context;
		}

		public async Task<TOut> Handle(TIn request, RequestHandlerDelegate<TOut> next, CancellationToken cancellationToken)
		{
			// Выполняем запрос
			var response = await next();

			if (request is ICommand<TOut>)
			{
				await DispatchingDomainEvents(cancellationToken);
			}

			return response;
		}

		private async Task DispatchingDomainEvents(CancellationToken cancellationToken)
		{
			while (HasUnpublishedDomainEvents())
			{
				// Собираем и публикуем события домена
				var entities = _context.ChangeTracker
					.Entries<Entity>()
					.Where(a => a.Entity.DomainEvents.Any())
					.Select(a => a.Entity)
					.ToList();

				var domainEvents = entities
					.SelectMany(_ => _.DomainEvents)
					.ToList();

				entities.ForEach(_ => _.ClearDomainEvents());

				foreach (var domainEvent in domainEvents)
				{
					await _mediator.Publish(domainEvent, cancellationToken);
				}
			}
		}

		private bool HasUnpublishedDomainEvents() => _context.ChangeTracker
			.Entries<Entity>()
			.Any(a => a.Entity.DomainEvents.Any());
	}
}
