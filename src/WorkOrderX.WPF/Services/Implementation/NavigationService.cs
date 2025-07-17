using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.Services.Implementation
{
	public class NavigationService : INavigationService, INotifyPropertyChanged
	{
		private readonly IServiceProvider _serviceProvider;
		private ViewModelBase _currentViewModel;

		public event PropertyChangedEventHandler? PropertyChanged;

		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			private set
			{
				_currentViewModel = value;
				OnPropertyChanged();
			}
		}

		public event Action? CurrentViewModelChanged;

		public NavigationService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void NavigateTo<T>() where T : ViewModelBase
		{
			CurrentViewModel = _serviceProvider.GetRequiredService<T>();
			CurrentViewModelChanged?.Invoke();
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
