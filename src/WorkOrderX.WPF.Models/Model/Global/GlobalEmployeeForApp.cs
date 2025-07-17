using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model.Global
{
	public partial class GlobalEmployeeForApp : ViewModelBase
	{
		[ObservableProperty]
		private Employee _employee = new();

		[ObservableProperty]
		private string _token;
	}
}
