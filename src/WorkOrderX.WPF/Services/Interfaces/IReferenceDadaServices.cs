using System.Collections.ObjectModel;

using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Services.Interfaces
{
	/// <summary>
	/// Интерфейс для работы со справочными данными приложения.
	/// </summary>
	public interface IReferenceDadaServices
	{
		/// <summary>
		/// Получение всех справочных данных в виде ObservableCollection.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<(ObservableCollection<ApplicationStatus>? Statuses,
			ObservableCollection<ApplicationType>? AppTypes,
			ObservableCollection<InstrumentKind>? EqupKinds,
			ObservableCollection<InstrumentModel>? EqupModels,
			ObservableCollection<InstrumentType>? EqupTypes,
			ObservableCollection<TypeBreakdown>? Breaks,
			ObservableCollection<Importance>? Importances
			)> GetAllRefenceDataInCollectionsAsync(CancellationToken token = default);

		/// <summary>
		/// Получение всех справочных данных в виде коллекций IEnumerable.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<(IEnumerable<ApplicationStatus?> statuses,
			IEnumerable<ApplicationType?> appTypes,
			IEnumerable<InstrumentKind?> kinds,
			IEnumerable<InstrumentModel?> models,
			IEnumerable<InstrumentType?> equpTypes,
			IEnumerable<TypeBreakdown?> breaks,
			IEnumerable<Importance?> importances)>
			GetAllReferenceDataAsync(CancellationToken token = default);

		/// <summary>
		/// Получение справочных данных для инициализации формы Активные заявки, в виде коллекций IEnumerable.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<(IEnumerable<ApplicationStatus> statusesList,
			IEnumerable<Importance?> importancesList,
			IEnumerable<ApplicationType?> appTypesList)>
			GetRefDataForInitAsync(CancellationToken token = default);
	}
}
