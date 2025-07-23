using AutoMapper;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.Services.Implementation
{
	public class ProcessRequestService : ViewModelBase, IProcessRequestService
	{
		private readonly IProcessRequestApiService _processRequestApi;
		private readonly IMapper _mapper;

		public ProcessRequestService(IProcessRequestApiService processRequestApi, IMapper mapper)
		{
			_processRequestApi = processRequestApi;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ProcessRequest>> GetActiveProcessRequestsAsync(Guid employeeId, CancellationToken token = default)
		{
			var processRequests = await _processRequestApi.GetActivProcessRequestsAsync(employeeId, token);
			return _mapper.Map<IEnumerable<ProcessRequest>>(processRequests);
		}
	}
}
