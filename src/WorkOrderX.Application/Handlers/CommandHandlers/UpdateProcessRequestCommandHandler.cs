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
		private readonly IReferenceDataRepository<InstrumentType> _referenceInstrumentType;
		private readonly IReferenceDataRepository<InstrumentKind> _referenceInstrumentKind;
		private readonly IReferenceDataRepository<InstrumentModel> _referenceInstrumentModel;
		private readonly IReferenceDataRepository<TypeBreakdown> _referenceTypeBreakdown;
		private readonly IReferenceDataRepository<Importance> _referenceImportance;
		private readonly IReferenceDataRepository<ApplicationStatus> _referenceApplicationStatus;


		/// <summary>
		/// Инициализирует новый экземпляр <see cref="UpdateProcessRequestCommandHandler"/>.
		/// </summary>
		/// <param name="processRequestService"></param>
		/// <param name="processRequestRepository"></param>
		public UpdateProcessRequestCommandHandler(
			IProcessRequestService processRequestService,
			IProcessRequestRepository processRequestRepository,
			IReferenceDataRepository<ApplicationType> referenceApplicationType,
			IReferenceDataRepository<InstrumentType> referenceInstrumentType,
			IReferenceDataRepository<InstrumentKind> referenceInstrumentKind,
			IReferenceDataRepository<InstrumentModel> referenceInstrumentModel,
			IReferenceDataRepository<TypeBreakdown> referenceTypeBreakdown,
			IReferenceDataRepository<Importance> referenceImportance,
			IReferenceDataRepository<ApplicationStatus> referenceApplicationStatus)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
			_referenceApplicationType = referenceApplicationType;
			_referenceInstrumentType = referenceInstrumentType;
			_referenceInstrumentKind = referenceInstrumentKind;
			_referenceInstrumentModel = referenceInstrumentModel;
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
			var applicationStatus = await _referenceApplicationStatus.GetReferenceDataByNameAsync(request.ApplicationStatus, cancellationToken);
			var importance = await _referenceImportance.GetReferenceDataByNameAsync(request.Importance, cancellationToken);
			var applicationType = await _referenceApplicationType.GetReferenceDataByNameAsync(request.ApplicationType, cancellationToken);

			var oldProcessRequest = await _processRequestRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Process request with ID {request.Id} not found.");

			ProcessRequest? processRequest = null;

			if (applicationType.Id == ApplicationType.InstrumentRepair.Id)
			{
				InstrumentType? instrumentType = await _referenceInstrumentType.GetReferenceDataByNameAsync(request.InstrumentType, cancellationToken);
				InstrumentKind? instrumentKind = await _referenceInstrumentKind.GetReferenceDataByNameAsync(request.InstrumentKind, cancellationToken);
				InstrumentModel? instrumentModel = await _referenceInstrumentModel.GetReferenceDataByNameAsync(request.InstrumentModel, cancellationToken);
				TypeBreakdown? typeBreakdown = await _referenceTypeBreakdown.GetReferenceDataByNameAsync(request.TypeBreakdown, cancellationToken);

				processRequest = await _processRequestService.UpdateInstrumentRepairRequest(
				processRequest: oldProcessRequest,
				applicationType: applicationType,
				instrumentType: instrumentType,
				instrumentKind: instrumentKind,
				instrumentModel: instrumentModel,
				serialNumber: serialNumber,
				typeBreakdown: typeBreakdown,
				descriptionMalfunction: descriptionMalfunction,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				importance: importance,
				customerEmployeeId: request.CustomerEmployeeId,
				cancellationToken);
			}
			else if (applicationType.Id == ApplicationType.HouseholdChores.Id)
			{
				Location? location = request.Location is not null ? Location.Create(request.Location) : null;

				processRequest = await _processRequestService.UpdateHouseholdChoresRequest(
				processRequest: oldProcessRequest,
				applicationType: applicationType,
				descriptionMalfunction: descriptionMalfunction,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				importance: importance,
				location: location,
				customerEmployeeId: request.CustomerEmployeeId,
				cancellationToken);
			}

			if (processRequest is null)
				throw new ApplicationException("Failed to update process request");

			await _processRequestRepository.UpdateAsync(processRequest, cancellationToken);

			return true;
		}
	}
}
