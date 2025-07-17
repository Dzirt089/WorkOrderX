using AutoMapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
		private NewRequestRepairViewModel _requestRepairViewModel;

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
			NewRequestRepairViewModel requestRepairViewModel)
		{
			_employeeApi = employeeApi;
			GlobalEmployee = globalEmployee;
			_mapper = mapper;

			_navigationService = navigationService;
			_navigationService.CurrentViewModelChanged += OnNavigationServiceCurrentViewModelChanged;
			_navigationService.NavigateTo<NewRequestRepairViewModel>();
			_requestRepairViewModel = requestRepairViewModel;
		}

		public async Task InitializationAsync()
		{
			GlobalEmployee.Employee.Account = Environment.UserName;
			var response = await _employeeApi.LoginAsync(GlobalEmployee.Employee.Account);
			LoginResponse? loginResp = _mapper.Map<LoginResponse>(response);

			GlobalEmployee.Employee = loginResp.Employee;
			GlobalEmployee.Token = loginResp.Token;

			await _requestRepairViewModel.InitializationAsync();
		}

		private void OnNavigationServiceCurrentViewModelChanged()
		{
			CurrentPageTitle = _navigationService.CurrentViewModel switch
			{
				NewRequestRepairViewModel => "Новая заявка",
				_ => "Приложение"
			};
		}

		[RelayCommand]
		private void ToggleMenu() => IsMenuExpanded = !IsMenuExpanded;

		[RelayCommand]
		private void NavigateToNewRequestRepair() => _navigationService.NavigateTo<NewRequestRepairViewModel>();
	}
}
