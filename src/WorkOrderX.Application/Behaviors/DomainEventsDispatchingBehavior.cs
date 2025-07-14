using MediatR;

using WorkOrderX.Domain.Root;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Application.Behaviors
{
	public class DomainEventsDispatchingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IMediator _mediator;
		private readonly WorkOrderDbContext _workOrderDbContext;

		public DomainEventsDispatchingBehavior(IMediator mediator, WorkOrderDbContext workOrderDbContext)
		{
			_mediator = mediator;
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			// Выполняем запрос
			var response = await next();

			// Собираем и публикуем события домена
			var entities = _workOrderDbContext.ChangeTracker
				.Entries<Entity>()
				.Select(_ => _.Entity)
				.Where(_ => _.DomainEvents.Any())
				.ToList();

			foreach (var entity in entities)
			{
				var events = entity.DomainEvents;
				foreach (var domainEvent in events)
					await _mediator.Publish(domainEvent, cancellationToken);

				entity.ClearDomainEvents();
			}

			return response;
		}
	}
}
