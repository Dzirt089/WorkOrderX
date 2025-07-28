using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.Services.Implementation
{
	/// <summary>
	/// Сервис навигации для управления текущей моделью представления.
	/// </summary>
	public class NavigationService : INavigationService, INotifyPropertyChanged
	{
		private readonly IServiceProvider _serviceProvider;
		private ViewModelBase _currentViewModel;

		/// <summary>
		/// Событие, которое вызывается при изменении свойства модели представления.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Текущая модель представления, используемая в навигации.
		/// </summary>
		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			private set
			{
				_currentViewModel = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Событие, которое вызывается при изменении текущей модели представления.
		/// </summary>
		public event Action? CurrentViewModelChanged;

		public NavigationService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Переход к указанной модели представления.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void NavigateTo<T>() where T : ViewModelBase
		{
			// Проверка, что модель представления уже не является текущей
			CurrentViewModel = _serviceProvider.GetRequiredService<T>();
			// Вызываем событие, чтобы уведомить об изменении текущей модели представления
			CurrentViewModelChanged?.Invoke();
		}

		/// <summary>
		/// Вызывается при изменении текущей модели представления.
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
