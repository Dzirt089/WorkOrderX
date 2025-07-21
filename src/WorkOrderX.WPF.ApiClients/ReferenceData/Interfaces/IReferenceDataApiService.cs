using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ReferenceData.Interfaces
{
	public interface IReferenceDataApiService : IWorkOrderXApi
	{
		Task<IEnumerable<ApplicationStatusDataModel?>> GetAllApplicationStatusAsync(CancellationToken token = default);
		Task<IEnumerable<ApplicationTypeDataModel?>> GetAllApplicationTypeAsync(CancellationToken token = default);
		Task<IEnumerable<EquipmentKindDataModel?>> GetAllEquipmentKindAsync(CancellationToken token = default);
		Task<IEnumerable<EquipmentModelDataModel?>> GetAllEquipmentModelAsync(CancellationToken token = default);
		Task<IEnumerable<EquipmentTypeDataModel?>> GetAllEquipmentTypeAsync(CancellationToken token = default);
		Task<IEnumerable<TypeBreakdownDataModel?>> GetAllTypeBreakdownAsync(CancellationToken token = default);
		Task<IEnumerable<ImportancesDataModel?>> GetAllImportancesAsync(CancellationToken token = default);
	}
}
