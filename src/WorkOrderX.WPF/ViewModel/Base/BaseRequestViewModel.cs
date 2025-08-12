using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Windows;

using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;
using WorkOrderX.WPF.Utils;
using WorkOrderX.WPF.Views;

namespace WorkOrderX.WPF.ViewModel
{
	public abstract partial class BaseRequestViewModel : ViewModelBase
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IProcessRequestService _processRequestService;
		private readonly IReferenceDadaServices _referenceDadaServices;
		private readonly SelectRequestRepairViewModel _requestRepairViewModel;
		private readonly SelectRequestChoreViewModel _requestChoreViewModel;
		private readonly SortableListView _sortableList;

		protected BaseRequestViewModel(GlobalEmployeeForApp globalEmployee, IProcessRequestService processRequestService, IReferenceDadaServices referenceDadaServices, SelectRequestRepairViewModel requestRepairViewModel, SelectRequestChoreViewModel requestChoreViewModel, SortableListView sortableList)
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
		public abstract Task InitializationAsync();
		#endregion

		#region Commands

		[RelayCommand]
		private async Task FilterForActive()
		{
			await InitializationAsync();
		}


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

			if (selectedRequest?.ApplicationType == "InstrumentRepair")
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

		[ObservableProperty]
		private bool _isSelectRepair = true;

		[ObservableProperty]
		private bool _isSelectChore = false;

		[ObservableProperty]
		private Visibility _visibilitySelectRequest;

		/// <summary>
		/// Список заявок для текущего пользователя.
		/// </summary>
		[ObservableProperty]
		private IEnumerable<ProcessRequest>? _processRequests;

		/// <summary>
		/// Список истории заявок, преобразованных в модель представления.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ActiveHistoryRequestProcess> _historyRequests;

		/// <summary>
		/// Список активных заявок, преобразованных в модель представления.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ActiveHistoryRequestProcess> _activeRequests;

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
