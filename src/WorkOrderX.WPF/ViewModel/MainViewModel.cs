using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.AspNetCore.SignalR.Client;

using WorkOrderX.ApiClients.Employees.Interfaces;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class MainViewModel : ViewModelBase
	{
		private readonly IEmployeeApiService _employeeApi;
		private readonly IMapper _mapper;
		private readonly INavigationService _navigationService;

		private HubConnection _hubConnection;

		private NewRequestRepairViewModel _requestRepairViewModel;
		private ActiveRequestViewModel _activeRequestViewModel;

		[ObservableProperty]
		private GlobalEmployeeForApp _globalEmployee;

		[ObservableProperty]
		private string _currentPageTitle = "Новая заявка";

		[ObservableProperty]
		private bool _isMenuExpanded = true;

		public INavigationService NavigationService => _navigationService;

		public MainViewModel(
			IEmployeeApiService employeeApi,
			GlobalEmployeeForApp globalEmployee,
			IMapper mapper,
			INavigationService navigationService,
			NewRequestRepairViewModel requestRepairViewModel,
			ActiveRequestViewModel activeRequestViewModel)
		{
			_employeeApi = employeeApi;
			GlobalEmployee = globalEmployee;
			_mapper = mapper;

			_navigationService = navigationService;
			_navigationService.CurrentViewModelChanged += OnNavigationServiceCurrentViewModelChanged;
			_navigationService.NavigateTo<NewRequestRepairViewModel>();
			_requestRepairViewModel = requestRepairViewModel;
			_activeRequestViewModel = activeRequestViewModel;
		}

		public async Task InitializationAsync()
		{
			GlobalEmployee.Employee.Account = "ceh09";//Environment.UserName;//"ceh17"
			var response = await _employeeApi.LoginAsync(GlobalEmployee.Employee.Account);
			LoginResponse? loginResp = _mapper.Map<LoginResponse>(response);

			GlobalEmployee.Employee = loginResp.Employee;
			GlobalEmployee.Token = loginResp.Token;

			await _requestRepairViewModel.InitializationAsync();
			await _activeRequestViewModel.InitializationAsync();
			await InitializeSignalR();
		}

		public async Task InitializeSignalR()
		{
			_hubConnection = new HubConnectionBuilder()
				.WithUrl($"{Settings.Default.WorkOrderXApi}/RequestChanged")
				.WithAutomaticReconnect()
				.Build();

			_hubConnection.On<string>("ProcessRequestChanged", async (requestId) =>
			{
				await _activeRequestViewModel.InitializationAsync();
			});

			await _hubConnection.StartAsync();
		}

		public async Task DisposeAsync()
		{
			if (_hubConnection != null)
			{
				await _hubConnection.StopAsync();
				await _hubConnection.DisposeAsync();
			}
		}

		private void OnNavigationServiceCurrentViewModelChanged()
		{
			CurrentPageTitle = _navigationService.CurrentViewModel switch
			{
				NewRequestRepairViewModel => "Новая заявка",
				ActiveRequestViewModel => "Активные заявки",
				_ => "Приложение"
			};
		}

		[RelayCommand]
		private void ToggleMenu() => IsMenuExpanded = !IsMenuExpanded;

		[RelayCommand]
		private void NavigateToNewRequestRepair() => _navigationService.NavigateTo<NewRequestRepairViewModel>();

		[RelayCommand]
		private void NavigateToActiveRequests() => _navigationService.NavigateTo<ActiveRequestViewModel>();

	}
}
