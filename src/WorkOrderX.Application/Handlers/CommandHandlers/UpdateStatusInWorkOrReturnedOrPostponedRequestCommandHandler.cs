using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.Application.Handlers.CommandHandlers
{
	/// <summary>
	/// Обработчик команды для обновления статуса заявки на "В работе", "Возвращено" или "Отложено".
	/// </summary>
	public sealed class UpdateStatusInWorkOrReturnedOrPostponedRequestCommandHandler : IRequestHandler<UpdateStatusInWorkOrReturnedOrPostponedRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="UpdateStatusInWorkOrReturnedOrPostponedRequestCommandHandler"/>.
		/// </summary>
		/// <param name="processRequestService"></param>
		/// <param name="processRequestRepository"></param>
		public UpdateStatusInWorkOrReturnedOrPostponedRequestCommandHandler(IProcessRequestService processRequestService, IProcessRequestRepository processRequestRepository)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
		}

		/// <summary>
		/// Обрабатывает команду для обновления статуса заявки на "В работе", "Возвращено" или "Отложено".
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<bool> Handle(UpdateStatusInWorkOrReturnedOrPostponedRequestCommand request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request), "Request cannot be null");
			if (request.Id == Guid.Empty)
				throw new ArgumentException("Request ID cannot be empty", nameof(request.Id));


			var oldProcessRequest = await _processRequestRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Process request with ID {request.Id} not found.");

			var applicationStatus = ApplicationStatus.Parse(request.ApplicationStatus);
			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;

			ProcessRequest? newProcessRequest;

			if (applicationStatus == ApplicationStatus.InWork)
			{
				newProcessRequest = _processRequestService.SetStatusInWork(
				processRequest: oldProcessRequest,
				internalComment: internalComment,
				applicationStatus: applicationStatus);
			}
			else if (applicationStatus == ApplicationStatus.Returned || applicationStatus == ApplicationStatus.Postponed)
			{
				newProcessRequest = _processRequestService.GetSetStatusReturnedOrPostponed(
				processRequest: oldProcessRequest,
				applicationStatus: applicationStatus,
				internalComment: internalComment);
			}
			else
			{
				throw new ApplicationException($"Unsupported application status: {applicationStatus}");
			}

			if (newProcessRequest is null)
				throw new ApplicationException("Failed to update process request status");

			await _processRequestRepository.UpdateAsync(newProcessRequest, cancellationToken);
			return true;
		}
	}
}
