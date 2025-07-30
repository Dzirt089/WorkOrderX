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
		private ActiveRequestViewModel _activeRequestViewModel;


		public MainViewModel(
			IEmployeeApiService employeeApi,
			GlobalEmployeeForApp globalEmployee,
			IMapper mapper,
			INavigationService navigationService,
			NewRequestRepairViewModel requestRepairViewModel,
			ActiveRequestViewModel activeRequestViewModel)
		{
			GlobalEmployee = globalEmployee;

			_employeeApi = employeeApi;
			_mapper = mapper;

			_navigationService = navigationService;
			_navigationService.CurrentViewModelChanged += OnNavigationServiceCurrentViewModelChanged;
			_navigationService.NavigateTo<NewRequestRepairViewModel>();
			_requestRepairViewModel = requestRepairViewModel;
			_activeRequestViewModel = activeRequestViewModel;
		}

		#region Methods

		/// <summary>
		/// Инициализация данных для главного окна приложения.
		/// </summary>
		/// <returns></returns>
		public async Task InitializationAsync()
		{
			// Установка учетной записи для входа
			GlobalEmployee.Employee.Account = "ceh17";//Environment.UserName;//"ceh17"//"ceh09";//

			// Вход в систему и получение токена
			var response = await _employeeApi.LoginAndAuthorizationAsync(GlobalEmployee.Employee.Account)
				?? throw new Exception("Ошибка входа в систему. Проверьте учетные данные.");

			// Преобразование ответа в модель LoginResponse
			LoginResponse? loginResp = _mapper.Map<LoginResponse>(response);

			GlobalEmployee.Employee = loginResp.Employee;
			GlobalEmployee.Token = loginResp.Token;

			// Инициализация представлений "Новая заявка на ремонт" и "Активные заявки".
			await _requestRepairViewModel.InitializationAsync();
			await _activeRequestViewModel.InitializationAsync();

			// Инициализация SignalR для получения уведомлений об изменениях заявок.
			await InitializeSignalR();
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
				.WithAutomaticReconnect() // Автоматическое переподключение
				.Build();

			// Обработка события получения уведомления об изменении заявки
			_hubConnection.On<string>("ProcessRequestChanged", async (requestId) =>
			{
				// Получение обновленных данных активных заявок
				await Application.Current.Dispatcher.InvokeAsync(async () =>
				{
					await _activeRequestViewModel.InitializationAsync(); // Обновление списка активных заявок
				});
			});

			// Старт подключения к SignalR и подписка на события
			await _hubConnection.StartAsync();
		}

		/// <summary>
		/// Освобождение ресурсов и остановка подключения к SignalR.
		/// </summary>
		/// <returns></returns>
		public async Task DisposeAsync()
		{
			if (_hubConnection != null)
			{
				await _hubConnection.StopAsync(); // Остановка подключения
				await _hubConnection.DisposeAsync(); // Освобождение ресурсов
			}
		}

		/// <summary>
		/// Обработчик события изменения текущей модели представления в навигационном сервисе.
		/// </summary>
		private void OnNavigationServiceCurrentViewModelChanged()
		{
			CurrentPageTitle = _navigationService.CurrentViewModel switch
			{
				NewRequestRepairViewModel => "Новая заявка",
				ActiveRequestViewModel => "Активные заявки",
				_ => "Приложение"
			};
		}
		#endregion

		#region Коллекции и св-ва 

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
		#endregion
	}
}
