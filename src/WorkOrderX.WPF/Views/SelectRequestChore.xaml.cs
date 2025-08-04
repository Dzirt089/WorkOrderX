using System.Windows;

using WorkOrderX.WPF.ViewModel;

namespace WorkOrderX.WPF.Views
{
	/// <summary>
	/// Логика взаимодействия для SelectRequestChore.xaml
	/// </summary>
	public partial class SelectRequestChore : Window
	{
		public SelectRequestChore(SelectRequestChoreViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;

			// Подписываемся на событие закрытия окна
			this.Loaded += (_, _) =>
			{
				if (DataContext is SelectRequestChoreViewModel selectRequestChoreViewModel)
					selectRequestChoreViewModel.CloseAction = () => this.Close();
			};
		}

		/// <summary>
		/// Команда закрытия окна при нажатии кнопки "Закрыть" или "Отмена".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Получаем ViewModel из DataContext окна.
			var vm = (SelectRequestChoreViewModel)DataContext;

			// Проверяем, нужно ли сохранять изменения перед закрытием окна.
			bool shouldClose = await vm.CheckAndSaveBeforeClosing();

			// Если нужно закрыть окно, то отменяем событие закрытия.
			e.Cancel = !shouldClose;
		}
	}
}
