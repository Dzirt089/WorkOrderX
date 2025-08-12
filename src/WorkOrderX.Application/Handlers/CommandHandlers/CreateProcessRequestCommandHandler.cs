using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;
using WorkOrderX.EFCoreDb.Models;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.CommandHandlers
{
	/// <summary>
	/// Обработчик команды для создания заявки на ремонт оборудования или хоз. работы.
	/// </summary>
	public sealed class CreateProcessRequestCommandHandler : IRequestHandler<CreateProcessRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;
		private readonly IAppNumberRepository _numberRepository;

		private readonly IReferenceDataRepository<ApplicationType> _referenceApplicationType;
		private readonly IReferenceDataRepository<InstrumentType> _referenceInstrumentType;
		private readonly IReferenceDataRepository<InstrumentKind> _referenceInstrumentKind;
		private readonly IReferenceDataRepository<InstrumentModel> _referenceInstrumentModel;
		private readonly IReferenceDataRepository<TypeBreakdown> _referenceTypeBreakdown;
		private readonly IReferenceDataRepository<Importance> _referenceImportance;
		private readonly IReferenceDataRepository<ApplicationStatus> _referenceApplicationStatus;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="CreateProcessRequestCommandHandler"/>.
		/// </summary>
		public CreateProcessRequestCommandHandler(
			IProcessRequestService processRequestService,
			IProcessRequestRepository processRequestRepository,
			IAppNumberRepository numberRepository,
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
			_numberRepository = numberRepository;
			_referenceApplicationType = referenceApplicationType;
			_referenceInstrumentType = referenceInstrumentType;
			_referenceInstrumentKind = referenceInstrumentKind;
			_referenceInstrumentModel = referenceInstrumentModel;
			_referenceTypeBreakdown = referenceTypeBreakdown;
			_referenceImportance = referenceImportance;
			_referenceApplicationStatus = referenceApplicationStatus;
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

			AppNumber appNumber = await CreateOrGetAppNumberAsync(cancellationToken)
				?? throw new ApplicationException("Failed to create or get application number");
			var createdAt = !string.IsNullOrEmpty(request.CreatedAt) ? DateTime.Parse(request.CreatedAt) : throw new ArgumentException("CreatedAt cannot be null or empty", nameof(request.CreatedAt));
			var plannedAt = !string.IsNullOrEmpty(request.PlannedAt) ? DateTime.Parse(request.PlannedAt) : throw new ArgumentException("PlannedAt cannot be null or empty", nameof(request.PlannedAt));
			var applicationNumber = ApplicationNumber.Create(appNumber.Number);
			var serialNumber = request.SerialNumber is not null ? SerialNumber.Create(request.SerialNumber) : null;
			var descriptionMalfunction = DescriptionMalfunction.Create(request.DescriptionMalfunction);
			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;
			var applicationStatus = await _referenceApplicationStatus.GetReferenceDataByNameAsync(request.ApplicationStatus, cancellationToken);
			var importance = await _referenceImportance.GetReferenceDataByNameAsync(request.Importance, cancellationToken);
			var applicationType = await _referenceApplicationType.GetReferenceDataByNameAsync(request.ApplicationType, cancellationToken);

			ProcessRequest? processRequest = null;

			if (applicationType.Id == ApplicationType.InstrumentRepair.Id)
			{
				InstrumentType? instrumentType = await _referenceInstrumentType.GetReferenceDataByNameAsync(request.InstrumentType, cancellationToken);
				InstrumentKind? instrumentKind = await _referenceInstrumentKind.GetReferenceDataByNameAsync(request.InstrumentKind, cancellationToken);
				InstrumentModel? instrumentModel = await _referenceInstrumentModel.GetReferenceDataByNameAsync(request.InstrumentModel, cancellationToken);
				TypeBreakdown? typeBreakdown = await _referenceTypeBreakdown.GetReferenceDataByNameAsync(request.TypeBreakdown, cancellationToken);

				processRequest = await _processRequestService.CreateInstrumentRepairRequest(
					applicationNumber,
					applicationType,
					createdAt,
					plannedAt,
					instrumentType,
					instrumentKind,
					instrumentModel,
					serialNumber,
					typeBreakdown,
					descriptionMalfunction,
					applicationStatus,
					internalComment,
					importance,
					request.CustomerEmployeeId,
					cancellationToken);
			}
			else if (applicationType.Id == ApplicationType.HouseholdChores.Id)
			{
				var location = request.Location is not null ? Location.Create(request.Location) : null;

				processRequest = await _processRequestService.CreateHouseholdChoresRequest(
					applicationNumber,
					applicationType,
					createdAt,
					plannedAt,
					descriptionMalfunction,
					applicationStatus,
					internalComment,
					importance,
					location,
					request.CustomerEmployeeId,
					cancellationToken);
			}

			if (processRequest is null)
				throw new ApplicationException("Failed to create process request");

			await _processRequestRepository.AddAsync(processRequest, cancellationToken);

			return true;
		}

		/// <summary>
		/// Создает или получает номер заявки.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task<AppNumber?> CreateOrGetAppNumberAsync(CancellationToken cancellationToken)
		{
			bool flagNew = false;

			var appNumber = await _numberRepository.GetNumberAsync(cancellationToken);

			if (appNumber is null)
			{
				flagNew = true;
				appNumber = await _numberRepository.InitializationAsync(cancellationToken);
			}

			if (!flagNew)
			{
				appNumber.Number++;
				await _numberRepository.UpdateNumber(appNumber, cancellationToken);
			}

			return appNumber;
		}
	}
}
