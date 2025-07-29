using System.Windows;

using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Views
{
	/// <summary>
	/// Логика взаимодействия для EmployeeSelectionDialog.xaml
	/// </summary>
	public partial class EmployeeSelectionDialog : Window
	{
		public Employee SelectedEmployee { get; set; }
		public IEnumerable<Employee> Employees { get; }

		public EmployeeSelectionDialog(IEnumerable<Employee> employees)
		{
			InitializeComponent();
			Employees = employees;
			DataContext = this;
		}

		private void Select_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
