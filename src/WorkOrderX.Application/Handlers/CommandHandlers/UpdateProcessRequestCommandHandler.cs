using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.CommandHandlers
{
	/// <summary>
	/// Обработчик команды для обновления заявки на ремонт оборудования или хоз. работы.
	/// </summary>
	public sealed class UpdateProcessRequestCommandHandler : IRequestHandler<UpdateProcessRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		private readonly IReferenceDataRepository<ApplicationType> _referenceApplicationType;
		private readonly IReferenceDataRepository<EquipmentType> _referenceEquipmentType;
		private readonly IReferenceDataRepository<EquipmentKind> _referenceEquipmentKind;
		private readonly IReferenceDataRepository<EquipmentModel> _referenceEquipmentModel;
		private readonly IReferenceDataRepository<TypeBreakdown> _referenceTypeBreakdown;
		private readonly IReferenceDataRepository<Importance> _referenceImportance;
		private readonly IReferenceDataRepository<ApplicationStatus> _referenceApplicationStatus;


		/// <summary>
		/// Инициализирует новый экземпляр <see cref="UpdateProcessRequestCommandHandler"/>.
		/// </summary>
		/// <param name="processRequestService"></param>
		/// <param name="processRequestRepository"></param>
		public UpdateProcessRequestCommandHandler(IProcessRequestService processRequestService, IProcessRequestRepository processRequestRepository, IReferenceDataRepository<ApplicationType> referenceApplicationType, IReferenceDataRepository<EquipmentType> referenceEquipmentType, IReferenceDataRepository<EquipmentKind> referenceEquipmentKind, IReferenceDataRepository<EquipmentModel> referenceEquipmentModel, IReferenceDataRepository<TypeBreakdown> referenceTypeBreakdown, IReferenceDataRepository<Importance> referenceImportance, IReferenceDataRepository<ApplicationStatus> referenceApplicationStatus)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
			_referenceApplicationType = referenceApplicationType;
			_referenceEquipmentType = referenceEquipmentType;
			_referenceEquipmentKind = referenceEquipmentKind;
			_referenceEquipmentModel = referenceEquipmentModel;
			_referenceTypeBreakdown = referenceTypeBreakdown;
			_referenceImportance = referenceImportance;
			_referenceApplicationStatus = referenceApplicationStatus;
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


			var serialNumber = request.SerialNumber is not null ? SerialNumber.Create(request.SerialNumber) : null;
			var descriptionMalfunction = DescriptionMalfunction.Create(request.DescriptionMalfunction);
			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;


			var applicationType = await _referenceApplicationType.GetReferenceDataByNameAsync(request.ApplicationType, cancellationToken);

			var equipmentType = await _referenceEquipmentType.GetReferenceDataByNameAsync(request.EquipmentType, cancellationToken);

			var equipmentKind = await _referenceEquipmentKind.GetReferenceDataByNameAsync(request.EquipmentKind, cancellationToken);

			var equipmentModel = await _referenceEquipmentModel.GetReferenceDataByNameAsync(request.EquipmentModel, cancellationToken);

			var typeBreakdown = await _referenceTypeBreakdown.GetReferenceDataByNameAsync(request.TypeBreakdown, cancellationToken);

			var applicationStatus = await _referenceApplicationStatus.GetReferenceDataByNameAsync(request.ApplicationStatus, cancellationToken);

			var importance = await _referenceImportance.GetReferenceDataByNameAsync(request.Importance, cancellationToken);

			var oldProcessRequest = await _processRequestRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Process request with ID {request.Id} not found.");

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
				importance: importance,
				customerEmployeeId: request.CustomerEmployeeId,
				cancellationToken);

			if (updatedProcessRequest is null)
				throw new ApplicationException("Failed to update process request");

			await _processRequestRepository.UpdateAsync(updatedProcessRequest, cancellationToken);

			return true;
		}
	}
}
