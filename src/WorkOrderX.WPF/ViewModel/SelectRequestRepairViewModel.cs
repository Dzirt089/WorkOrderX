using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Windows;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class SelectRequestRepairViewModel : ViewModelBase
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IReferenceDadaServices _referenceDada;
		private readonly IProcessRequestApiService _processRequestApi;

		/// <summary>
		/// Конструктор для инициализации сервиса и API для работы с заявками на ремонт.
		/// </summary>
		/// <param name="referenceDada"></param>
		/// <param name="processRequestApi"></param>
		public SelectRequestRepairViewModel(IReferenceDadaServices referenceDada, IProcessRequestApiService processRequestApi, GlobalEmployeeForApp globalEmployee)
		{
			_referenceDada = referenceDada;
			_processRequestApi = processRequestApi;
			_globalEmployee = globalEmployee;
		}

		//TODO: Загрузить список сотрудников с ролью ИСПОЛНИТЕЛЬ, для реализации логики "ПЕРЕНАПРАВИТЬ"

		/// <summary>
		/// Инициализация данных для формы выбранной заявки на ремонт.
		/// </summary>
		/// <param name="processRequest"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task InitializationAsync(ProcessRequest processRequest)
		{
			// Проверка на null
			ArgumentNullException.ThrowIfNull(processRequest);
			SelectProcessRequest = processRequest;

			// Получаем данные для заполнения ComboBox'ов на форме заявки
			(ObservableCollection<ApplicationStatus>? Statuse,
			ObservableCollection<ApplicationType>? AppTypes,
			ObservableCollection<EquipmentKind>? EqupKinds,
			ObservableCollection<EquipmentModel>? EqupModels,
			ObservableCollection<EquipmentType>? EqupTypes,
			ObservableCollection<TypeBreakdown>? Breaks,
			ObservableCollection<Importance>? Importance) = await _referenceDada.GetAllRefenceDataInCollectionsAsync();

			// Полная версия списков  
			Kinds = EqupKinds; // Вид оборудования
			TypeBreakdowns = Breaks; // Тип поломки			
			Statuses = Statuse; // Статусов
			ApplicationTypes = AppTypes; // Тип заявки (Здесь: "Ремонт оборудования")
			Models = EqupModels; // Список моделей (сейчас нет, только: "Другое")
			Importances = Importance; // Важность заявки
			EquipmentTypes = EqupTypes; // Типы оборудования

			UpdateReferenceDataInForm();
			AccessRights();
		}

		/// <summary>
		/// Метод для обновления данных формы заявки на ремонт с учетом выбранной заявки.
		/// </summary>
		private void UpdateReferenceDataInForm()
		{
			ItemKind = Kinds.FirstOrDefault(_ => _.Name == SelectProcessRequest.EquipmentKind);
			ItemModel = Models.FirstOrDefault(_ => _.Name == SelectProcessRequest.EquipmentModel);
			ItemEqType = EquipmentTypes.FirstOrDefault(_ => _.Name == SelectProcessRequest.EquipmentType);
			ItemBreak = TypeBreakdowns.FirstOrDefault(_ => _.Name == SelectProcessRequest.TypeBreakdown);
			ItemImport = Importances.FirstOrDefault(_ => _.Name == SelectProcessRequest.Importance);
			ItemApplicationStatus = Statuses.FirstOrDefault(_ => _.Name == SelectProcessRequest.ApplicationStatus);
			TextNumberRequest = $"Заявка №{SelectProcessRequest.ApplicationNumber}";
		}

		/// <summary>
		/// Метод для определения прав доступа к кнопкам на форме заявки в зависимости от роли сотрудника.
		/// </summary>
		private void AccessRights()
		{
			IsCustomerVisibilitySave = Visibility.Collapsed;

			IsCustomerReturned = _globalEmployee.Employee.Role == "Customer" && SelectProcessRequest.ApplicationStatus == "Returned";
			IsExecutor = _globalEmployee.Employee.Role == "Executer" && SelectProcessRequest.ApplicationStatus != "Returned";

			IsExecutorVisibilityButton = IsExecutor == true ? Visibility.Visible : Visibility.Collapsed;
			IsCustomerVisibilityUpdate = IsCustomerReturned == true ? Visibility.Visible : Visibility.Collapsed;
			IsExecutorVisibilityDoneButton = IsExecutor == true && SelectProcessRequest.ApplicationStatus == "InWork" ? Visibility.Visible : Visibility.Collapsed;

			IsExecutorVisibilityInWorkButton = IsExecutor == true && (SelectProcessRequest.ApplicationStatus == "New" ||
				SelectProcessRequest.ApplicationStatus == "Redirected" || SelectProcessRequest.ApplicationStatus == "Changed")
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		[RelayCommand]
		private async Task InWork()
		{
			if (SelectProcessRequest.Id is null) throw new ArgumentNullException(nameof(SelectProcessRequest.Id), "Id cannot be null");

			SelectProcessRequest.ApplicationStatus = "InWork";
			UpdateStatusInWorkOrReturnedOrPostponedRequestModel updateStatusInWork = new UpdateStatusInWorkOrReturnedOrPostponedRequestModel
			{
				Id = (Guid)SelectProcessRequest.Id!,
				ApplicationStatus = SelectProcessRequest.ApplicationStatus,
				InternalComment = SelectProcessRequest.InternalComment,
			};

			bool result = await _processRequestApi.UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(updateStatusInWork);

			UpdateReferenceDataInForm();
			AccessRights();
		}

		#region Коллекции и св-ва для формы Выбранная заявка 

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является заказчиком заявки и может производить манипуляции с ней.
		/// </summary>
		[ObservableProperty]
		private bool _isCustomerUpdate;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является заказчиком заявки и может сохранить изменения в ней.
		/// </summary>
		[ObservableProperty]
		private bool _isCustomerSave;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является заказчиком заявки и может видеть кнопку "Сохранить заявку".
		/// </summary>
		[ObservableProperty]
		private Visibility _isCustomerVisibilitySave;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является заказчиком заявки и может видеть кнопку "Изменить заявку".
		/// </summary>
		[ObservableProperty]
		private Visibility _isCustomerVisibilityUpdate;

		/// <summary>
		/// Флаг,указывающий, что сотрудник за компьютером является заказчиком заявки и эту заявку ему вернули для изменений.
		/// </summary>
		[ObservableProperty]
		private bool _isCustomerReturned;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть кнопку "Принять в работу".
		/// </summary>
		[ObservableProperty]
		private Visibility _isExecutorVisibilityInWorkButton;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть кнопку "Выполнить заявку".
		/// </summary>
		[ObservableProperty]
		private Visibility _isExecutorVisibilityDoneButton;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть кнопку "Выполнить заявку".
		/// </summary>
		[ObservableProperty]
		private Visibility _isExecutorVisibilityButton;

		/// <summary>
		/// Флаг,указывающий, что сотрудник за компьютером является исполнителем заявки. И может производить манипуляции с ними.
		/// </summary>
		[ObservableProperty]
		private bool _isExecutor;

		/// <summary>
		/// Свойство для хранения номера заявки, отображаемого на форме.
		/// </summary>
		[ObservableProperty]
		private string _textNumberRequest;

		/// <summary>
		/// Свойство для хранения выбранной заявки на ремонт.
		/// </summary>
		[ObservableProperty]
		private ProcessRequest _selectProcessRequest;

		/// <summary>
		/// Свойство для хранения списка статусов заявки.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ApplicationStatus>? _statuses;

		/// <summary>
		/// Свойство для хранения выбранного статуса заявки.
		/// </summary>
		[ObservableProperty]
		private ApplicationStatus? _itemApplicationStatus;

		/// <summary>
		/// Свойство для хранения списка типов заявок.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ApplicationType>? _applicationTypes;

		/// <summary>
		/// Свойство для хранения выбранного типа заявки.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<Importance>? _importances;

		/// <summary>
		/// Свойство для хранения выбранной важности заявки.
		/// </summary>
		[ObservableProperty]
		private Importance? _itemImport;

		/// <summary>
		/// Свойство для хранения списка видов оборудования.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<EquipmentKind>? _kinds;

		/// <summary>
		/// Свойство для хранения выбранного вида оборудования.
		/// </summary>
		[ObservableProperty]
		private EquipmentKind? _itemKind;

		/// <summary>
		/// Свойство для хранения списка моделей оборудования.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<EquipmentModel>? _models;

		/// <summary>
		/// Свойство для хранения выбранной модели оборудования.
		/// </summary>
		[ObservableProperty]
		private EquipmentModel? _itemModel;

		/// <summary>
		/// Свойство для хранения списка типов оборудования.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<EquipmentType>? _equipmentTypes;

		/// <summary>
		/// Свойство для хранения выбранного типа оборудования.
		/// </summary>
		[ObservableProperty]
		private EquipmentType? _itemEqType;

		/// <summary>
		/// Свойство для хранения списка типов поломок.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<TypeBreakdown>? _typeBreakdowns;

		/// <summary>
		/// Свойство для хранения выбранного типа поломки.
		/// </summary>
		[ObservableProperty]
		private TypeBreakdown? _itemBreak;
		#endregion
	}
}
