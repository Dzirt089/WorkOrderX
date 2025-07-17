using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	/// <summary>
	/// Модель оборудования
	/// </summary>
	public partial class EquipmentModel : ViewModelBase
	{
		[ObservableProperty]
		private int _id;

		[ObservableProperty]
		private string _name;

		[ObservableProperty]
		private string _description;
	}
}
