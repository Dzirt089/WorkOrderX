using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	/// <summary>
	/// ViewModel для управления заявкой на ремонт, позволяющий пользователю взаимодействовать с данными заявки и выполнять действия над ней.
	/// </summary>
	public partial class SelectRequestRepairViewModel : BaseSelectRequestViewModel
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IReferenceDadaServices _referenceDada;
		private readonly IProcessRequestApiService _processRequestApi;
		private readonly IEmployeeService _employeeService;


		/// <summary>
		/// Конструктор для инициализации сервиса и API для работы с заявками на ремонт.
		/// </summary>
		/// <param name="referenceDada"></param>
		/// <param name="processRequestApi"></param>
		public SelectRequestRepairViewModel(
			IReferenceDadaServices referenceDada,
			IProcessRequestApiService processRequestApi,
			GlobalEmployeeForApp globalEmployee,
			IEmployeeService employeeService)
			:
			base(
				globalEmployee,
				referenceDada,
				processRequestApi,
				employeeService)
		{
			_referenceDada = referenceDada;
			_processRequestApi = processRequestApi;
			_globalEmployee = globalEmployee;
			_employeeService = employeeService;
		}

		#region Methods

		/// <summary>
		/// Метод для обновления данных формы заявки на ремонт с учетом выбранной заявки.
		/// </summary>
		public override async Task UpdateReferenceDataInForm()
		{
			// Получаем данные для заполнения ComboBox'ов на форме заявки
			(IEnumerable<ApplicationStatus?> statuses,
				IEnumerable<ApplicationType?> appTypes,
				IEnumerable<InstrumentKind?> kinds,
				IEnumerable<InstrumentModel?> models,
				IEnumerable<InstrumentType?> equpTypes,
				IEnumerable<TypeBreakdown?> breaks,
				IEnumerable<Importance?> importances) = await _referenceDada.GetAllReferenceDataAsync();

			KindsOrig = kinds; // Вид оборудования
			TypeBreakdownsOrig = breaks; // Тип поломки	

			// Полная версия списков  
			EquipmentTypes = new ObservableCollection<InstrumentType>(equpTypes); // Типы оборудования
			Kinds = new ObservableCollection<InstrumentKind>(kinds); // Вид оборудования
			TypeBreakdowns = new ObservableCollection<TypeBreakdown>(breaks); // Тип поломки			
			Statuses = new ObservableCollection<ApplicationStatus>(statuses); // Статусов
			ApplicationTypes = new ObservableCollection<ApplicationType>(appTypes); // Тип заявки (Здесь: "Ремонт оборудования")
			Models = new ObservableCollection<InstrumentModel>(models); // Список моделей (сейчас нет, только: "Другое")
			Importances = new ObservableCollection<Importance>(importances); // Важность заявки

			// Инициализация списков справочных данных в словарях
			var equpTypeDict = equpTypes.ToDictionary(_ => _.Name);
			var kindDict = kinds.ToDictionary(_ => _.Name);
			var modelDict = models.ToDictionary(_ => _.Name);

			var typeBreakDict = breaks.ToDictionary(_ => _.Name);
			var importancesDict = importances.ToDictionary(_ => _.Name);
			var statusesDict = statuses.ToDictionary(_ => _.Name);

			ItemEqType = equpTypeDict[SelectProcessRequest.EquipmentType];
			ItemKind = kindDict[SelectProcessRequest.EquipmentKind];
			ItemModel = modelDict[SelectProcessRequest.EquipmentModel];

			ItemBreak = typeBreakDict[SelectProcessRequest.TypeBreakdown];
			ItemImport = importancesDict[SelectProcessRequest.Importance];
			ItemApplicationStatus = statusesDict[SelectProcessRequest.ApplicationStatus];



			TextNumberRequest = $"Заявка №{SelectProcessRequest.ApplicationNumber}";
		}

		#endregion

		#region Commands

		/// <summary>
		/// Метод для сохранения изменений в заявке на ремонт, если сотрудник является заказчиком заявки.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task SaveRequest()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			SelectProcessRequest.Importance = ItemImport?.Name ?? throw new ArgumentException($"Занчение {nameof(ItemImport.Name)} равно null");
			SelectProcessRequest.EquipmentKind = ItemKind?.Name ?? throw new ArgumentException($"Занчение {nameof(ItemKind.Name)} равно null");
			SelectProcessRequest.EquipmentModel = ItemModel?.Name ?? throw new ArgumentException($"Занчение {nameof(ItemModel.Name)} равно null");
			SelectProcessRequest.EquipmentType = ItemEqType?.Name ?? throw new ArgumentException($"Занчение {nameof(ItemEqType.Name)} равно null");
			SelectProcessRequest.TypeBreakdown = ItemBreak?.Name ?? throw new ArgumentException($"Значение {nameof(ItemBreak.Name)} равно null");

			SelectProcessRequest.ApplicationStatus = "Changed";

			UpdateProcessRequestModel updateProcess = new UpdateProcessRequestModel
			{
				Id = (Guid)SelectProcessRequest.Id!,
				ApplicationType = SelectProcessRequest.ApplicationType,
				ApplicationStatus = SelectProcessRequest.ApplicationStatus,
				ApplicationNumber = SelectProcessRequest.ApplicationNumber,
				CreatedAt = SelectProcessRequest.CreatedAt,
				PlannedAt = SelectProcessRequest.PlannedAt,
				EquipmentKind = SelectProcessRequest.EquipmentKind,
				EquipmentModel = SelectProcessRequest.EquipmentModel,
				EquipmentType = SelectProcessRequest.EquipmentType,
				SerialNumber = SelectProcessRequest.SerialNumber,
				Importance = SelectProcessRequest.Importance,
				InternalComment = SelectProcessRequest.InternalComment,
				TypeBreakdown = SelectProcessRequest.TypeBreakdown,
				DescriptionMalfunction = SelectProcessRequest.DescriptionMalfunction,
				CustomerEmployeeId = SelectProcessRequest.CustomerEmployee.Id,
			};

			await _processRequestApi.UpdateProcessRequestAsync(updateProcess);

			IsCustomerUpdate = false;

			await FinalMethodAsync();

			CloseAction?.Invoke();
		}

		#endregion
	}
}
