using AutoMapper;

using System.Collections.ObjectModel;

using WorkOrderX.ApiClients.ReferenceData.Interfaces;
using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.Services.Implementation
{
	public class ReferenceDadaServices : ViewModelBase, IReferenceDadaServices
	{
		private readonly IReferenceDataApiService _referenceDataApi;
		private readonly IMapper _mapper;

		public ReferenceDadaServices(IReferenceDataApiService referenceDataApi, IMapper mapper)
		{
			_referenceDataApi = referenceDataApi;
			_mapper = mapper;
		}

		public async Task<(ObservableCollection<ApplicationStatus>? Statuses,
			ObservableCollection<ApplicationType>? AppTypes,
			ObservableCollection<EquipmentKind>? EqupKinds,
			ObservableCollection<EquipmentModel>? EqupModels,
			ObservableCollection<EquipmentType>? EqupTypes,
			ObservableCollection<TypeBreakdown>? Breaks,
			ObservableCollection<Importance>? Importances
			)> GetAllRefenceDataAsync(CancellationToken token = default)
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


			ObservableCollection<ApplicationStatus>? statusesObserbal = new ObservableCollection<ApplicationStatus>(statusList);
			ObservableCollection<ApplicationType>? appTypeObserbal = new ObservableCollection<ApplicationType>(appTypeList);
			ObservableCollection<EquipmentKind>? kindsObserbal = new ObservableCollection<EquipmentKind>(kindsList);
			ObservableCollection<EquipmentModel>? modelsObserbal = new ObservableCollection<EquipmentModel>(modelsList);
			ObservableCollection<EquipmentType>? equpTypesObserbal = new ObservableCollection<EquipmentType>(equpTypesList);
			ObservableCollection<TypeBreakdown>? breaksObserbal = new ObservableCollection<TypeBreakdown>(breaksList);
			ObservableCollection<Importance>? importancesObserbal = new ObservableCollection<Importance>(importancesList);

			return (statusesObserbal, appTypeObserbal, kindsObserbal, modelsObserbal, equpTypesObserbal, breaksObserbal, importancesObserbal);
		}

		public async Task<IEnumerable<ApplicationStatus>> GetApplicationStatusesAsync(CancellationToken token = default)
		{
			var statuses = await _referenceDataApi.GetAllApplicationStatusAsync(token);
			return _mapper.Map<IEnumerable<ApplicationStatus>>(statuses);
		}

		public async Task<IEnumerable<Importance>> GetImportancesAsync(CancellationToken token = default)
		{
			var importances = await _referenceDataApi.GetAllImportancesAsync(token);
			return _mapper.Map<IEnumerable<Importance>>(importances);
		}

		public async Task<IEnumerable<ApplicationType>> GetApplicationTypesAsync(CancellationToken token = default)
		{
			var appTypes = await _referenceDataApi.GetAllApplicationTypeAsync(token);
			return _mapper.Map<IEnumerable<ApplicationType>>(appTypes);
		}
	}
}
