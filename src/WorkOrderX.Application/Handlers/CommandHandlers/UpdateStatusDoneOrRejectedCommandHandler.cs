using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.Application.Handlers.CommandHandlers
{
	/// <summary>
	/// Обработчик команды для обновления статуса заявки на "Выполнено" или "Отклонено".
	/// </summary>
	public sealed class UpdateStatusDoneOrRejectedCommandHandler : IRequestHandler<UpdateStatusDoneOrRejectedCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		/// <summary>
		/// Инициализирует новый экземпляр <see cref="UpdateStatusDoneOrRejectedCommandHandler"/>.
		/// </summary>
		/// <param name="processRequestService"></param>
		/// <param name="processRequestRepository"></param>
		public UpdateStatusDoneOrRejectedCommandHandler(IProcessRequestService processRequestService, IProcessRequestRepository processRequestRepository)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
		}

		/// <summary>
		/// Обрабатывает команду для обновления статуса заявки на "Выполнено" или "Отклонено".
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<bool> Handle(UpdateStatusDoneOrRejectedCommand request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request), "Request cannot be null");
			if (request.Id == Guid.Empty)
				throw new ArgumentException("Request ID cannot be empty", nameof(request.Id));
			if (string.IsNullOrEmpty(request.CompletedAt))
				throw new ArgumentException("CompletedAt cannot be null or empty", nameof(request.CompletedAt));


			var oldProcessRequest = await _processRequestRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Process request with ID {request.Id} not found.");

			var applicationStatus = ApplicationStatus.FromName<ApplicationStatus>(request.ApplicationStatus);
			var internalComment = request.InternalComment is not null ? InternalComment.Create(request.InternalComment) : null;
			var completedAt = DateTime.Parse(request.CompletedAt);

			var newProcessRequest = _processRequestService.SetRequestDoneOrRejected(
				processRequest: oldProcessRequest,
				completionAt: completedAt,
				applicationStatus: applicationStatus,
				internalComment: internalComment);

			if (newProcessRequest is null)
				throw new ApplicationException("Failed to update process request status");

			await _processRequestRepository.UpdateAsync(newProcessRequest, cancellationToken);
			return true;
		}
	}
}
