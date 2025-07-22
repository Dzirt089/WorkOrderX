using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.Application.Handlers.CommandHandlers
{
	/// <summary>
	/// Обработчик команды для обновления статуса заявки на "Перенаправлено".
	/// </summary>
	public sealed class UpdateStatusRedirectedRequestCommandHandler : IRequestHandler<UpdateStatusRedirectedRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		private readonly IReferenceDataRepository<ApplicationStatus> _referenceApplicationStatus;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="UpdateStatusRedirectedRequestCommandHandler"/>.
		/// </summary>
		/// <param name="processRequestService"></param>
		/// <param name="processRequestRepository"></param>
		public UpdateStatusRedirectedRequestCommandHandler(IProcessRequestService processRequestService, IProcessRequestRepository processRequestRepository, IReferenceDataRepository<ApplicationStatus> referenceApplicationStatus)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
			_referenceApplicationStatus = referenceApplicationStatus;
		}

		/// <summary>
		/// Обрабатывает команду для обновления статуса заявки на "Перенаправлено".
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<bool> Handle(UpdateStatusRedirectedRequestCommand request, CancellationToken cancellationToken)
		{

			if (request is null)
				throw new ArgumentNullException(nameof(request), "Request cannot be null");

			if (request.Id == Guid.Empty)
				throw new ArgumentException("Request ID cannot be empty", nameof(request.Id));


			var oldProcessRequest = await _processRequestRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Process request with ID {request.Id} not found.");

			var applicationStatus = await _referenceApplicationStatus.GetReferenceDataByNameAsync(request.ApplicationStatus, cancellationToken);

			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;

			var newProcessRequest = await _processRequestService.GetReassignmentExecutorEmployeeIdAsync(
				processRequest: oldProcessRequest,
				executerEmployeeID: request.ExecutorEmployeeId,
				applicationStatus: applicationStatus,
				internalComment: internalComment,
				token: cancellationToken);

			if (newProcessRequest is null)
				throw new ApplicationException("Failed to update process request status");

			await _processRequestRepository.UpdateAsync(newProcessRequest, cancellationToken);
			return true;
		}
	}
}
