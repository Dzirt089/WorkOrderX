using MediatR;

namespace WorkOrderX.Application.Commands.ProcessRequest
{
	public record CreateProcessRequestCommand : IRequest<bool>
	{
		public long ApplicationNumber { get; init; }
		public string ApplicationType { get; init; }
		public string CreatedAt { get; init; }
		public string PlannedAt { get; init; }
		public string? EquipmentType { get; init; }
		public string? EquipmentKind { get; init; }
		public string? EquipmentModel { get; init; }
		public string? serialNumber { get; init; }
		public string TypeBreakdown { get; init; }
		public string DescriptionMalfunction { get; init; }
		public string ApplicationStatus { get; init; }
		public string? InternalComment { get; init; }
		public Guid CustomerEmployeeId { get; init; }
	}
}
