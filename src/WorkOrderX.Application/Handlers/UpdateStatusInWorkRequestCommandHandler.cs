using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.Application.Handlers
{
	public sealed class UpdateStatusInWorkRequestCommandHandler : IRequestHandler<UpdateStatusInWorkRequestCommand, bool>
	{
		private readonly IProcessRequestService _processRequestService;
		private readonly IProcessRequestRepository _processRequestRepository;

		public UpdateStatusInWorkRequestCommandHandler(IProcessRequestService processRequestService, IProcessRequestRepository processRequestRepository)
		{
			_processRequestService = processRequestService;
			_processRequestRepository = processRequestRepository;
		}

		public async Task<bool> Handle(UpdateStatusInWorkRequestCommand request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request), "Request cannot be null");
			if (request.Id == Guid.Empty)
				throw new ArgumentException("Request ID cannot be empty", nameof(request.Id));
			var oldProcessRequest = await _processRequestRepository.GetByIdAsync(request.Id, cancellationToken)
				?? throw new ApplicationException($"Process request with ID {request.Id} not found.");


			var applicationStatus = ApplicationStatus.Parse(request.ApplicationStatus);
			var newProcessRequest = _processRequestService.SetStatusInWork(oldProcessRequest, applicationStatus);
			if (newProcessRequest is null)
				throw new ApplicationException("Failed to update process request status");

			await _processRequestRepository.UpdateAsync(newProcessRequest, cancellationToken);
			return true;
		}
	}
}
