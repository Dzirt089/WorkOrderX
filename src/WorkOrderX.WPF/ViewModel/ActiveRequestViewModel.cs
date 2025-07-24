using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;
using WorkOrderX.WPF.Views;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class ActiveRequestViewModel : ViewModelBase
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IProcessRequestService _processRequestService;
		private readonly IReferenceDadaServices _referenceDadaServices;
		private readonly SelectRequestRepairViewModel _requestRepairViewModel;

		public ActiveRequestViewModel(
			GlobalEmployeeForApp globalEmployee,
			IProcessRequestService processRequestService,
			IReferenceDadaServices referenceDadaServices,
			SelectRequestRepairViewModel requestRepairViewModel)
		{
			_globalEmployee = globalEmployee;
			_activeRequests = [];
			_processRequestService = processRequestService;
			_referenceDadaServices = referenceDadaServices;
			_requestRepairViewModel = requestRepairViewModel;
		}

		public async Task InitializationAsync()
		{

			(IEnumerable<ApplicationStatus> statuses,
			IEnumerable<ApplicationType?> appTypes,
			IEnumerable<Importance?> importances)
			result = await _referenceDadaServices.GetRefDataForInitAsync();

			var statusesDict = result.statuses.ToDictionary(_ => _.Name);
			var importancesDict = result.importances.ToDictionary(_ => _.Name);
			var applicationTypesDict = result.appTypes.ToDictionary(_ => _.Name);


			ProcessRequests = await _processRequestService.GetActiveProcessRequestsAsync(_globalEmployee.Employee.Id);
			var activeRequests = ProcessRequests
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

		[RelayCommand]
		private async Task ShowSelectRequestRepair(Guid id)
		{
			if (id == Guid.Empty) return;

			var selectedRequest = ProcessRequests?.FirstOrDefault(_ => _.Id == id);
			if (selectedRequest == null) return;

			await _requestRepairViewModel.InitializationAsync(selectedRequest);

			SelectRequestRepair selectRequest = new(_requestRepairViewModel);
			selectRequest.ShowDialog();
		}

		[ObservableProperty]
		private IEnumerable<ProcessRequest>? _processRequests;

		[ObservableProperty]
		private ObservableCollection<ActiveRequestProcess> _activeRequests;

		public ActiveRequestProcess? SelectedRequest
		{
			get => _selectedRequest;
			set => SetProperty(ref _selectedRequest, value);
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
