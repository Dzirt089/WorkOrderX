using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.Application.Handlers.CommandHandlers
{
	/// <summary>
	/// Обработчик команды для обновления заявки на ремонт оборудования или хоз. работы.
	/// </summary>
	public sealed class UpdateProcessRequestCommandHandler : IRequestHandler<UpdateProcessRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="UpdateProcessRequestCommandHandler"/>.
		/// </summary>
		/// <param name="processRequestService"></param>
		/// <param name="processRequestRepository"></param>
		public UpdateProcessRequestCommandHandler(IProcessRequestService processRequestService, IProcessRequestRepository processRequestRepository)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
		}

		/// <summary>
		/// Обрабатывает команду для обновления заявки на ремонт оборудования или хоз. работы.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<bool> Handle(UpdateProcessRequestCommand request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request), "Request cannot be null");

			if (request.Id == Guid.Empty)
				throw new ArgumentException("Request ID cannot be empty", nameof(request.Id));


			var oldProcessRequest = await _processRequestRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Process request with ID {request.Id} not found.");

			var applicationType = ApplicationType.Parse(request.ApplicationType);
			var equipmentType = request.EquipmentType is not null ? EquipmentType.Parse(request.EquipmentType) : null;
			var equipmentKind = request.EquipmentKind is not null ? EquipmentKind.Parse(request.EquipmentKind) : null;
			var equipmentModel = request.EquipmentModel is not null ? EquipmentModel.Parse(request.EquipmentModel) : null;
			var serialNumber = request.SerialNumber;
			var typeBreakdown = TypeBreakdown.Parse(request.TypeBreakdown);
			var descriptionMalfunction = DescriptionMalfunction.Create(request.DescriptionMalfunction);
			var applicationStatus = ApplicationStatus.Parse(request.ApplicationStatus);
			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;

			var updatedProcessRequest = await _processRequestService.UpdateProcessRequest(
				processRequest: oldProcessRequest,
				applicationType: applicationType,
				equipmentType: equipmentType,
				equipmentKind: equipmentKind,
				equipmentModel: equipmentModel,
				serialNumber: serialNumber,
				typeBreakdown: typeBreakdown,
				descriptionMalfunction: descriptionMalfunction,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				customerEmployeeId: request.CustomerEmployeeId,
				cancellationToken);

			if (updatedProcessRequest is null)
				throw new ApplicationException("Failed to update process request");

			await _processRequestRepository.UpdateAsync(updatedProcessRequest, cancellationToken);

			return true;
		}
	}
}
