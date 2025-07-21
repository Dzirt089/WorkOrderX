using System.Collections.ObjectModel;

using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Services.Interfaces
{
	public interface IReferenceDadaServices
	{
		Task<(ObservableCollection<ApplicationStatus>? Statuses,
			ObservableCollection<ApplicationType>? AppTypes,
			ObservableCollection<EquipmentKind>? EqupKinds,
			ObservableCollection<EquipmentModel>? EqupModels,
			ObservableCollection<EquipmentType>? EqupTypes,
			ObservableCollection<TypeBreakdown>? Breaks,
			ObservableCollection<Importance>? Importances
			)> GetAllRefenceDataAsync(CancellationToken token = default);
	}
}
