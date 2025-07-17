using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	/// <summary>
	/// Вид оборудования
	/// </summary>
	public partial class EquipmentKind : ViewModelBase
	{
		[ObservableProperty]
		private int _id;

		[ObservableProperty]
		private string _name;

		[ObservableProperty]
		private EquipmentType _type;

		[ObservableProperty]
		private string _description;
	}
}
