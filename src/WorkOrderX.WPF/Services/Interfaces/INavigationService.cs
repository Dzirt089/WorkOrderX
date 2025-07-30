using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Services.Interfaces
{
	/// <summary>
	/// Интерфейс навигации для управления текущей моделью представления.
	/// </summary>
	public interface INavigationService
	{
		/// <summary>
		/// Переход к указанной модели представления.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void NavigateTo<T>() where T : ViewModelBase;

		/// <summary>
		/// Текущая модель представления, используемая в навигации.
		/// </summary>
		ViewModelBase CurrentViewModel { get; }

		/// <summary>
		/// Событие, которое вызывается при изменении текущей модели представления.
		/// </summary>
		event Action? CurrentViewModelChanged;
	}
}
