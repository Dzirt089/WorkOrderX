using AutoMapper;

using Microsoft.Extensions.Caching.Memory;

using System.Collections.ObjectModel;

using WorkOrderX.ApiClients.ReferenceData.Interfaces;
using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.Services.Implementation
{
	/// <summary>
	/// Сервис для работы со справочными данными приложения.
	/// </summary>
	public class ReferenceDadaServices : ViewModelBase, IReferenceDadaServices
	{
		private readonly IReferenceDataApiService _referenceDataApi;
		private readonly IMapper _mapper;
		private readonly IMemoryCache _cache;
		private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10); // Кэш на 10 мин

		public ReferenceDadaServices(IReferenceDataApiService referenceDataApi, IMapper mapper, IMemoryCache cache)
		{
			_referenceDataApi = referenceDataApi;
			_mapper = mapper;
			_cache = cache;
		}

		/// <summary>
		/// Получение всех справочных данных в виде ObservableCollection.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<(
			ObservableCollection<ApplicationStatus>? Statuses,
			ObservableCollection<ApplicationType>? AppTypes,
			ObservableCollection<EquipmentKind>? EqupKinds,
			ObservableCollection<EquipmentModel>? EqupModels,
			ObservableCollection<EquipmentType>? EqupTypes,
			ObservableCollection<TypeBreakdown>? Breaks,
			ObservableCollection<Importance>? Importances
			)> GetAllRefenceDataInCollectionsAsync(CancellationToken token = default)
		{

			(IEnumerable<ApplicationStatus?> statusList,
				IEnumerable<ApplicationType?> appTypeList,
				IEnumerable<EquipmentKind?> kindsList,
				IEnumerable<EquipmentModel?> modelsList,
				IEnumerable<EquipmentType?> equpTypesList,
				IEnumerable<TypeBreakdown?> breaksList,
				IEnumerable<Importance?> importancesList) = await GetAllReferenceDataAsync(token: token);


			ObservableCollection<ApplicationStatus>? statusesObserbal = new ObservableCollection<ApplicationStatus>(statusList);
			ObservableCollection<ApplicationType>? appTypeObserbal = new ObservableCollection<ApplicationType>(appTypeList);
			ObservableCollection<EquipmentKind>? kindsObserbal = new ObservableCollection<EquipmentKind>(kindsList);
			ObservableCollection<EquipmentModel>? modelsObserbal = new ObservableCollection<EquipmentModel>(modelsList);
			ObservableCollection<EquipmentType>? equpTypesObserbal = new ObservableCollection<EquipmentType>(equpTypesList);
			ObservableCollection<TypeBreakdown>? breaksObserbal = new ObservableCollection<TypeBreakdown>(breaksList);
			ObservableCollection<Importance>? importancesObserbal = new ObservableCollection<Importance>(importancesList);

			return (statusesObserbal, appTypeObserbal, kindsObserbal, modelsObserbal, equpTypesObserbal, breaksObserbal, importancesObserbal);
		}

		/// <summary>
		/// Получение всех справочных данных в виде коллекций IEnumerable.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<(
			IEnumerable<ApplicationStatus?> statuses,
			IEnumerable<ApplicationType?> appTypes,
			IEnumerable<EquipmentKind?> kinds,
			IEnumerable<EquipmentModel?> models,
			IEnumerable<EquipmentType?> equpTypes,
			IEnumerable<TypeBreakdown?> breaks,
			IEnumerable<Importance?> importances)>
			GetAllReferenceDataAsync(CancellationToken token = default) =>

			// Используем кэш для хранения данных, чтобы избежать повторных запросов к API
			await _cache.GetOrCreateAsync("AllReferenceData", async _ =>
			{
				_.AbsoluteExpirationRelativeToNow = _cacheDuration; // Устанавливаем время жизни кэша
				return await LoadReferenceDataFromApiAsync(token); // Загружаем данные из API, если их нет в кэше или они устарели
			});


		/// <summary>
		/// Загрузка справочных данных из API и преобразование их в модели.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		private async Task<(
			IEnumerable<ApplicationStatus> statusList,
			IEnumerable<ApplicationType> appTypeList,
			IEnumerable<EquipmentKind> kindsList,
			IEnumerable<EquipmentModel> modelsList,
			IEnumerable<EquipmentType> equpTypesList,
			IEnumerable<TypeBreakdown> breaksList,
			IEnumerable<Importance> importancesList)>
			LoadReferenceDataFromApiAsync(CancellationToken token)
		{

			var statusTask = _referenceDataApi.GetAllApplicationStatusAsync(token);
			var appTypeTask = _referenceDataApi.GetAllApplicationTypeAsync(token);
			var kindsTask = _referenceDataApi.GetAllEquipmentKindAsync(token);
			var modelsTask = _referenceDataApi.GetAllEquipmentModelAsync(token);
			var equpTypeTask = _referenceDataApi.GetAllEquipmentTypeAsync(token);
			var typeBreakTask = _referenceDataApi.GetAllTypeBreakdownAsync(token);
			var importTask = _referenceDataApi.GetAllImportancesAsync(token);

			await Task.WhenAll(statusTask, appTypeTask, kindsTask, modelsTask, equpTypeTask, typeBreakTask, importTask);

			IEnumerable<ApplicationStatusDataModel?>? statuses = await statusTask;
			IEnumerable<ApplicationTypeDataModel?>? appTypes = await appTypeTask;
			IEnumerable<EquipmentKindDataModel?>? kinds = await kindsTask;
			IEnumerable<EquipmentModelDataModel?>? models = await modelsTask;
			IEnumerable<EquipmentTypeDataModel?>? equpTypes = await equpTypeTask;
			IEnumerable<TypeBreakdownDataModel?>? breaks = await typeBreakTask;
			IEnumerable<ImportancesDataModel?>? importances = await importTask;

			var statusList = _mapper.Map<IEnumerable<ApplicationStatus>>(statuses);
			var appTypeList = _mapper.Map<IEnumerable<ApplicationType>>(appTypes);
			var kindsList = _mapper.Map<IEnumerable<EquipmentKind>>(kinds);
			var modelsList = _mapper.Map<IEnumerable<EquipmentModel>>(models);
			var equpTypesList = _mapper.Map<IEnumerable<EquipmentType>>(equpTypes);
			var breaksList = _mapper.Map<IEnumerable<TypeBreakdown>>(breaks);
			var importancesList = _mapper.Map<IEnumerable<Importance>>(importances);
			return (statusList, appTypeList, kindsList, modelsList, equpTypesList, breaksList, importancesList);
		}


		/// <summary>
		/// Получение справочных данных для инициализации формы Активные заявки, в виде коллекций IEnumerable.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<(IEnumerable<ApplicationStatus> statusesList,
			IEnumerable<Importance?> importancesList,
			IEnumerable<ApplicationType?> appTypesList)>
			GetRefDataForInitAsync(CancellationToken token = default) =>

			// Используем кэш для хранения данных, чтобы избежать повторных запросов к API
			await _cache.GetOrCreateAsync("RefDataForInit", async _ =>
			{
				_.AbsoluteExpirationRelativeToNow = _cacheDuration; // Устанавливаем время жизни кэша
				return await LoadRefDataForInitFromApiAsync(token); // Загружаем данные из API, если их нет в кэше или они устарели
			});


		/// <summary>
		/// Загрузка справочных данных для инициализации формы Активные заявки из API.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		private async Task<(
			IEnumerable<ApplicationStatus> statusesList,
			IEnumerable<Importance> importancesList,
			IEnumerable<ApplicationType> appTypesList)>
			LoadRefDataForInitFromApiAsync(CancellationToken token)
		{
			var statusesTask = _referenceDataApi.GetAllApplicationStatusAsync(token);
			var importancesTask = _referenceDataApi.GetAllImportancesAsync(token);
			var appTypesTask = _referenceDataApi.GetAllApplicationTypeAsync(token);

			await Task.WhenAll(statusesTask, importancesTask, appTypesTask);

			IEnumerable<ApplicationStatusDataModel?>? statuses = await statusesTask;
			IEnumerable<ImportancesDataModel?>? importances = await importancesTask;
			IEnumerable<ApplicationTypeDataModel?>? appTypes = await appTypesTask;

			var statusesList = _mapper.Map<IEnumerable<ApplicationStatus>>(statuses);
			var importancesList = _mapper.Map<IEnumerable<Importance>>(importances);
			var appTypesList = _mapper.Map<IEnumerable<ApplicationType>>(appTypes);

			return (statusesList, importancesList, appTypesList);
		}
	}
}
