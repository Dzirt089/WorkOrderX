using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkOrderX.WPF.Views
{
	/// <summary>
	/// Логика взаимодействия для EmployeeSelectionDialog.xaml
	/// </summary>
	public partial class DateSelectionDialog : Window
	{
		/// <summary>
		/// Свойство для хранения выбранной даты.
		/// </summary>
		public DateTime? SelectedDate { get; set; }

		/// <summary>
		/// Исходная дата для отображения.
		/// </summary>
		public DateTime CurrentDate { get; set; }

		/// <summary>
		/// Конструктор для инициализации диалога выбора даты.
		/// </summary>
		/// <param name="currentDate">Текущая дата для отображения</param>
		public DateSelectionDialog(DateTime currentDate)
		{
			InitializeComponent();
			CurrentDate = currentDate;
			SelectedDate = currentDate;
			DataContext = this;

			// Устанавливаем фокус на окно при загрузке
			Loaded += (s, e) => Keyboard.Focus(this);
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			Mouse.Capture(null); // снимаем захват мыши
			SelectButton.Focus(); // или другой фокусируемый элемент
		}


		private void Calendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			Mouse.Capture(null); // снимаем захват мыши
			SelectButton.Focus(); // или другой фокусируемый элемент
		}


		/// <summary>
		/// Метод, вызываемый при нажатии кнопки "Выбрать".
		/// </summary>
		private void Select_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		/// <summary>
		/// Метод, вызываемый при нажатии кнопки "Отмена".
		/// </summary>
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
