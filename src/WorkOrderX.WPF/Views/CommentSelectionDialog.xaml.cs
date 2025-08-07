using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WorkOrderX.WPF.Views
{
	public partial class CommentSelectionDialog : Window, INotifyPropertyChanged
	{
		private string _comment = string.Empty;
		private bool _isCommentValid = false;

		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Введенный комментарий
		/// </summary>
		public string Comment
		{
			get => _comment;
			set
			{
				_comment = value;
				IsCommentValid = !string.IsNullOrWhiteSpace(value);
				OnPropertyChanged(nameof(Comment));
			}
		}

		/// <summary>
		/// Флаг валидности комментария
		/// </summary>
		public bool IsCommentValid
		{
			get => _isCommentValid;
			set
			{
				_isCommentValid = value;
				OnPropertyChanged(nameof(IsCommentValid));
			}
		}

		public CommentSelectionDialog()
		{
			InitializeComponent();
			DataContext = this;
			Loaded += (s, e) => Keyboard.Focus(this);
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void Select_Click(object sender, RoutedEventArgs e)
		{
			if (IsCommentValid)
			{
				DialogResult = true;
				Close();
			}
			else
			{
				MessageBox.Show("Комментарий не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}