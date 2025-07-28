using AutoMapper;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.Services.Implementation
{
	/// <summary>
	/// Сервис для работы с активными заявками на ремонт.
	/// </summary>
	public class ProcessRequestService : ViewModelBase, IProcessRequestService
	{
		private readonly IProcessRequestApiService _processRequestApi;
		private readonly IMapper _mapper;

		public ProcessRequestService(IProcessRequestApiService processRequestApi, IMapper mapper)
		{
			_processRequestApi = processRequestApi;
			_mapper = mapper;
		}

		/// <summary>
		/// Получение активных заявок на ремонт для указанного сотрудника.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ProcessRequest>> GetActiveProcessRequestsAsync(Guid employeeId, CancellationToken token = default)
		{
			var processRequests = await _processRequestApi.GetActivProcessRequestsAsync(employeeId, token);
			return _mapper.Map<IEnumerable<ProcessRequest>>(processRequests);
		}
	}
}
