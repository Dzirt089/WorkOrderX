using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.Application.Handlers.CommandHandlers
{
	/// <summary>
	/// Обработчик команды для создания заявки на ремонт оборудования или хоз. работы.
	/// </summary>
	public sealed class CreateProcessRequestCommandHandler : IRequestHandler<CreateProcessRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="CreateProcessRequestCommandHandler"/>.
		/// </summary>
		/// <param name="processRequestService"></param>
		/// <param name="workplaceEmployeesRepository"></param>
		/// <param name="processRequestRepository"></param>
		public CreateProcessRequestCommandHandler(IProcessRequestService processRequestService, IWorkplaceEmployeesRepository workplaceEmployeesRepository, IProcessRequestRepository processRequestRepository)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
		}

		/// <summary>
		/// Обрабатывает команду для создания заявки на ремонт оборудования или хоз. работы.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<bool> Handle(CreateProcessRequestCommand request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request), "Request cannot be null");

			var applicationNumber = ApplicationNumber.Create(request.ApplicationNumber);
			var applicationType = ApplicationType.FromName<ApplicationType>(request.ApplicationType);
			var createdAt = DateTime.Parse(request.CreatedAt);
			var plannedAt = DateTime.Parse(request.PlannedAt);
			var equipmentType = request.EquipmentType is not null ? EquipmentType.FromName<EquipmentType>(request.EquipmentType) : null;
			var equipmentKind = request.EquipmentKind is not null ? EquipmentKind.FromName<EquipmentKind>(request.EquipmentKind) : null;
			var equipmentModel = request.EquipmentModel is not null ? EquipmentModel.FromName<EquipmentModel>(request.EquipmentModel) : null;
			var serialNumber = request.SerialNumber is not null ? SerialNumber.Create(request.SerialNumber) : null;
			var typeBreakdown = TypeBreakdown.FromName<TypeBreakdown>(request.TypeBreakdown);
			var descriptionMalfunction = DescriptionMalfunction.Create(request.DescriptionMalfunction);
			var applicationStatus = ApplicationStatus.FromName<ApplicationStatus>(request.ApplicationStatus);
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
