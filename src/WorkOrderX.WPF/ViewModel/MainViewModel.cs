using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.AspNetCore.SignalR.Client;

using System.Windows;

using WorkOrderX.ApiClients.Employees.Interfaces;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	/// <summary>
	/// ViewModel для главного окна приложения, управляющий навигацией и состоянием приложения.
	/// </summary>
	public partial class MainViewModel : ViewModelBase
	{
		private readonly IEmployeeApiService _employeeApi;
		private readonly IMapper _mapper;
		private readonly INavigationService _navigationService;

		/// <summary>
		/// Навигационный сервис, используемый для управления переходами между представлениями.
		/// </summary>
		public INavigationService NavigationService => _navigationService;


		private HubConnection _hubConnection;
		private NewRequestRepairViewModel _requestRepairViewModel;
		private NewRequestChoreViewModel _requestChoreViewModel;
		private ActiveRequestViewModel _activeRequestViewModel;
		private HistoryRequestViewModel _historyRequestViewModel;

		private Timer _pollingTimer;
		private bool _isStopping = false;

		public MainViewModel(
			IEmployeeApiService employeeApi,
			GlobalEmployeeForApp globalEmployee,
			IMapper mapper,
			INavigationService navigationService,
			NewRequestRepairViewModel requestRepairViewModel,
			ActiveRequestViewModel activeRequestViewModel,
			HistoryRequestViewModel historyRequestViewModel,
			NewRequestChoreViewModel requestChoreViewModel)
		{
			GlobalEmployee = globalEmployee;

			_employeeApi = employeeApi;
			_mapper = mapper;

			_navigationService = navigationService;
			_navigationService.CurrentViewModelChanged += OnNavigationServiceCurrentViewModelChanged;
			_navigationService.NavigateTo<NewRequestRepairViewModel>();
			_requestRepairViewModel = requestRepairViewModel;
			_activeRequestViewModel = activeRequestViewModel;
			_historyRequestViewModel = historyRequestViewModel;
			_requestChoreViewModel = requestChoreViewModel;
		}

		#region Methods

		/// <summary>
		/// Инициализация данных для главного окна приложения.
		/// </summary>
		/// <returns></returns>
		public async Task InitializationAsync()
		{
			// Установка учетной записи для входа
			GlobalEmployee.Employee.Account = Environment.UserName;//"ceh17";//"teho12";//"ceh09";//Environment.UserName;//"ceh17"//"ceh09";//"teho12";//"okad01";//

			// Вход в систему и получение токена
			var response = await _employeeApi.LoginAndAuthorizationAsync(GlobalEmployee.Employee.Account)
				?? throw new Exception("Ошибка входа в систему. Проверьте учетные данные.");

			// Преобразование ответа в модель LoginResponse
			LoginResponse? loginResp = _mapper.Map<LoginResponse>(response);

			GlobalEmployee.Employee = loginResp.Employee;
			GlobalEmployee.Token = loginResp.Token;

			// Инициализация представлений "Новая заявка на ремонт" и "Активные заявки", "История заявок".
			await _requestRepairViewModel.InitializationAsync();
			await _requestChoreViewModel.InitializationAsync();
			await _activeRequestViewModel.InitializationAsync();
			await _historyRequestViewModel.InitializationAsync();

			// Инициализация SignalR для получения уведомлений об изменениях заявок.
			await InitializeSignalR();
			StartPolling(TimeSpan.FromMinutes(1));
		}

		private void StartPolling(TimeSpan interval)
		{
			_pollingTimer = new Timer(async _ =>
			{
				if (Application.Current?.Dispatcher != null)
				{
					// Получение обновленных данных активных заявок
					await Application.Current.Dispatcher.InvokeAsync(async () =>
					{
						await _activeRequestViewModel.InitializationAsync(); // Обновление списка активных заявок
						await _historyRequestViewModel.InitializationAsync(); // Обновление списка истории заявок
					});
				}
				else
				{
					await _activeRequestViewModel.InitializationAsync();
					await _historyRequestViewModel.InitializationAsync();
				}
			}, null, interval, interval);
		}

		/// <summary>
		/// Инициализация SignalR для получения уведомлений об изменениях заявок.
		/// </summary>
		/// <returns></returns>
		public async Task InitializeSignalR()
		{
			// Настройка подключения к SignalR
			_hubConnection = new HubConnectionBuilder()
				.WithUrl($"{Settings.Default.WorkOrderXApi}/RequestChanged") // URL вашего SignalR хаба					
				.WithAutomaticReconnect(new[] //Создание подключения с автоматическим восстановлением
				{
					TimeSpan.Zero,
					TimeSpan.FromSeconds(2),
					TimeSpan.FromSeconds(10),
					TimeSpan.FromSeconds(30)
				}) //Интервал попыток: сразу(0), затем через 2, 10 и 30 секунд.После этого если не получилось, соединение переходит в состояние Disconnected, и событие Closed срабатывает.
				.Build();

			// Подписка на событие ProcessRequestChanged
			_hubConnection.On<string>("ProcessRequestChanged", async (requestId) =>
			{
				if (Application.Current?.Dispatcher != null)
				{
					// Получение обновленных данных активных заявок
					await Application.Current.Dispatcher.InvokeAsync(async () =>
					{
						await _activeRequestViewModel.InitializationAsync(); // Обновление списка активных заявок
						await _historyRequestViewModel.InitializationAsync(); // Обновление списка истории заявок
					});
				}
				else
				{
					await _activeRequestViewModel.InitializationAsync();
					await _historyRequestViewModel.InitializationAsync();
				}
			});

			_hubConnection.Reconnecting += error =>
			{
				// возможно показать UI, что reconnect
				MessageBox.Show("Переподключение...");
				return Task.CompletedTask;
			};
			_hubConnection.Reconnected += id =>
			{
				// лог, можно уведомить UI
				MessageBox.Show("Переподключился!");
				return Task.CompletedTask;
			};

			// Старт подключения к SignalR и подписка на события
			await _hubConnection.StartAsync();

			//Если все попытки reconnect неудачные, срабатывает Closed. Он делает паузу 5 секунд, а затем вручную вызывает StartAsync() — чтобы попытаться восстановить подключение самостоятельно
			_hubConnection.Closed += _hubConnection_Closed;
		}

		private async Task _hubConnection_Closed(Exception? arg)
		{
			if (_isStopping) return;
			try
			{
				if (_hubConnection?.State == HubConnectionState.Disconnected)
				{
					await Task.Delay(5000);
					await _hubConnection.StartAsync();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				throw;
			}
		}


		/// <summary>
		/// Освобождение ресурсов и остановка подключения к SignalR.
		/// </summary>
		/// <returns></returns>
		public async Task DisposeAsync()
		{
			_isStopping = true;
			_hubConnection.Closed -= _hubConnection_Closed;
			_pollingTimer?.Dispose(); // Остановка таймера

			if (_hubConnection != null)
			{
				await _hubConnection.StopAsync().ConfigureAwait(false); // Остановка подключения
				await _hubConnection.DisposeAsync().ConfigureAwait(false); // Освобождение ресурсов
			}
		}

		/// <summary>
		/// Обработчик события изменения текущей модели представления в навигационном сервисе.
		/// </summary>
		private void OnNavigationServiceCurrentViewModelChanged()
		{
			CurrentPageTitle = _navigationService.CurrentViewModel switch
			{
				NewRequestRepairViewModel => "Новая заявка на ремонт",
				NewRequestChoreViewModel => "Новая заявка на хоз. работы",
				ActiveRequestViewModel => "Активные заявки",
				HistoryRequestViewModel => "История заявок",
				_ => "Приложение"
			};

			IsSelectRepair = _navigationService.CurrentViewModel switch
			{
				NewRequestRepairViewModel => true,
				NewRequestChoreViewModel => false,
				_ => true
			};

			IsSelectChore = _navigationService.CurrentViewModel switch
			{
				NewRequestRepairViewModel => false,
				NewRequestChoreViewModel => true,
				_ => false
			};

			VisibilitySelectRequest = _navigationService.CurrentViewModel switch
			{
				NewRequestRepairViewModel => Visibility.Visible,
				NewRequestChoreViewModel => Visibility.Visible,
				_ => Visibility.Collapsed
			};
		}
		#endregion

		#region Коллекции и св-ва 

		[ObservableProperty]
		private Visibility _visibilitySelectRequest;

		/// <summary>
		/// Глобальный сотрудник для приложения, содержащий информацию о текущем пользователе.
		/// </summary>
		[ObservableProperty]
		private GlobalEmployeeForApp _globalEmployee;

		/// <summary>
		/// Текущий заголовок страницы, отображаемый в интерфейсе.
		/// </summary>
		[ObservableProperty]
		private string _currentPageTitle = "Новая заявка";

		/// <summary>
		/// Флаг, указывающий, развернуто ли меню навигации.
		/// </summary>
		[ObservableProperty]
		private bool _isMenuExpanded = true;

		[ObservableProperty]
		private bool _isSelectRepair = true;

		[ObservableProperty]
		private bool _isSelectChore = false;
		#endregion

		#region Commands

		/// <summary>
		/// Команда для переключения состояния меню навигации (развернуто/свернуто).
		/// </summary>
		[RelayCommand]
		private void ToggleMenu() => IsMenuExpanded = !IsMenuExpanded;

		/// <summary>
		/// Команда для навигации к представлению "Новая заявка на ремонт".
		/// </summary>
		[RelayCommand]
		private void NavigateToNewRequestRepair() => _navigationService.NavigateTo<NewRequestRepairViewModel>();

		/// <summary>
		/// Команда для навигации к представлению "Активные заявки".
		/// </summary>
		[RelayCommand]
		private void NavigateToActiveRequests() => _navigationService.NavigateTo<ActiveRequestViewModel>();

		/// <summary>
		/// Команда для навигации к представлению "История заявок".
		/// </summary>
		[RelayCommand]
		private void NavigateToHistoryRequests() => _navigationService.NavigateTo<HistoryRequestViewModel>();

		/// <summary>
		/// Команда для навигации к представлению "Новая заявка на хоз. работы".
		/// </summary>
		[RelayCommand]
		private void NavigateToNewRequestChore() => _navigationService.NavigateTo<NewRequestChoreViewModel>();
		#endregion
	}
}
