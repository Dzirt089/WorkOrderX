using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;
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
		private readonly IReferenceDataRepository<EquipmentType> _referenceEquipmentType;
		private readonly IReferenceDataRepository<EquipmentKind> _referenceEquipmentKind;
		private readonly IReferenceDataRepository<EquipmentModel> _referenceEquipmentModel;
		private readonly IReferenceDataRepository<TypeBreakdown> _referenceTypeBreakdown;
		private readonly IReferenceDataRepository<Importance> _referenceImportance;
		private readonly IReferenceDataRepository<ApplicationStatus> _referenceApplicationStatus;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="CreateProcessRequestCommandHandler"/>.
		/// </summary>
		public CreateProcessRequestCommandHandler(IProcessRequestService processRequestService, IProcessRequestRepository processRequestRepository, IAppNumberRepository numberRepository, IReferenceDataRepository<ApplicationType> referenceApplicationType, IReferenceDataRepository<EquipmentType> referenceEquipmentType, IReferenceDataRepository<EquipmentKind> referenceEquipmentKind, IReferenceDataRepository<EquipmentModel> referenceEquipmentModel, IReferenceDataRepository<TypeBreakdown> referenceTypeBreakdown, IReferenceDataRepository<Importance> referenceImportance, IReferenceDataRepository<ApplicationStatus> referenceApplicationStatus)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
			_numberRepository = numberRepository;
			_referenceApplicationType = referenceApplicationType;
			_referenceEquipmentType = referenceEquipmentType;
			_referenceEquipmentKind = referenceEquipmentKind;
			_referenceEquipmentModel = referenceEquipmentModel;
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


			var createdAt = DateTime.Parse(request.CreatedAt);
			var plannedAt = DateTime.Parse(request.PlannedAt);


			var applicationNumber = ApplicationNumber.Create(appNumber.Number);
			var serialNumber = request.SerialNumber is not null ? SerialNumber.Create(request.SerialNumber) : null;
			var descriptionMalfunction = DescriptionMalfunction.Create(request.DescriptionMalfunction);
			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;
			var location = request.Location is not null ? Location.Create(request.Location) : null;



			var applicationStatus = await _referenceApplicationStatus.GetReferenceDataByNameAsync(request.ApplicationStatus, cancellationToken);
			var importance = await _referenceImportance.GetReferenceDataByNameAsync(request.Importance, cancellationToken);
			var applicationType = await _referenceApplicationType.GetReferenceDataByNameAsync(request.ApplicationType, cancellationToken);

			EquipmentType? equipmentType = null;
			EquipmentKind? equipmentKind = null;
			EquipmentModel? equipmentModel = null;
			TypeBreakdown? typeBreakdown = null;

			if (applicationType.Id == ApplicationType.EquipmentRepair.Id)
			{
				equipmentType = await _referenceEquipmentType.GetReferenceDataByNameAsync(request.EquipmentType, cancellationToken);
				equipmentKind = await _referenceEquipmentKind.GetReferenceDataByNameAsync(request.EquipmentKind, cancellationToken);
				equipmentModel = await _referenceEquipmentModel.GetReferenceDataByNameAsync(request.EquipmentModel, cancellationToken);
				typeBreakdown = await _referenceTypeBreakdown.GetReferenceDataByNameAsync(request.TypeBreakdown, cancellationToken);
			}


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
				importance,
				location,
				request.CustomerEmployeeId,
				cancellationToken);

			if (newProcessRequest is null)
				throw new ApplicationException("Failed to create process request");

			await _processRequestRepository.AddAsync(newProcessRequest, cancellationToken);

			return true;
		}
	}
}
