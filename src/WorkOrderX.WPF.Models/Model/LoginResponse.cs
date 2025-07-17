using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	public partial class LoginResponse : ViewModelBase
	{
		[ObservableProperty]
		private string _token;

		[ObservableProperty]
		private Employee _employee;
	}
}
