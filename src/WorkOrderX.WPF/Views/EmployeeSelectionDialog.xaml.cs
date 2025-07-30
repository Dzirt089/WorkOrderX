using System.Windows;

using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Views
{
	/// <summary>
	/// Логика взаимодействия для EmployeeSelectionDialog.xaml
	/// </summary>
	public partial class EmployeeSelectionDialog : Window
	{
		/// <summary>
		/// Свойство для хранения выбранного сотрудника.
		/// </summary>
		public Employee SelectedEmployee { get; set; }

		/// <summary>
		/// Список сотрудников, доступных для выбора.
		/// </summary>
		public IEnumerable<Employee> Employees { get; }

		/// <summary>
		/// Конструктор для инициализации диалога выбора сотрудника.
		/// </summary>
		/// <param name="employees"></param>
		public EmployeeSelectionDialog(IEnumerable<Employee> employees)
		{
			InitializeComponent();
			Employees = employees;
			DataContext = this;
		}

		/// <summary>
		/// Метод, вызываемый при нажатии кнопки "Выбрать".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Select_Click(object sender, RoutedEventArgs e)
		{
			if (SelectedEmployee != null)
			{
				DialogResult = true; // Устанавливаем результат диалога как успешный
				Close(); // Закрываем диалог
			}
			else
			{
				MessageBox.Show("Пожалуйста, выберите сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Метод, вызываемый при нажатии кнопки "Отмена".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false; // Устанавливаем результат диалога как отмененный
			Close(); // Закрываем диалог
		}
	}
}
