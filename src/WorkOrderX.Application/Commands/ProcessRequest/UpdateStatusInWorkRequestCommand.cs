using MediatR;

namespace WorkOrderX.Application.Commands.ProcessRequest
{
	public record UpdateStatusInWorkRequestCommand : IRequest<bool>
	{
		public Guid Id { get; init; }
		public string ApplicationStatus { get; init; }
	}
}
