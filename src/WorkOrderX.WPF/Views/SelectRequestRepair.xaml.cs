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
	}
}
