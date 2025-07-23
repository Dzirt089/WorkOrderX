using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class ActiveRequestViewModel : ViewModelBase
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IProcessRequestService _processRequestService;
		private readonly IReferenceDadaServices _referenceDadaServices;

		public ActiveRequestViewModel(GlobalEmployeeForApp globalEmployee, IProcessRequestService processRequestService, IReferenceDadaServices referenceDadaServices)
		{
			_globalEmployee = globalEmployee;
			_activeRequests = [];
			_processRequestService = processRequestService;
			_referenceDadaServices = referenceDadaServices;
		}

		public async Task InitializationAsync()
		{

			var statuses = await _referenceDadaServices.GetApplicationStatusesAsync();
			var importances = await _referenceDadaServices.GetImportancesAsync();
			var applicationTypes = await _referenceDadaServices.GetApplicationTypesAsync();

			var statusesDict = statuses.ToDictionary(_ => _.Name);
			var importancesDict = importances.ToDictionary(_ => _.Name);
			var applicationTypesDict = applicationTypes.ToDictionary(_ => _.Name);


			var processRequests = await _processRequestService.GetActiveProcessRequestsAsync(_globalEmployee.Employee.Id);
			var activeRequests = processRequests
				.Select(_ => new ActiveRequestProcess
				{
					Id = _.Id,
					ApplicationType = applicationTypesDict[_.ApplicationType].Description,
					ApplicationNumber = _.ApplicationNumber,
					ApplicationStatus = statusesDict[_.ApplicationStatus].Description,
					Importance = importancesDict[_.Importance].Description,
					CustomerEmployee = _.CustomerEmployee,
					ExecutorEmployee = _.ExecutorEmployee,
					CreatedAt = _.CreatedAt,
					PlannedAt = _.PlannedAt,
				});

			ActiveRequests = new ObservableCollection<ActiveRequestProcess>(activeRequests.OrderByDescending(_ => _.CreatedAt));
		}


		[ObservableProperty]
		private ObservableCollection<ActiveRequestProcess> _activeRequests;

		public ActiveRequestProcess? SelectedRequest
		{
			get => _selectedRequest;
			set
			{
				if (SetProperty(ref _selectedRequest, value))
				{
					if (_selectedRequest != null)
					{
						//Statuses = _referenceDadaServices.GetStatuses();
						//Importances = _referenceDadaServices.GetImportances();
					}
				}
			}
		}
		private ActiveRequestProcess? _selectedRequest;

		[ObservableProperty]
		private List<ApplicationStatus> _statuses;

		[ObservableProperty]
		private List<Importance> _importances;

		[ObservableProperty]
		private List<ApplicationType> _applicationTypes;
	}
}
