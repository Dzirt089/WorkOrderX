using MediatR;

using WorkOrderX.Domain.Root;
using WorkOrderX.EFCoreDb.DbContexts;

namespace WorkOrderX.Application.Behaviors
{
	public class IntegrationEventsDispatchingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IMediator _mediator;
		private readonly WorkOrderDbContext _workOrderDbContext;

		public IntegrationEventsDispatchingBehavior(IMediator mediator, WorkOrderDbContext workOrderDbContext)
		{
			_mediator = mediator;
			_workOrderDbContext = workOrderDbContext;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			// Выполняем запрос
			var response = await next();

			// Собираем и публикуем интеграционные события домена
			var entities = _workOrderDbContext.ChangeTracker
				.Entries<Entity>()
				.Select(_ => _.Entity)
				.Where(_ => _.IntegrationEvents.Any())
				.ToList();

			foreach (var entity in entities)
			{
				var events = new Queue<INotification>(entity.IntegrationEvents);
				entity.ClearIntegrationEvents();

				foreach (var integrationEvent in events)
					await _mediator.Publish(integrationEvent, cancellationToken);

				//TODO: Разработать систему Retry для повторных публикаций, упавшего события. Сделать сохранение событий, что не терять их. С последующей работой с ними 
			}

			return response;
		}
	}
}
