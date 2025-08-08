using MediatR;

using WorkOrderX.Application.Commands.Interfaces;
using WorkOrderX.Domain.Root;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Application.Behaviors
{
	public class IntegrationEventsDispatchingBehavior<TIn, TOut> : IPipelineBehavior<TIn, TOut>
		where TIn : IRequest<TOut>
	{
		private readonly IMediator _mediator;
		private readonly WorkOrderDbContext _context;

		public IntegrationEventsDispatchingBehavior(IMediator mediator, WorkOrderDbContext context)
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
				await DispatchingIntegrationEvents(cancellationToken);
			}

			return response;
		}

		private async Task DispatchingIntegrationEvents(CancellationToken cancellationToken)
		{
			while (HasUnpublishedIntegrationEvents())
			{
				// Собираем и публикуем интеграционные события домена
				var entities = _context.ChangeTracker
					.Entries<Entity>()
					.Where(_ => _.Entity.IntegrationEvents.Any())
					.Select(_ => _.Entity)

					.ToList();

				var integrationEvents = entities
					.SelectMany(_ => _.IntegrationEvents)
					.ToList();

				entities.ForEach(_ => _.ClearIntegrationEvents());

				foreach (var integrationEvent in integrationEvents)
				{
					await _mediator.Publish(integrationEvent, cancellationToken);
				}
			}
		}

		private bool HasUnpublishedIntegrationEvents() => _context.ChangeTracker
			.Entries<Entity>()
			.Any(_ => _.Entity.IntegrationEvents.Any());
	}
}
