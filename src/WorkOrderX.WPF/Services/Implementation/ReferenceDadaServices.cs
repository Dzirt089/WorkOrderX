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
			ObservableCollection<TypeBreakdown>? Breaks
			)> GetAllRefenceDataAsync(CancellationToken token = default)
		{
			var statusTask = _referenceDataApi.GetAllApplicationStatusAsync(token);
			var appTypeTask = _referenceDataApi.GetAllApplicationTypeAsync(token);
			var kindsTask = _referenceDataApi.GetAllEquipmentKindAsync(token);
			var modelsTask = _referenceDataApi.GetAllEquipmentModelAsync(token);
			var equpTypeTask = _referenceDataApi.GetAllEquipmentTypeAsync(token);
			var typeBreakTask = _referenceDataApi.GetAllTypeBreakdownAsync(token);

			await Task.WhenAll(statusTask, appTypeTask, kindsTask, modelsTask, equpTypeTask, typeBreakTask);

			IEnumerable<ApplicationStatusDataModel?>? statuses = await statusTask;
			IEnumerable<ApplicationTypeDataModel?>? appTypes = await appTypeTask;
			IEnumerable<EquipmentKindDataModel?>? kinds = await kindsTask;
			IEnumerable<EquipmentModelDataModel?>? models = await modelsTask;
			IEnumerable<EquipmentTypeDataModel?>? equpTypes = await equpTypeTask;
			IEnumerable<TypeBreakdownDataModel?>? breaks = await typeBreakTask;


			var statusList = _mapper.Map<IEnumerable<ApplicationStatus>>(statuses);
			var appTypeList = _mapper.Map<IEnumerable<ApplicationType>>(appTypes);
			var kindsList = _mapper.Map<IEnumerable<EquipmentKind>>(kinds);
			var modelsList = _mapper.Map<IEnumerable<EquipmentModel>>(models);
			var equpTypesList = _mapper.Map<IEnumerable<EquipmentType>>(equpTypes);
			var breaksList = _mapper.Map<IEnumerable<TypeBreakdown>>(breaks);


			ObservableCollection<ApplicationStatus>? statusesObserbal = new ObservableCollection<ApplicationStatus>(statusList);
			ObservableCollection<ApplicationType>? appTypeObserbal = new ObservableCollection<ApplicationType>(appTypeList);
			ObservableCollection<EquipmentKind>? kindsObserbal = new ObservableCollection<EquipmentKind>(kindsList);
			ObservableCollection<EquipmentModel>? modelsObserbal = new ObservableCollection<EquipmentModel>(modelsList);
			ObservableCollection<EquipmentType>? equpTypesObserbal = new ObservableCollection<EquipmentType>(equpTypesList);
			ObservableCollection<TypeBreakdown>? breaksObserbal = new ObservableCollection<TypeBreakdown>(breaksList);

			return (statusesObserbal, appTypeObserbal, kindsObserbal, modelsObserbal, equpTypesObserbal, breaksObserbal);
		}

	}
}
