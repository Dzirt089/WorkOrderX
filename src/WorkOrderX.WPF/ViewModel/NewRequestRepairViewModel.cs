using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class NewRequestRepairViewModel : ViewModelBase
	{
		private readonly IReferenceDadaServices _referenceDada;
		private readonly IProcessRequestApiService _processRequestApi;
		private readonly GlobalEmployeeForApp _globalEmployee;

		public NewRequestRepairViewModel(
			IReferenceDadaServices referenceDada,
			IProcessRequestApiService processRequestApi,
			GlobalEmployeeForApp globalEmployee)
		{
			_referenceDada = referenceDada;
			_processRequestApi = processRequestApi;
			_globalEmployee = globalEmployee;

			ProcessRequestNew = new ProcessRequest();
			ProcessRequestNew.PropertyChanged += (_, _) =>
			{
				SaveAndSendRequestOrderCommand.NotifyCanExecuteChanged();
			};
		}

		/// <summary>
		/// Инициализация данных для формы Новой заявки на ремонт.
		/// </summary>
		/// <returns></returns>
		public async Task InitializationAsync()
		{
			(ObservableCollection<ApplicationStatus>? Statuses,
			ObservableCollection<ApplicationType>? AppTypes,
			ObservableCollection<EquipmentKind>? EqupKinds,
			ObservableCollection<EquipmentModel>? EqupModels,
			ObservableCollection<EquipmentType>? EqupTypes,
			ObservableCollection<TypeBreakdown>? Breaks,
			ObservableCollection<Importance>? Importances) result = await _referenceDada.GetAllRefenceDataAsync();

			// Полная версия списков  
			KindsOrig = result.EqupKinds; // Вид оборудования
			TypeBreakdownsOrig = result.Breaks; // Тип поломки

			// Полный список 
			Statuses = result.Statuses; // Статусов
			ApplicationTypes = result.AppTypes; // Тип заявки (Здесь: "Ремонт оборудования")
			Models = result.EqupModels; // Список моделей (сейчас нет, только: "Другое")
			Importances = result.Importances; // Важность заявки

			EquipmentTypes = result.EqupTypes; // Типы оборудования
			ItemEqType = EquipmentTypes.FirstOrDefault(); //Берём первый



			CreateTemplateNewRequest();
		}

		private void CreateTemplateNewRequest()
		{
			ProcessRequestNew.CustomerEmployee = _globalEmployee.Employee;
			ProcessRequestNew.ApplicationNumber = 0;
			ProcessRequestNew.ApplicationType = ApplicationTypes?.FirstOrDefault(_ => _.Id == 2)?.Name ?? "EquipmentRepair";
			ProcessRequestNew.ApplicationStatus = Statuses?.FirstOrDefault(_ => _.Id == 1)?.Name ?? "New";
		}

		/// <summary>
		/// Сохранение и отправка заявки на ремонт.
		/// </summary>
		/// <returns></returns>
		[RelayCommand(CanExecute = nameof(CanSaveAndSendRequestOrder))]
		private async Task SaveAndSendRequestOrder()
		{
			if (ProcessRequestNew != null)
			{
				ProcessRequestNew.CreatedAt = DateTime.Now.ToString("f");
				if (ItemImportance.Name == "Normal")
					ProcessRequestNew.PlannedAt = DateTime.Now.AddDays(2).ToString("f");
				else if (ItemImportance.Name == "High")
					ProcessRequestNew.PlannedAt = DateTime.Now.ToString("f");
				else
					throw new InvalidOperationException("Неизвестный уровень важности заявки.");

				CreateProcessRequestModel createProcess = new CreateProcessRequestModel()
				{
					ApplicationNumber = ProcessRequestNew.ApplicationNumber,
					ApplicationType = ProcessRequestNew.ApplicationType,
					ApplicationStatus = ProcessRequestNew.ApplicationStatus,
					CreatedAt = ProcessRequestNew.CreatedAt,
					PlannedAt = ProcessRequestNew.PlannedAt,
					EquipmentType = ProcessRequestNew.EquipmentType,
					EquipmentKind = ProcessRequestNew.EquipmentKind,
					EquipmentModel = ProcessRequestNew.EquipmentModel,
					SerialNumber = ProcessRequestNew.SerialNumber,
					TypeBreakdown = ProcessRequestNew.TypeBreakdown,
					DescriptionMalfunction = ProcessRequestNew.DescriptionMalfunction,
					Importance = ProcessRequestNew.Importance,
					InternalComment = ProcessRequestNew.InternalComment,
					CustomerEmployeeId = ProcessRequestNew.CustomerEmployee.Id,
				};

				bool check = await _processRequestApi.CreateProcessRequestAsync(createProcess);
				if (check)
				{
					ClearFieldRequest();
					CreateTemplateNewRequest();
					_ = MessageBox.Show(
						"Заявка успешно создана и отправлена на обработку.",
						"Успех",
						MessageBoxButton.OK,
						MessageBoxImage.Information);
				}
			}
		}

		[RelayCommand]
		private void CancelRequestOrder()
		{
			ClearFieldRequest();
			CreateTemplateNewRequest();
		}

		private void ClearFieldRequest()
		{
			ProcessRequestNew = new();
			ItemEqType = EquipmentTypes.FirstOrDefault(); //Берём первый
			ItemKind = null;
			ItemModel = null;
			ItemBreak = null;
			ItemImportance = null;
		}

		/// <summary>
		/// Проверка возможности сохранения и отправки заявки на ремонт.
		/// </summary>
		/// <returns></returns>
		private bool CanSaveAndSendRequestOrder()
		{
			if (ProcessRequestNew == null) return false;

			// Проверяем валидацию всех Required-полей
			return HasValidationError();
		}

		/// <summary>
		/// Проверка наличия ошибок валидации для формы Новой заявки.
		/// </summary>
		/// <returns></returns>
		private bool HasValidationError()
		{
			var validationContext = new ValidationContext(ProcessRequestNew);
			var validationResult = new List<ValidationResult>();

			// Проверяем валидацию всех Required-полей
			bool isValid = Validator.TryValidateObject(
				ProcessRequestNew,
				validationContext,
				validationResult);

			return isValid;
		}

		#region Коллекции и св-ва для формы Новой заявки

		[ObservableProperty]
		private ProcessRequest _processRequestNew;

		[ObservableProperty]
		private ObservableCollection<ApplicationStatus>? _statuses;

		[ObservableProperty]
		private ObservableCollection<ApplicationType>? _applicationTypes;

		[ObservableProperty]
		private ObservableCollection<Importance>? _importances;

		public Importance? ItemImportance
		{
			get => _itemImport;
			set
			{
				SetProperty(ref _itemImport, value);
				ProcessRequestNew.Importance = ItemImportance?.Name;
			}
		}
		private Importance? _itemImport;


		[ObservableProperty]
		private ObservableCollection<EquipmentKind>? _kinds;

		public EquipmentKind? ItemKind
		{
			get => _itemKind;
			set
			{
				SetProperty(ref _itemKind, value);
				ProcessRequestNew.EquipmentKind = ItemKind?.Name;
			}
		}
		private EquipmentKind? _itemKind;

		[ObservableProperty]
		private ObservableCollection<EquipmentModel>? _models;

		public EquipmentModel? ItemModel
		{
			get => _itemModel;
			set
			{
				SetProperty(ref _itemModel, value);
				ProcessRequestNew.EquipmentModel = ItemModel?.Name;
			}
		}
		private EquipmentModel? _itemModel;

		[ObservableProperty]
		private ObservableCollection<EquipmentType>? _equipmentTypes;

		public EquipmentType? ItemEqType
		{
			get => _itemEqType;
			set
			{
				SetProperty(ref _itemEqType, value);
				Kinds = new ObservableCollection<EquipmentKind>(KindsOrig
					.Where(_ => _.Type.Id == ItemEqType.Id)
					.ToList());

				TypeBreakdowns = new ObservableCollection<TypeBreakdown>(TypeBreakdownsOrig.
					Where(_ => _.Type.Id == ItemEqType.Id)
					.ToList());

				ProcessRequestNew.EquipmentType = ItemEqType?.Name;
			}
		}
		private EquipmentType? _itemEqType;

		[ObservableProperty]
		private ObservableCollection<TypeBreakdown>? _typeBreakdowns;

		public TypeBreakdown? ItemBreak
		{
			get => _itemBreak;
			set
			{
				SetProperty(ref _itemBreak, value);
				ProcessRequestNew.TypeBreakdown = ItemBreak?.Name;
			}
		}
		private TypeBreakdown? _itemBreak;


		// Полные данные для фильтра на форме UI клиента.
		[ObservableProperty]
		private IEnumerable<EquipmentKind>? _kindsOrig;

		[ObservableProperty]
		private IEnumerable<TypeBreakdown>? _typeBreakdownsOrig;

		#endregion
	}
}
