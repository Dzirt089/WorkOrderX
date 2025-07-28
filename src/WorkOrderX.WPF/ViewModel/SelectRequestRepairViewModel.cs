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

			// Поверхостное копирование данных переданной заявки 
			SelectProcessRequest = processRequest.GetCopy();

			await UpdateReferenceDataInForm();
			AccessRights();

		}

		/// <summary>
		/// Метод для обновления данных формы заявки на ремонт с учетом выбранной заявки.
		/// </summary>
		private async Task UpdateReferenceDataInForm()
		{
			// Получаем данные для заполнения ComboBox'ов на форме заявки
			(IEnumerable<ApplicationStatus?> statuses,
				IEnumerable<ApplicationType?> appTypes,
				IEnumerable<EquipmentKind?> kinds,
				IEnumerable<EquipmentModel?> models,
				IEnumerable<EquipmentType?> equpTypes,
				IEnumerable<TypeBreakdown?> breaks,
				IEnumerable<Importance?> importances) = await _referenceDada.GetAllReferenceDataAsync();

			KindsOrig = kinds; // Вид оборудования
			TypeBreakdownsOrig = breaks; // Тип поломки	

			// Полная версия списков  
			EquipmentTypes = new ObservableCollection<EquipmentType>(equpTypes); // Типы оборудования
			Kinds = new ObservableCollection<EquipmentKind>(kinds); // Вид оборудования
			TypeBreakdowns = new ObservableCollection<TypeBreakdown>(breaks); // Тип поломки			
			Statuses = new ObservableCollection<ApplicationStatus>(statuses); // Статусов
			ApplicationTypes = new ObservableCollection<ApplicationType>(appTypes); // Тип заявки (Здесь: "Ремонт оборудования")
			Models = new ObservableCollection<EquipmentModel>(models); // Список моделей (сейчас нет, только: "Другое")
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

		/// <summary>
		/// Метод для определения прав доступа к кнопкам на форме заявки в зависимости от роли сотрудника.
		/// </summary>
		private void AccessRights()
		{
			// Заказчик (Customer)
			IsCustomerVisibilitySave = Visibility.Collapsed;
			IsCustomerUpdate = false;

			// Проверяем, является ли сотрудник заказчиком заявки и не находится ли заявка в статусе "Возвращена" для изменений.
			IsCustomerReturned = (_globalEmployee.Employee.Role == "Customer" || (_globalEmployee.Employee.Role == "Executer" && _globalEmployee.Employee.Id == SelectProcessRequest.CustomerEmployee.Id))
				&& SelectProcessRequest.ApplicationStatus == "Returned";

			// Если заявка возвращена, то показываем кнопку "Сохранить заявку" и скрываем кнопку "Изменить заявку".
			IsCustomerVisibilityUpdate = IsCustomerReturned == true ? Visibility.Visible : Visibility.Collapsed;



			// Испольнитель (Executor)
			// Изначально скрываем кнопки для исполнителя, если заявка в статусе "Возвращена". Т.к. только заказчик может с ней что-то делать
			IsExecutor = _globalEmployee.Employee.Role == "Executer" && SelectProcessRequest.ApplicationStatus != "Returned";

			// Если сотрудник является исполнителем, то проверяем статус заявки и определяем видимость кнопки "Выполнено".
			IsExecutorVisibilityDoneButton = IsExecutor == true && SelectProcessRequest.ApplicationStatus == "InWork"
				? Visibility.Visible
				: Visibility.Collapsed;

			// Если сотрудник является исполнителем, то проверяем статус заявки и определяем видимость кнопки "В работе".
			IsExecutorVisibilityInWorkButton = IsExecutor == true &&
				(SelectProcessRequest.ApplicationStatus == "New" ||
					(SelectProcessRequest.ApplicationStatus == "Redirected" &&
					SelectProcessRequest.ExecutorEmployee.Id == _globalEmployee.Employee.Id) ||
				SelectProcessRequest.ApplicationStatus == "Changed" || SelectProcessRequest.ApplicationStatus == "Postponed")
				? Visibility.Visible
				: Visibility.Collapsed;

			// Если сотрудник является исполнителем, то проверяем статус заявки и определяем видимость кнопок "Отклонить", "Возврат", "Перенаправить"
			IsExecutorVisibilityButton = IsExecutor == true
				? Visibility.Visible
				: Visibility.Collapsed;

			// Если сотрудник является исполнителем и заявка в статусе "Выполнено", то скрываем все кнопки для исполнителя.
			if (IsExecutor && SelectProcessRequest.ApplicationStatus == "Done")
			{
				IsExecutorVisibilityButton = Visibility.Collapsed;
				IsExecutorVisibilityInWorkButton = Visibility.Collapsed;
				IsExecutorVisibilityDoneButton = Visibility.Collapsed;
			}
		}

		/// <summary>
		/// Метод для установки статуса заявки "В работе".
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
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

			bool _ = await _processRequestApi.UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(updateStatusInWork);
			if (!_)
			{
				MessageBox.Show("Не удалось принять в работу заявку. Попробуйте позже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			await UpdateReferenceDataInForm();
			AccessRights();
		}

		/// <summary>
		/// Метод для установки статуса заявки "Возвращена заказчику".
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Returned()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			SelectProcessRequest.ApplicationStatus = "Returned";
			UpdateStatusInWorkOrReturnedOrPostponedRequestModel returnedOrPostponedRequestModel =
				new UpdateStatusInWorkOrReturnedOrPostponedRequestModel
				{
					Id = (Guid)SelectProcessRequest.Id!,
					ApplicationStatus = SelectProcessRequest.ApplicationStatus,
					InternalComment = SelectProcessRequest.InternalComment,
				};

			bool _ = await _processRequestApi.UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(returnedOrPostponedRequestModel);
			if (!_)
			{
				MessageBox.Show("Не удалось вернуть заявку. Попробуйте позже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			await UpdateReferenceDataInForm();
			AccessRights();
		}

		/// <summary>
		/// Метод, который открывает доступ к обновлению заявки для заказчика на форме.
		/// </summary>
		[RelayCommand]
		private void Update()
		{
			IsCustomerVisibilitySave = Visibility.Visible;
			IsCustomerUpdate = true;
			IsCustomerVisibilityUpdate = Visibility.Collapsed;
		}

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

			bool _ = await _processRequestApi.UpdateProcessRequestAsync(updateProcess);
			if (!_)
			{
				MessageBox.Show("Не удалось сохранить заявку. Попробуйте позже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			IsCustomerUpdate = false;

			await UpdateReferenceDataInForm();
			AccessRights();
		}

		/// <summary>
		/// Метод для установки статуса заявки "Выполнена".
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Done()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			SelectProcessRequest.ApplicationStatus = "Done";
			UpdateStatusDoneOrRejectedModel updateStatusDone = new UpdateStatusDoneOrRejectedModel
			{
				Id = (Guid)SelectProcessRequest.Id!,
				ApplicationStatus = SelectProcessRequest.ApplicationStatus,
				InternalComment = SelectProcessRequest.InternalComment,
				CompletedAt = DateTime.Now.ToString("f")
			};

			bool _ = await _processRequestApi.UpdateStatusDoneOrRejectedAsync(updateStatusDone);
			if (!_)
			{
				MessageBox.Show("Не удалось выполнить заявку. Попробуйте позже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			await UpdateReferenceDataInForm();
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
		public EquipmentType? ItemEqType
		{
			get => _itemEqType;
			set
			{
				SetProperty(ref _itemEqType, value);

				Kinds = new ObservableCollection<EquipmentKind>(KindsOrig
					.Where(_ => _.Type.Id == ItemEqType?.Id)
					.ToList());

				TypeBreakdowns = new ObservableCollection<TypeBreakdown>(TypeBreakdownsOrig.
					Where(_ => _.Type.Id == ItemEqType?.Id)
					.ToList());
			}
		}
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


		// Полные данные для фильтра на форме UI клиента.
		[ObservableProperty]
		private IEnumerable<EquipmentKind>? _kindsOrig;

		[ObservableProperty]
		private IEnumerable<TypeBreakdown>? _typeBreakdownsOrig;
		#endregion
	}
}
