using System.Windows;

using WorkOrderX.WPF.ViewModel;

namespace WorkOrderX.WPF.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Получаем ViewModel из DataContext окна.
			var vm = (MainViewModel)DataContext;

			await vm.DisposeAsync();

			e.Cancel = false;
		}
	}
}