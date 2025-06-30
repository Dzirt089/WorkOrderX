using MediatR;

namespace WorkOrderX.Application.Commands.ProcessRequest
{
	public record UpdateStatusRedirectedRequestCommand : IRequest<bool>
	{
		public Guid Id { get; init; }
		public string ApplicationStatus { get; init; }
		public string? InternalComment { get; init; }
		public Guid ExecutorEmployeeId { get; init; }
	}
}
