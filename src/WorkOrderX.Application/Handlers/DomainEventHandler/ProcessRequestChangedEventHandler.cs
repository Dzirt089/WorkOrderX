using MediatR;

using Microsoft.AspNetCore.SignalR;

using WorkOrderX.Application.Hubs;
using WorkOrderX.Domain.AggregationModels.ProcessRequests.DomainEvents;

namespace WorkOrderX.Application.Handlers.DomainEventHandler
{
	public class ProcessRequestChangedEventHandler : INotificationHandler<ProcessRequestChangedEvent>
	{
		private readonly IHubContext<ProcessRequestChangedHub> _hubContext;

		public ProcessRequestChangedEventHandler(IHubContext<ProcessRequestChangedHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task Handle(ProcessRequestChangedEvent notification, CancellationToken cancellationToken)
		{
			await _hubContext.Clients.All.SendAsync("ProcessRequestChanged", notification.RequestId, cancellationToken);
		}
	}
}
