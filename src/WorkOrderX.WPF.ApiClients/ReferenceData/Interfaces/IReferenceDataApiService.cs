using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.ReferenceData.Interfaces
{
	/// <summary>
	/// Интерфейс для работы с API справочных данных.
	/// </summary>
	public interface IReferenceDataApiService : IWorkOrderXApi
	{
		/// <summary>
		/// Метод для получения всех статусов заявок.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ApplicationStatusDataModel?>> GetAllApplicationStatusAsync(CancellationToken token = default);

		/// <summary>
		/// Метод для получения всех типов заявок.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ApplicationTypeDataModel?>> GetAllApplicationTypeAsync(CancellationToken token = default);

		/// <summary>
		/// Метод для получения всех видов оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<InstrumentKindDataModel?>> GetAllEquipmentKindAsync(CancellationToken token = default);

		/// <summary>
		/// Метод для получения всех моделей оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<InstrumentModelDataModel?>> GetAllEquipmentModelAsync(CancellationToken token = default);

		/// <summary>
		/// Метод для получения всех типов оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<InstrumentTypeDataModel?>> GetAllEquipmentTypeAsync(CancellationToken token = default);

		/// <summary>
		/// Метод для получения всех видов поломок у оборудования.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<TypeBreakdownDataModel?>> GetAllTypeBreakdownAsync(CancellationToken token = default);

		/// <summary>
		/// Метод для получения всех уровней важности заявок.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<IEnumerable<ImportancesDataModel?>> GetAllImportancesAsync(CancellationToken token = default);
	}
}
