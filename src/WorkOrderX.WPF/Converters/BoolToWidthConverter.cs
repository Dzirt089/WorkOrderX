using System.Globalization;
using System.Windows.Data;

namespace WorkOrderX.WPF.Converters
{
	/// <summary>
	/// Конвертер для преобразования булевого значения в ширину элемента управления.
	/// </summary>
	public class BoolToWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var sizes = parameter.ToString().Split('|');
			return (bool)value ? double.Parse(sizes[0]) : double.Parse(sizes[1]);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}
