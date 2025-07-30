using System.Windows;

using WorkOrderX.WPF.ViewModel;

namespace WorkOrderX.WPF.Views
{
	/// <summary>
	/// Логика взаимодействия для SelectRequestRepair.xaml
	/// </summary>
	public partial class SelectRequestRepair : Window
	{
		public SelectRequestRepair(SelectRequestRepairViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}

		/// <summary>
		/// Команда закрытия окна при нажатии кнопки "Закрыть" или "Отмена".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Получаем ViewModel из DataContext окна.
			var vm = (SelectRequestRepairViewModel)DataContext;

			// Проверяем, нужно ли сохранять изменения перед закрытием окна.
			bool shouldClose = await vm.CheckAndSaveBeforeClosing();

			// Если нужно закрыть окно, то отменяем событие закрытия.
			e.Cancel = !shouldClose;
		}
	}
}
