using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.ViewModel
{
	public partial class SelectRequestChoreViewModel : BaseSelectRequestViewModel
	{
		private readonly GlobalEmployeeForApp _globalEmployee;
		private readonly IReferenceDadaServices _referenceDada;
		private readonly IProcessRequestApiService _processRequestApi;
		private readonly IEmployeeService _employeeService;

		public SelectRequestChoreViewModel(
			GlobalEmployeeForApp globalEmployee,
			IReferenceDadaServices referenceDada,
			IProcessRequestApiService processRequestApi,
			IEmployeeService employeeService)
			:
			base(
				globalEmployee,
				referenceDada,
				processRequestApi,
				employeeService)
		{
			_globalEmployee = globalEmployee;
			_referenceDada = referenceDada;
			_processRequestApi = processRequestApi;
			_employeeService = employeeService;
		}

		#region Methods

		/// <summary>
		/// Метод для обновления данных формы заявки на ремонт с учетом выбранной заявки.
		/// </summary>
		public override async Task UpdateReferenceDataInForm()
		{
			// Получаем данные для заполнения ComboBox'ов на форме заявки
			(IEnumerable<ApplicationStatus?> statuses,
				IEnumerable<ApplicationType?> appTypes,
				_, _, _, _,
				IEnumerable<Importance?> importances) = await _referenceDada.GetAllReferenceDataAsync();


			// Полная версия списков  					
			Statuses = new ObservableCollection<ApplicationStatus>(statuses); // Статусов
			ApplicationTypes = new ObservableCollection<ApplicationType>(appTypes); // Тип заявки (Здесь: "Ремонт оборудования")	
			Importances = new ObservableCollection<Importance>(importances); // Важность заявки

			// Инициализация списков справочных данных в словарях			
			var importancesDict = importances.ToDictionary(_ => _.Name);
			var statusesDict = statuses.ToDictionary(_ => _.Name);

			ItemImport = importancesDict[SelectProcessRequest.Importance];
			ItemApplicationStatus = statusesDict[SelectProcessRequest.ApplicationStatus];

			TextNumberRequest = $"Заявка №{SelectProcessRequest.ApplicationNumber}";
		}

		#endregion

		#region Commands

		/// <summary>
		/// Метод для сохранения изменений в заявке на ремонт, если сотрудник является заказчиком заявки.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task SaveRequest()
		{
			ArgumentNullException.ThrowIfNull(SelectProcessRequest.Id);

			SelectProcessRequest.Importance = ItemImport?.Name ?? throw new ArgumentException($"Занчение {nameof(ItemImport.Name)} равно null");

			SelectProcessRequest.ApplicationStatus = "Changed";

			UpdateProcessRequestModel updateProcess = new UpdateProcessRequestModel
			{
				Id = (Guid)SelectProcessRequest.Id!,
				ApplicationType = SelectProcessRequest.ApplicationType,
				ApplicationStatus = SelectProcessRequest.ApplicationStatus,
				ApplicationNumber = SelectProcessRequest.ApplicationNumber,
				CreatedAt = SelectProcessRequest.CreatedAt,
				PlannedAt = SelectProcessRequest.PlannedAt,
				EquipmentKind = SelectProcessRequest.EquipmentKind,
				EquipmentModel = SelectProcessRequest.EquipmentModel,
				EquipmentType = SelectProcessRequest.EquipmentType,
				SerialNumber = SelectProcessRequest.SerialNumber,
				Importance = SelectProcessRequest.Importance,
				Location = SelectProcessRequest.Location,
				InternalComment = SelectProcessRequest.InternalComment,
				TypeBreakdown = SelectProcessRequest.TypeBreakdown,
				DescriptionMalfunction = SelectProcessRequest.DescriptionMalfunction,
				CustomerEmployeeId = SelectProcessRequest.CustomerEmployee.Id,
			};

			await _processRequestApi.UpdateProcessRequestAsync(updateProcess);

			IsCustomerUpdate = false;

			await FinalMethodAsync();

			CloseAction?.Invoke();
		}

		#endregion


	}
}
