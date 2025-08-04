using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;
using WorkOrderX.WPF.Utils;
using WorkOrderX.WPF.Views;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class HistoryRequestViewModel : ViewModelBase
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IProcessRequestService _processRequestService;
		private readonly IReferenceDadaServices _referenceDadaServices;
		private readonly SelectRequestRepairViewModel _requestRepairViewModel;
		private readonly SelectRequestChoreViewModel _requestChoreViewModel;
		private readonly SortableListView _sortableList;

		public HistoryRequestViewModel(GlobalEmployeeForApp globalEmployee, IProcessRequestService processRequestService, IReferenceDadaServices referenceDadaServices, SelectRequestRepairViewModel requestRepairViewModel, SelectRequestChoreViewModel requestChoreViewModel, SortableListView sortableList)
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
		public async Task InitializationAsync()
		{
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
			ProcessRequests = await _processRequestService.GetHistoryProcessRequestsAsync(_globalEmployee.Employee.Id);

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
					CompletionAt = string.IsNullOrEmpty(_.CompletionAt) ? null : DateTime.Parse(_.CompletionAt),
					UpdatedAt = string.IsNullOrEmpty(_.UpdatedAt) ? null : DateTime.Parse(_.UpdatedAt)
				});

			// Список активных заявок
			HistoryRequests = new ObservableCollection<ActiveHistoryRequestProcess>(activeRequests.OrderByDescending(_ => _.UpdatedAt));

			_sortableList.RestoreLastSort(HistoryRequests);

			SelectedRequest = _lastSelectedRequest != null
				? HistoryRequests.FirstOrDefault(_ => _.Id == _lastSelectedRequest)
				: HistoryRequests.FirstOrDefault();
		}
		#endregion

		#region Commands

		/// <summary>
		/// Показать диалоговое окно для выбора заявки на ремонт.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[RelayCommand]
		private async Task ShowSelectRequestRepair(Guid id)
		{
			// Если идентификатор пустой, ничего не делать
			if (id == Guid.Empty) return;

			// Найти заявку по идентификатору и если не найдена, выйти
			var selectedRequest = ProcessRequests?.FirstOrDefault(_ => _.Id == id);

			if (selectedRequest?.ApplicationType == "EquipmentRepair")
			{
				if (selectedRequest == null ||
				selectedRequest.EquipmentKind == null ||
				selectedRequest.EquipmentType == null ||
				selectedRequest.TypeBreakdown == null ||
				selectedRequest.EquipmentModel == null ||
				selectedRequest.ApplicationStatus == null) return;

				// Инициализация модели представления для заявки на ремонт
				await _requestRepairViewModel.InitializationAsync(selectedRequest);

				// Показать диалоговое окно для выбора заявки на ремонт
				SelectRequestRepair selectRequest = new(_requestRepairViewModel);
				selectRequest.ShowDialog();

				// Если диалоговое окно закрыто с результатом OK, обновить активные заявки
				await InitializationAsync();
			}
			else if (selectedRequest?.ApplicationType == "HouseholdChores")
			{
				if (selectedRequest == null ||
				selectedRequest.Location == null ||
				selectedRequest.DescriptionMalfunction == null ||
				selectedRequest.Importance == null) return;

				await _requestChoreViewModel.InitializationAsync(selectedRequest);
				SelectRequestChore selectRequestChore = new SelectRequestChore(_requestChoreViewModel);
				selectRequestChore.ShowDialog();

				// Если диалоговое окно закрыто с результатом OK, обновить активные заявки
				await InitializationAsync();
			}
		}
		#endregion

		#region Коллекции и св-ва 

		/// <summary>
		/// Список активных заявок для текущего пользователя.
		/// </summary>
		[ObservableProperty]
		private IEnumerable<ProcessRequest>? _processRequests;

		/// <summary>
		/// Список активных заявок, преобразованных в модель представления.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ActiveHistoryRequestProcess> _historyRequests;

		/// <summary>
		/// Выбранная заявка для просмотра или редактирования.
		/// </summary>
		public ActiveHistoryRequestProcess? SelectedRequest
		{
			get => _selectedRequest;
			set => SetProperty(ref _selectedRequest, value);
		}
		private ActiveHistoryRequestProcess? _selectedRequest;

		/// <summary>
		/// Список статусов заявок для инициализации.
		/// </summary>
		[ObservableProperty]
		private List<ApplicationStatus> _statuses;

		/// <summary>
		/// Список важности для инициализации.
		/// </summary>
		[ObservableProperty]
		private List<Importance> _importances;

		/// <summary>
		/// Список типов заявок для инициализации.
		/// </summary>
		[ObservableProperty]
		private List<ApplicationType> _applicationTypes;

		#endregion
	}
}
