using System.Text.Json;

using WorkOrderX.ApiClients.ReferenceData.Interfaces;
using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ReferenceData.Implementation
{
	public class ReferenceDataApiService : BaseApiClient, IReferenceDataApiService
	{
		public ReferenceDataApiService(
			IHttpClientFactory httpClientFactory,
			JsonSerializerOptions jsonOptions)
			: base(httpClientFactory.CreateClient("WorkOrderXApi"), jsonOptions)
		{
			_httpClient.BaseAddress = new Uri(_httpClient.BaseAddress, "ReferenceData/");
		}

		public async Task<IEnumerable<ApplicationStatusDataModel?>> GetAllApplicationStatusAsync(CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<ApplicationStatusDataModel?>>("GetAllApplicationStatus", token).ConfigureAwait(false);

		public async Task<IEnumerable<ApplicationTypeDataModel?>> GetAllApplicationTypeAsync(CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<ApplicationTypeDataModel>>("GetAllApplicationType", token).ConfigureAwait(false);

		public async Task<IEnumerable<EquipmentKindDataModel?>> GetAllEquipmentKindAsync(CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<EquipmentKindDataModel>>("GetAllEquipmentKind", token).ConfigureAwait(false);

		public async Task<IEnumerable<EquipmentModelDataModel?>> GetAllEquipmentModelAsync(CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<EquipmentModelDataModel>>("GetAllEquipmentModel", token).ConfigureAwait(false);

		public async Task<IEnumerable<EquipmentTypeDataModel?>> GetAllEquipmentTypeAsync(CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<EquipmentTypeDataModel>>("GetAllEquipmentType", token).ConfigureAwait(false);

		public async Task<IEnumerable<TypeBreakdownDataModel?>> GetAllTypeBreakdownAsync(CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<TypeBreakdownDataModel>>("GetAllTypeBreakdown", token).ConfigureAwait(false);

		public async Task<IEnumerable<ImportancesDataModel?>> GetAllImportancesAsync(CancellationToken token = default) =>
			await GetTJsonTAsync<IEnumerable<ImportancesDataModel>>("GetAllImportances", token).ConfigureAwait(false);
	}
}
