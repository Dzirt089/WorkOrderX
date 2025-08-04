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
	public partial class NewRequestChoreViewModel : ViewModelBase
	{
		private readonly IReferenceDadaServices _referenceDada;
		private readonly IProcessRequestApiService _processRequestApi;
		private readonly GlobalEmployeeForApp _globalEmployee;


		public NewRequestChoreViewModel(IReferenceDadaServices referenceDada, IProcessRequestApiService processRequestApi, GlobalEmployeeForApp globalEmployee)
		{
			_referenceDada = referenceDada;
			_processRequestApi = processRequestApi;
			_globalEmployee = globalEmployee;

			// Инициализация свойства заявки для представления "Новой заявки" на ремонт.
			ProcessRequestNew = new ProcessRequest();

			// Обработчик для обновления доступности команды сохранения и отправки заявки.
			ProcessRequestNew.PropertyChanged += (_, _) =>
			{
				SaveAndSendRequestOrderCommand.NotifyCanExecuteChanged();
			};
		}

		/// <summary>
		/// Инициализация данных для представления "Новой заявки" на ремонт.
		/// </summary>
		/// <returns></returns>
		public async Task InitializationAsync()
		{
			await LoadAndInitialCollectionRefDataForFormNewRequest();

			// Создание шаблона "новой заявки" на ремонт.
			CreateTemplateNewRequest();
		}

		/// <summary>
		/// Загрузка и инициализация коллекций справочных данных для представления "Новой заявки" на ремонт.
		/// </summary>
		/// <returns></returns>
		public async Task LoadAndInitialCollectionRefDataForFormNewRequest()
		{
			// Получаем данные для заполнения ComboBox'ов для представления заявки
			(ObservableCollection<ApplicationStatus>? Statuse,
			ObservableCollection<ApplicationType>? AppTypes,
			_, _, _, _,
			ObservableCollection<Importance>? Importance) = await _referenceDada.GetAllRefenceDataInCollectionsAsync();

			// Полный список 
			Statuses = Statuse; // Статусов
			ApplicationTypes = AppTypes; // Тип заявки 		
			Importances = Importance; // Важность заявки	

			ItemImport = Importances.FirstOrDefault(_ => _.Id == 1);
		}

		/// <summary>
		/// Создание шаблона "новой заявки" на ремонт.
		/// </summary>
		private void CreateTemplateNewRequest()
		{
			ProcessRequestNew.CustomerEmployee = _globalEmployee.Employee;
			ProcessRequestNew.ApplicationNumber = 0;
			ProcessRequestNew.ApplicationType = "HouseholdChores";
			ProcessRequestNew.ApplicationStatus = "New";
		}

		/// <summary>
		/// Очистка полей для представления "Новой заявки" на ремонт.
		/// </summary>
		private void ClearFieldRequest()
		{
			ProcessRequestNew = new ProcessRequest();

			// Обработчик для обновления доступности команды сохранения и отправки заявки.
			ProcessRequestNew.PropertyChanged += (_, _) =>
			{
				SaveAndSendRequestOrderCommand.NotifyCanExecuteChanged();
			};

			ItemImport = Importances.FirstOrDefault(_ => _.Id == 1);
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
		/// Проверка наличия ошибок валидации для представления "Новой заявки".
		/// </summary>
		/// <returns></returns>
		private bool HasValidationError()
		{
			// Задаём только нужные свойства, которые надо проверить на валидацию.
			var propertiesToValidate = new[]
			{
				nameof(ProcessRequest.DescriptionMalfunction),
				nameof(ProcessRequest.Importance),
				nameof(ProcessRequest.Location)
			};

			var validationResults = new List<ValidationResult>();

			foreach (var propertyName in propertiesToValidate)
			{
				var propertyValue = typeof(ProcessRequest)
					.GetProperty(propertyName)? //Через рефлексию берём PropertyInfo для конкретного свойства.
					.GetValue(ProcessRequestNew); //GetValue вытаскивает текущее значение из объекта ProcessRequestNew.

				//Создаём контекст валидации, указывая, что проверяем ProcessRequestNew.
				var context = new ValidationContext(ProcessRequestNew)
				{
					MemberName = propertyName //MemberName - указывает, какое конкретно свойство ты хочешь валидировать.
				};

				//Читает атрибуты валидации на свойстве — например, [Required], [CustomValidation(...)], [Range(...)] и т.п.
				//Применяет их к propertyValue.
				//Если есть ошибка — добавляет её в список validationResults
				Validator.TryValidateProperty(propertyValue, context, validationResults);
			}

			// Если в списке нет ошибок — значит, всё прошло.
			return validationResults.Count == 0;
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
				// Усанавливаем дату создания
				ProcessRequestNew.CreatedAt = DateTime.Now.ToString();

				// Если важность заявки нормальная, то планируем на 2 дня вперёд
				if (ItemImport.Name == "Normal")
					ProcessRequestNew.PlannedAt = DateTime.Now.AddDays(2).ToString();

				// Если важность заявки высокая, то планируем на сегодня
				else if (ItemImport.Name == "High")
					ProcessRequestNew.PlannedAt = DateTime.Now.ToString();
				else
					throw new InvalidOperationException("Неизвестный уровень важности заявки.");

				// Создаём модель запроса для отправки на сервер
				CreateProcessRequestModel createProcess = new CreateProcessRequestModel()
				{
					ApplicationNumber = ProcessRequestNew.ApplicationNumber,
					ApplicationType = ProcessRequestNew.ApplicationType,
					ApplicationStatus = ProcessRequestNew.ApplicationStatus,
					CreatedAt = ProcessRequestNew.CreatedAt,
					PlannedAt = ProcessRequestNew.PlannedAt,

					DescriptionMalfunction = ProcessRequestNew.DescriptionMalfunction,
					Importance = ProcessRequestNew.Importance,
					InternalComment = ProcessRequestNew.InternalComment,
					Location = ProcessRequestNew.Location,
					CustomerEmployeeId = ProcessRequestNew.CustomerEmployee.Id,
				};

				bool _ = await _processRequestApi.CreateProcessRequestAsync(createProcess);

				if (_)
				{
					ClearFieldRequest();
					CreateTemplateNewRequest();

					MessageBox.Show(
						"Заявка успешно создана и отправлена на обработку.",
						"Успех",
						MessageBoxButton.OK,
						MessageBoxImage.Information);
				}
			}
		}

		/// <summary>
		/// Отмена заявки на ремонт и очистка полей формы.
		/// </summary>
		[RelayCommand]
		private void CancelRequestOrder()
		{
			ClearFieldRequest();
			CreateTemplateNewRequest();
		}


		/// <summary>
		/// Новая заявка на ремонт.
		/// </summary>
		[ObservableProperty]
		private ProcessRequest _processRequestNew;

		/// <summary>
		/// Свойство для хранения списка статусов заявки.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ApplicationStatus>? _statuses;

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
		public Importance? ItemImport
		{
			get => _itemImport;
			set
			{
				SetProperty(ref _itemImport, value);
				ProcessRequestNew.Importance = ItemImport?.Name;
			}
		}
		private Importance? _itemImport;
	}
}
