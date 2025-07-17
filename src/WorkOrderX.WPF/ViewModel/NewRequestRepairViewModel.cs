using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class NewRequestRepairViewModel : ViewModelBase
	{
		private readonly IReferenceDadaServices _referenceDada;

		private readonly GlobalEmployeeForApp _globalEmployee;

		public NewRequestRepairViewModel(IReferenceDadaServices referenceDada, GlobalEmployeeForApp globalEmployee)
		{
			_referenceDada = referenceDada;
			_globalEmployee = globalEmployee;
		}

		public async Task InitializationAsync()
		{
			(ObservableCollection<ApplicationStatus>? Statuses,
			ObservableCollection<ApplicationType>? AppTypes,
			ObservableCollection<EquipmentKind>? EqupKinds,
			ObservableCollection<EquipmentModel>? EqupModels,
			ObservableCollection<EquipmentType>? EqupTypes,
			ObservableCollection<TypeBreakdown>? Breaks) result = await _referenceDada.GetAllRefenceDataAsync();

			Statuses = result.Statuses;
			ApplicationTypes = result.AppTypes;
			Kinds = result.EqupKinds;
			Models = result.EqupModels;
			EquipmentTypes = result.EqupTypes;
			TypeBreakdowns = result.Breaks;

			ProcessRequestNew = new();
			ProcessRequestNew.CustomerEmployee = _globalEmployee.Employee;
		}

		[ObservableProperty]
		private ProcessRequest _processRequestNew;

		[ObservableProperty]
		private ObservableCollection<ApplicationStatus>? _statuses;
		[ObservableProperty]
		private ApplicationStatus _itemStatus;

		[ObservableProperty]
		private ObservableCollection<ApplicationType>? _applicationTypes;
		[ObservableProperty]
		private ApplicationType _itemAppType;

		[ObservableProperty]
		private ObservableCollection<EquipmentKind>? _kinds;
		[ObservableProperty]
		private EquipmentKind _itemKind;

		[ObservableProperty]
		private ObservableCollection<EquipmentModel>? _models;
		[ObservableProperty]
		private EquipmentModel _itemModel;

		[ObservableProperty]
		private ObservableCollection<EquipmentType>? _equipmentTypes;
		[ObservableProperty]
		private EquipmentType _itemEqType;

		[ObservableProperty]
		private ObservableCollection<TypeBreakdown>? _typeBreakdowns;
		[ObservableProperty]
		private TypeBreakdown _itemBreak;

	}
}
