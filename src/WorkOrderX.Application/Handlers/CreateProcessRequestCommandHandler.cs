using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.Employees;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.Application.Handlers
{
	public sealed class CreateProcessRequestCommandHandler : IRequestHandler<CreateProcessRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		public CreateProcessRequestCommandHandler(IProcessRequestService processRequestService, IWorkplaceEmployeesRepository workplaceEmployeesRepository, IProcessRequestRepository processRequestRepository)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
		}

		public async Task<bool> Handle(CreateProcessRequestCommand request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request), "Request cannot be null");

			var applicationNumber = ApplicationNumber.Create(request.ApplicationNumber);
			var applicationType = ApplicationType.Parse(request.ApplicationType);
			var createdAt = DateTime.Parse(request.CreatedAt);
			var plannedAt = DateTime.Parse(request.PlannedAt);
			var equipmentType = request.EquipmentType is not null ? EquipmentType.Parse(request.EquipmentType) : null;
			var equipmentKind = request.EquipmentKind is not null ? EquipmentKind.Parse(request.EquipmentKind) : null;
			var equipmentModel = request.EquipmentModel is not null ? EquipmentModel.Parse(request.EquipmentModel) : null;
			var serialNumber = request.serialNumber;
			var typeBreakdown = TypeBreakdown.Parse(request.TypeBreakdown);
			var descriptionMalfunction = DescriptionMalfunction.Create(request.DescriptionMalfunction);
			var applicationStatus = ApplicationStatus.Parse(request.ApplicationStatus);
			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;

			var newProcessRequest = await _processRequestService.CreateProcessRequest(
				applicationNumber,
				applicationType,
				createdAt,
				plannedAt,
				equipmentType,
				equipmentKind,
				equipmentModel,
				serialNumber,
				typeBreakdown,
				descriptionMalfunction,
				applicationStatus,
				internalComment,
				request.CustomerEmployeeId,
				cancellationToken);

			if (newProcessRequest is null)
				throw new ApplicationException("Failed to create process request");

			await _processRequestRepository.AddAsync(newProcessRequest, cancellationToken);

			return true;
		}
	}
}
