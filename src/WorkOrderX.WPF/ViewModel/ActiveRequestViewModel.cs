using System.Collections.ObjectModel;
using System.Windows;

using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;
using WorkOrderX.WPF.Utils;

namespace WorkOrderX.WPF.ViewModel
{
	/// <summary>
	/// ViewModel для управления активными заявками в приложении.
	/// </summary>
	public partial class ActiveRequestViewModel : BaseRequestViewModel
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IProcessRequestService _processRequestService;
		private readonly IReferenceDadaServices _referenceDadaServices;
		private readonly SelectRequestRepairViewModel _requestRepairViewModel;
		private readonly SelectRequestChoreViewModel _requestChoreViewModel;
		private readonly SortableListView _sortableList;

		public ActiveRequestViewModel(
			GlobalEmployeeForApp globalEmployee,
			IProcessRequestService processRequestService,
			IReferenceDadaServices referenceDadaServices,
			SelectRequestRepairViewModel requestRepairViewModel,
			SelectRequestChoreViewModel requestChoreViewModel,
			SortableListView sortableList)
			:
			base(
				globalEmployee,
				processRequestService,
				referenceDadaServices,
				requestRepairViewModel,
				requestChoreViewModel,
				sortableList)
		{
			_globalEmployee = globalEmployee;
			_processRequestService = processRequestService;
			_referenceDadaServices = referenceDadaServices;
			_requestRepairViewModel = requestRepairViewModel;
			_requestChoreViewModel = requestChoreViewModel;
			_sortableList = sortableList;
		}

		#region Methods

		/// <summary>
		/// Инициализация данных для активных заявок.
		/// </summary>
		/// <returns></returns>
		public override async Task InitializationAsync()
		{
			bool flagAdmin = _globalEmployee.Employee.Role == "Admin" ? true : false;

			VisibilitySelectRequest = flagAdmin
				? Visibility.Visible
				: Visibility.Collapsed;


			Guid? _lastSelectedRequest = SelectedRequest != null ? SelectedRequest.Id : null;
			SelectedRequest = null;

			// Получение справочных данных для инициализации
			(IEnumerable<ApplicationStatus> statusesList,
				IEnumerable<Importance?> importancesList,
				IEnumerable<ApplicationType?> appTypesList)
			result = await _referenceDadaServices.GetRefDataForInitAsync();

			// Инициализация списков справочных данных в словарях
			var statusesDict = result.statusesList.ToDictionary(_ => _.Name);
			var importancesDict = result.importancesList.ToDictionary(_ => _.Name);
			var applicationTypesDict = result.appTypesList.ToDictionary(_ => _.Name);

			// Получение активных заявок для текущего пользователя
			ProcessRequests = await _processRequestService.GetActiveProcessRequestsAsync(_globalEmployee.Employee.Id);

			var activeRequests = ProcessRequests
				.Select(_ => new ActiveHistoryRequestProcess
				{
					Id = _.Id,
					ApplicationType = applicationTypesDict[_.ApplicationType].Description,
					ApplicationNumber = _.ApplicationNumber,
					ApplicationStatus = statusesDict[_.ApplicationStatus].Description,
					Importance = importancesDict[_.Importance].Description,
					CustomerEmployee = _.CustomerEmployee,
					ExecutorEmployee = _.ExecutorEmployee,

					CreatedAt = string.IsNullOrEmpty(_.CreatedAt) ? DateTime.Today : DateTime.Parse(_.CreatedAt),
					PlannedAt = string.IsNullOrEmpty(_.PlannedAt) ? DateTime.Today : DateTime.Parse(_.PlannedAt),
					UpdatedAt = string.IsNullOrEmpty(_.UpdatedAt) ? null : DateTime.Parse(_.UpdatedAt)
				});

			string typeApp = IsSelectRepair
				? "Ремонт оборудования"
				: "Хоз. работы";

			activeRequests = flagAdmin
				? activeRequests.Where(_ => _.ApplicationType == typeApp).ToList()
				: activeRequests;

			// Список активных заявок
			ActiveRequests = new ObservableCollection<ActiveHistoryRequestProcess>(activeRequests.OrderByDescending(_ => _.UpdatedAt));
			_sortableList.RestoreLastSort(ActiveRequests);

			SelectedRequest = _lastSelectedRequest != null
				? ActiveRequests.FirstOrDefault(_ => _.Id == _lastSelectedRequest)
				: ActiveRequests.FirstOrDefault();
		}

		#endregion
	}
}
