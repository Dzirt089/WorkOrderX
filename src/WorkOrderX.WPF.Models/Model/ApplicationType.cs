using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	/// <summary>
	/// Тип заявки
	/// </summary>
	public partial class ApplicationType : ViewModelBase
	{
		[ObservableProperty]
		private int _id;

		[ObservableProperty]
		private string _name;

		[ObservableProperty]
		private string _description;
	}
}
