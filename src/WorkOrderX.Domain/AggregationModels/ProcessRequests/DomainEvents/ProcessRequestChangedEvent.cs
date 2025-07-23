using MediatR;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests.DomainEvents
{
	public sealed record ProcessRequestChangedEvent : INotification
	{
		/// <summary>
		/// ID заявки
		/// </summary>
		public Guid RequestId { get; init; }
	}
}