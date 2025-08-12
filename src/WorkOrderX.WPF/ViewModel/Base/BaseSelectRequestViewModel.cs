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
using WorkOrderX.WPF.Views;

namespace WorkOrderX.WPF.ViewModel
{
	public abstract partial class BaseSelectRequestViewModel : ViewModelBase
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IReferenceDadaServices _referenceDada;
		private readonly IProcessRequestApiService _processRequestApi;
		private readonly IEmployeeService _employeeService;

		protected BaseSelectRequestViewModel(GlobalEmployeeForApp globalEmployee, IReferenceDadaServices referenceDada, IProcessRequestApiService processRequestApi, IEmployeeService employeeService)
		{
			_globalEmployee = globalEmployee;
			_referenceDada = referenceDada;
			_processRequestApi = processRequestApi;
			_employeeService = employeeService;
		}

		#region Methods

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
			OldInternalComment = SelectProcessRequest.InternalComment;

			await UpdateReferenceDataInForm();
			AccessRights();

		}

		/// <summary>
		/// Метод для обновления данных формы заявки на ремонт с учетом выбранной заявки.
		/// </summary>
		public abstract Task UpdateReferenceDataInForm();

		/// <summary>
		/// Метод для определения прав доступа к кнопкам на форме заявки в зависимости от роли сотрудника.
		/// </summary>
		public void AccessRights()
		{
			// Испольнитель (Executor)
			// Изначально скрываем кнопки для исполнителя, если заявка в статусе "Возвращена", "Выполнена", "Отменена" или "Отклонена".
			// Т.к. только заказчик может с ней что-то делать
			IsExecutor =
				_globalEmployee.Employee.Role == "Executer" &&
				SelectProcessRequest.ExecutorEmployee.Id == _globalEmployee.Employee.Id &&
				SelectProcessRequest.ApplicationStatus != "Returned" &&
				SelectProcessRequest.ApplicationStatus != "Done" &&
				SelectProcessRequest.ApplicationStatus != "Rejected";

			// Проверяем дату завершения заявки, если она заполнена, то скрываем все кнопки для исполнителя и заказчика.
			if (!string.IsNullOrEmpty(SelectProcessRequest.CompletionAt))
			{
				IsExecutorVisibilityButton = Visibility.Collapsed;
				IsExecutorVisibilityInWorkButton = Visibility.Collapsed;
				IsExecutorVisibilityDoneButton = Visibility.Collapsed;

				IsCustomerVisibilitySave = Visibility.Collapsed;
				IsCustomerUpdate = false;

				IsCustomerVisibilityUpdate = Visibility.Collapsed;
				IsExecutorVisibilityReturnedButton = Visibility.Collapsed;
				IsExecutorVisibilityRejectedButton = Visibility.Collapsed;
				IsExecutorVisibilityPostponedButton = Visibility.Collapsed;
				IsExecutorVisibilityRedirectedButton = Visibility.Collapsed;

				// Выходим из метода, т.к. заявка уже завершена.
				return;
			}

			// Если сотрудник является исполнителем и заявка в статусе "Выполнено", то скрываем все кнопки для исполнителя.
			if (IsExecutor && (SelectProcessRequest.ApplicationStatus == "Done" ||
				SelectProcessRequest.ApplicationStatus == "Rejected" ||
				(SelectProcessRequest.ApplicationStatus == "Redirected" && SelectProcessRequest.ExecutorEmployee.Id != _globalEmployee.Employee.Id)))
			{
				IsExecutorVisibilityButton = Visibility.Collapsed;
				IsExecutorVisibilityInWorkButton = Visibility.Collapsed;
				IsExecutorVisibilityDoneButton = Visibility.Collapsed;

				IsExecutorVisibilityReturnedButton = Visibility.Collapsed;
				IsExecutorVisibilityRejectedButton = Visibility.Collapsed;
				IsExecutorVisibilityPostponedButton = Visibility.Collapsed;
				IsExecutorVisibilityRedirectedButton = Visibility.Collapsed;

				return;
			}


			// Заказчик (Customer)
			IsCustomerVisibilitySave = Visibility.Collapsed;
			IsCustomerUpdate = false;

			// Проверяем, является ли сотрудник заказчиком заявки и не находится ли заявка в статусе "Возвращена" для изменений.
			IsCustomerReturned = (
				(_globalEmployee.Employee.Role == "Customer" ||
				_globalEmployee.Employee.Role == "Admin" ||
				_globalEmployee.Employee.Role == "Manager") ||
				(_globalEmployee.Employee.Role == "Executer" && _globalEmployee.Employee.Id == SelectProcessRequest.CustomerEmployee.Id)) &&
				SelectProcessRequest.ApplicationStatus == "Returned";

			// Если заявка возвращена, то показываем кнопку "Сохранить заявку" и скрываем кнопку "Изменить заявку".
			IsCustomerVisibilityUpdate = IsCustomerReturned == true ? Visibility.Visible : Visibility.Collapsed;



			IsExecutorVisibilityReturnedButton = IsExecutor == true &&
				SelectProcessRequest.ApplicationStatus != "Returned" ? Visibility.Visible : Visibility.Collapsed;

			IsExecutorVisibilityRejectedButton = IsExecutor == true &&
				SelectProcessRequest.ApplicationStatus != "Rejected" ? Visibility.Visible : Visibility.Collapsed;

			IsExecutorVisibilityPostponedButton = IsExecutor == true &&
				SelectProcessRequest.ApplicationStatus != "Postponed" ? Visibility.Visible : Visibility.Collapsed;

			IsExecutorVisibilityRedirectedButton = IsExecutor == true &&
				SelectProcessRequest.ApplicationStatus != "Redirected" ? Visibility.Visible : Visibility.Collapsed;


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
		}

		/// <summary>
		/// Метод, для проверки и сохранения комментария перед закрытием окна заявки.
		/// </summary>
		/// <returns></returns>
		public async Task<bool> CheckAndSaveBeforeClosing()
		{
			// Проверка, был ли изменен комментарий
			if (!string.Equals(SelectProcessRequest.InternalComment, OldInternalComment, StringComparison.OrdinalIgnoreCase))
			{

				// Если комментарий был изменен, то спрашиваем пользователя, хочет ли он сохранить изменения
				var result = MessageBox.Show(
					"Комментарий был изменен. Сохранить изменения?",
					"Сохранение комментария",
					MessageBoxButton.YesNoCancel,
					MessageBoxImage.Question);

				// Если пользователь выбрал "Да", то сохраняем комментарий
				if (result == MessageBoxResult.Yes)
				{
					// Сохраняем комментарий, используя команду SaveCommentCommand
					await SaveCommentCommand.ExecuteAsync(null);
					return true; // Продолжить закрытие
				}
				else if (result == MessageBoxResult.Cancel)
				{
					return false; // Отменить закрытие
				}
			}
			return true; // Продолжить закрытие
		}

		/// <summary>
		/// Завершающий метод, в котором ряд вызывающих методов
		/// </summary>
		/// <returns></returns>
		public async Task FinalMethodAsync()
		{
			OldInternalComment = SelectProcessRequest.InternalComment;
			await UpdateReferenceDataInForm();
			AccessRights();
		}
		#endregion

		#region Commands

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

			await _processRequestApi.UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(updateStatusInWork);

			await FinalMethodAsync();
		}

		/// <summary>
		/// Метод для установки статуса заявки "Возвращена заказчику".
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Returned()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			DetectionActivWindow(out SelectRequestChore? activeWinChore, out SelectRequestRepair? activeWinRepair);

			var dialog = new CommentSelectionDialog()
			{
				Owner = activeWinChore is null ? activeWinRepair : activeWinChore
			};

			if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Comment))
			{
				var comment = string.IsNullOrEmpty(SelectProcessRequest.InternalComment)
					? $"Возвращена: {dialog.Comment}"
					: $"{SelectProcessRequest.InternalComment}.\nВозвращена: {dialog.Comment}";

				SelectProcessRequest.ApplicationStatus = "Returned";
				UpdateStatusInWorkOrReturnedOrPostponedRequestModel returnedOrPostponedRequestModel =
					new UpdateStatusInWorkOrReturnedOrPostponedRequestModel
					{
						Id = (Guid)SelectProcessRequest.Id!,
						ApplicationStatus = SelectProcessRequest.ApplicationStatus,
						InternalComment = comment,
					};

				await _processRequestApi.UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(returnedOrPostponedRequestModel);
				await FinalMethodAsync();
				CloseAction?.Invoke();
			}
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
		/// Метод для установки статуса заявки "Выполнена".
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Done()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			DetectionActivWindow(out SelectRequestChore? activeWinChore, out SelectRequestRepair? activeWinRepair);

			var dialog = new CommentSelectionDialog()
			{
				Owner = activeWinChore is null ? activeWinRepair : activeWinChore
			};

			if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Comment))
			{
				var comment = string.IsNullOrEmpty(SelectProcessRequest.InternalComment)
					? $"Выполнена: {dialog.Comment}"
					: $"{SelectProcessRequest.InternalComment}.\nВыполнена: {dialog.Comment}";

				SelectProcessRequest.ApplicationStatus = "Done";
				UpdateStatusDoneOrRejectedModel updateStatusDone = new UpdateStatusDoneOrRejectedModel
				{
					Id = (Guid)SelectProcessRequest.Id!,
					ApplicationStatus = SelectProcessRequest.ApplicationStatus,
					InternalComment = comment,
					CompletedAt = DateTime.Now.ToString("f")
				};

				await _processRequestApi.UpdateStatusDoneOrRejectedAsync(updateStatusDone);
				await FinalMethodAsync();
				CloseAction?.Invoke();
			}
		}

		/// <summary>
		/// Метод для установки статуса заявки "Отклонена".
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Rejected()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			DetectionActivWindow(out SelectRequestChore? activeWinChore, out SelectRequestRepair? activeWinRepair);

			var dialog = new CommentSelectionDialog()
			{
				Owner = activeWinChore is null ? activeWinRepair : activeWinChore
			};

			if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.Comment))
			{
				var comment = string.IsNullOrEmpty(SelectProcessRequest.InternalComment)
					? $"Отклонена: {dialog.Comment}"
					: $"{SelectProcessRequest.InternalComment}.\nОтклонена: {dialog.Comment}";

				SelectProcessRequest.ApplicationStatus = "Rejected";
				UpdateStatusDoneOrRejectedModel rejectedModel = new UpdateStatusDoneOrRejectedModel
				{
					Id = (Guid)SelectProcessRequest.Id!,
					ApplicationStatus = SelectProcessRequest.ApplicationStatus,
					InternalComment = comment,
					CompletedAt = DateTime.Now.ToString("f")
				};

				await _processRequestApi.UpdateStatusDoneOrRejectedAsync(rejectedModel);
				await FinalMethodAsync();
				CloseAction?.Invoke();
			}
		}

		/// <summary>
		/// Метод для установки статуса заявки "Отложена".
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Postponed()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			DateTime? selectedDate = null;
			string? comment = null;

			DetectionActivWindow(out SelectRequestChore? activeWinChore, out SelectRequestRepair? activeWinRepair);

			if (DateTime.TryParse(SelectProcessRequest.PlannedAt, out DateTime oldPlanDt))
			{
				var dialogDate = new DateSelectionDialog(oldPlanDt)
				{
					Owner = activeWinChore is null ? activeWinRepair : activeWinChore
				};

				if (dialogDate.ShowDialog() == true && dialogDate.SelectedDate != null)
					selectedDate = dialogDate.SelectedDate;
			}

			var dialogComment = new CommentSelectionDialog()
			{
				Owner = activeWinChore is null ? activeWinRepair : activeWinChore
			};

			if (dialogComment.ShowDialog() == true && !string.IsNullOrEmpty(dialogComment.Comment))
				comment = string.IsNullOrEmpty(SelectProcessRequest.InternalComment)
					? $"Отложена: {dialogComment.Comment}"
					: $"{SelectProcessRequest.InternalComment}.\nОтложена: {dialogComment.Comment}";


			if (selectedDate != null && !string.IsNullOrEmpty(comment))
			{
				SelectProcessRequest.ApplicationStatus = "Postponed";
				UpdateStatusInWorkOrReturnedOrPostponedRequestModel updateStatusRejected = new UpdateStatusInWorkOrReturnedOrPostponedRequestModel
				{
					Id = (Guid)SelectProcessRequest.Id!,
					ApplicationStatus = SelectProcessRequest.ApplicationStatus,
					InternalComment = comment,
					PlannedAt = selectedDate.ToString()
				};

				await _processRequestApi.UpdateStatusInWorkOrReturnedOrPostponedRequestAsync(updateStatusRejected);
				await FinalMethodAsync();
				CloseAction?.Invoke();
			}
		}

		private void DetectionActivWindow(out SelectRequestChore? activeWinChore, out SelectRequestRepair? activeWinRepair)
		{
			activeWinChore = null;
			activeWinRepair = null;
			if (SelectProcessRequest.ApplicationType == "HouseholdChores")

				activeWinChore = Application.Current.Windows
							.OfType<SelectRequestChore>()
							.FirstOrDefault(w => w.IsActive);
			else
				activeWinRepair = Application.Current.Windows
							.OfType<SelectRequestRepair>()
							.FirstOrDefault(w => w.IsActive);
		}

		/// <summary>
		/// Метод для комментария специалиста в заявке.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task SaveComment()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			bool _ = string.Equals(SelectProcessRequest.InternalComment, OldInternalComment, StringComparison.OrdinalIgnoreCase);

			if (_) MessageBox.Show("Комментарий не был изменен. Сохранение не требуется");

			UpdateInternalCommentRequestModel updateInternal = new()
			{
				Id = (Guid)SelectProcessRequest.Id!,
				InternalComment = SelectProcessRequest.InternalComment,
			};

			await _processRequestApi.UpdateInternalCommentRequestAsync(updateInternal);

			await FinalMethodAsync();
		}

		/// <summary>
		/// Команда для перенаправления заявки на ремонт другому исполнителю.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task Redirected()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			Employee? selectedEmployee = null;
			string? comment = null;

			// Проверяем, является ли сотрудник исполнителем заявки
			if (_globalEmployee.Employee.Role != "Executer")
			{
				MessageBox.Show("Вы не можете перенаправить заявку, так как не являетесь исполнителем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Получаем список доступных исполнителей
			var availableExecutors = await _employeeService.GetByRoleEmployeesAsync("Executer");

			DetectionActivWindow(out SelectRequestChore? activeWinChore, out SelectRequestRepair? activeWinRepair);

			// Показываем диалог выбора
			var dialogEmployee = new EmployeeSelectionDialog(availableExecutors.Where(_ => _.Id != _globalEmployee.Employee.Id))
			{
				Owner = activeWinChore is null ? activeWinRepair : activeWinChore
			};

			// Проверяем, что диалог был закрыт с результатом "ОК" и выбран сотрудник
			if (dialogEmployee.ShowDialog() == true && dialogEmployee.SelectedEmployee != null)
				selectedEmployee = dialogEmployee.SelectedEmployee;

			var dialogComment = new CommentSelectionDialog()
			{
				Owner = activeWinChore is null ? activeWinRepair : activeWinChore
			};

			if (dialogComment.ShowDialog() == true && !string.IsNullOrEmpty(dialogComment.Comment))
				comment = string.IsNullOrEmpty(SelectProcessRequest.InternalComment)
					? $"Перенаправлена: {dialogComment.Comment}"
					: $"{SelectProcessRequest.InternalComment}.\nПеренаправлена: {dialogComment.Comment}";

			if (selectedEmployee != null && !string.IsNullOrEmpty(comment))
			{
				// Создаем модель для обновления статуса заявки
				UpdateStatusRedirectedRequestModel updateStatusRedirected = new UpdateStatusRedirectedRequestModel
				{
					Id = (Guid)SelectProcessRequest.Id!,
					ApplicationStatus = "Redirected",
					InternalComment = comment,
					ExecutorEmployeeId = selectedEmployee.Id,
				};

				// Перенаправляем заявку
				await _processRequestApi.UpdateStatusRedirectedRequestAsync(updateStatusRedirected);

				// Обновляем данные представления после перенаправления
				SelectProcessRequest.ExecutorEmployee = selectedEmployee;
				await FinalMethodAsync();
				CloseAction?.Invoke();
			}
		}

		#endregion

		#region Коллекции и св-ва для формы Выбранная заявка 

		public Action? CloseAction { get; set; }


		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть кнопку "Возврат".
		/// </summary>
		[ObservableProperty]
		private Visibility _isExecutorVisibilityReturnedButton;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть кнопку "Отклонить".
		/// </summary>
		[ObservableProperty]
		private Visibility _isExecutorVisibilityRejectedButton;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть кнопку "Отложить".
		/// </summary>
		[ObservableProperty]
		private Visibility _isExecutorVisibilityPostponedButton;

		/// <summary>
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть кнопку "Перенаправить".
		/// </summary>
		[ObservableProperty]
		private Visibility _isExecutorVisibilityRedirectedButton;

		/// <summary>
		/// Старый комментарий
		/// </summary>
		[ObservableProperty]
		private string? _oldInternalComment;

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
		/// Флаг, указывающий, что сотрудник за компьютером является исполнителем заявки и может видеть Кнопки управления статусом.
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
		private ObservableCollection<InstrumentKind>? _kinds;

		/// <summary>
		/// Свойство для хранения выбранного вида оборудования.
		/// </summary>
		[ObservableProperty]
		private InstrumentKind? _itemKind;

		/// <summary>
		/// Свойство для хранения списка моделей оборудования.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<InstrumentModel>? _models;

		/// <summary>
		/// Свойство для хранения выбранной модели оборудования.
		/// </summary>
		[ObservableProperty]
		private InstrumentModel? _itemModel;

		/// <summary>
		/// Свойство для хранения списка типов оборудования.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<InstrumentType>? _equipmentTypes;

		/// <summary>
		/// Свойство для хранения выбранного типа оборудования.
		/// </summary>
		public InstrumentType? ItemEqType
		{
			get => _itemEqType;
			set
			{
				SetProperty(ref _itemEqType, value);

				Kinds = new ObservableCollection<InstrumentKind>(KindsOrig
					.Where(_ => _.Type.Id == ItemEqType?.Id)
					.ToList());

				TypeBreakdowns = new ObservableCollection<TypeBreakdown>(TypeBreakdownsOrig.
					Where(_ => _.Type.Id == ItemEqType?.Id)
					.ToList());
			}
		}
		private InstrumentType? _itemEqType;

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
		private IEnumerable<InstrumentKind>? _kindsOrig;

		[ObservableProperty]
		private IEnumerable<TypeBreakdown>? _typeBreakdownsOrig;
		#endregion
	}
}
