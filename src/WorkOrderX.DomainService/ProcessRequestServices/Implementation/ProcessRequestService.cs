using WorkOrderX.Domain.AggregationModels.Employees;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.Root.Exceptions;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;

namespace WorkOrderX.DomainService.ProcessRequestServices.Implementation
{
	public class ProcessRequestService(IEmployeeRepository employeeRepository) : IProcessRequestService
	{
		private readonly IEmployeeRepository _employeeRepository = employeeRepository;

		/// <summary>
		/// Создает заявку на ремонт оборудования или хоз. работы в зависимости от типа заявки.
		/// </summary>
		/// <param name="applicationNumber">Номер заявки</param>
		/// <param name="applicationType">Тип заявки</param>
		/// <param name="createdAt">Даиа создания заявки</param>
		/// <param name="plannedAt">Плановая дата завершения заявки</param>
		/// <param name="equipmentType">Тип оборудования</param>
		/// <param name="equipmentKind">Вид оборудования</param>
		/// <param name="equipmentModel">Модель оборудования</param>
		/// <param name="serialNumber">Серийный номер оборудования</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="descriptionMalfunction">Описание неисправности</param>
		/// <param name="applicationStatus">Статус заявки</param>
		/// <param name="internalComment">Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainException"></exception>
		public async Task<ProcessRequest> CreateProcessRequest(
			long applicationNumber,
			string applicationType,
			DateTime createdAt,
			DateTime plannedAt,
			string? equipmentType,
			string? equipmentKind,
			string? equipmentModel,
			string? serialNumber,
			string typeBreakdown,
			string descriptionMalfunction,
			string applicationStatus,
			string? internalComment,
			Guid customerEmployeeId,
			CancellationToken token)
		{

			var typeApplication = ApplicationType.Parse(applicationType);

			(TypeBreakdown breakdown, Guid? executorEmployeeId) = await GetBreakdownAndExecutorAsync(typeApplication, typeBreakdown, customerEmployeeId, token);

			// Создаем новую заявку
			ProcessRequest? processRequest = ProcessRequest.Create(
				applicationNumber: ApplicationNumber.Create(applicationNumber),
				applicationType: typeApplication,
				createdAt: createdAt,
				plannedAt: plannedAt,
				equipmentType: equipmentType is null ? null : EquipmentType.Parse(equipmentType),
				equipmentKind: equipmentKind is null ? null : EquipmentKind.Parse(equipmentKind),
				equipmentModel: equipmentModel is null ? null : EquipmentModel.Parse(equipmentModel),
				serialNumber: serialNumber,
				typeBreakdown: breakdown,
				descriptionMalfunction: DescriptionMalfunction.Create(descriptionMalfunction),
				applicationStatus: ApplicationStatus.Parse(applicationStatus),
				internalComment: internalComment is null ? null : InternalComment.Create(internalComment),
				customerEmployeeId: customerEmployeeId,
				executorEmployeeId: executorEmployeeId);

			return processRequest;
		}


		/// <summary>
		/// Проверяет, что заказчик существует и может быть заказчиком.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="DomainException"></exception>
		private async Task ValidateCustomerAsync(Guid customerEmployeeId, CancellationToken token)
		{
			// Проверяем, что customerEmployeeId существует и может являться заказчиком
			var employeeCustomer = await _employeeRepository.GetByIdAsync(customerEmployeeId, token)
				?? throw new DomainException($"Employee with ID {customerEmployeeId} not found.");

			// Проверяем, что сотрудник имеет возможность создать заявку имея роль: заказчик, исполнитель или администратор.
			if (employeeCustomer.Role != Role.Customer && employeeCustomer.Role != Role.Contractor && employeeCustomer.Role != Role.Admin)
				throw new DomainException($"Employee with ID {customerEmployeeId} is not a customer.");
		}

		/// <summary>
		/// По типу поломки и по типу заявки находит исполнителя
		/// </summary>
		/// <exception cref="DomainException"></exception>
		private async Task<Guid?> GetEmployeeSpecializedAsync(TypeBreakdown type, ApplicationType applicationType, CancellationToken token)
		{
			if (type == TypeBreakdown.Electrical && applicationType == ApplicationType.EquipmentRepair)
				return await GetIdEmployeeBySpecializedAsync(Specialized.Electrician, token);

			if (type == TypeBreakdown.Mechanical && applicationType == ApplicationType.EquipmentRepair)
				return await GetIdEmployeeBySpecializedAsync(Specialized.Mechanic, token);

			if (type == TypeBreakdown.РouseholdСhores && applicationType == ApplicationType.HouseholdChores)
				return await GetIdEmployeeBySpecializedAsync(Specialized.Plumber, token);

			throw new DomainException("Not type of breakdown.");
		}

		/// <summary>
		/// Получает ID исполнителя по типу его специальности.
		/// </summary>
		/// <param name="specialized"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="DomainException"></exception>
		private async Task<Guid> GetIdEmployeeBySpecializedAsync(Specialized specialized, CancellationToken token)
		{
			var result = await _employeeRepository.GetBySpecializedAsync(specialized, token)
											?? throw new DomainException("No electrician available for this request.");
			if (result.Role != Role.Contractor)
				throw new DomainException($"Employee with ID {result.Id} is not an executor.");

			return result.Id;
		}

		/// <summary>
		/// Проверяет заказчика и получает тип поломки и исполнителя.
		/// </summary>
		/// <param name="applicationType">Номер заявки</param>
		/// <param name="typeBreakdown">Тип поломки</param>
		/// <param name="customerEmployeeId">ID заказчика заявки</param>
		/// <param name="token"></param>
		/// <returns></returns>
		private async Task<(TypeBreakdown breakdown, Guid? executorEmployeeId)> GetBreakdownAndExecutorAsync(
			ApplicationType applicationType,
			string typeBreakdown,
			Guid customerEmployeeId,
			CancellationToken token)
		{
			// Проверяем, что заказчик существует и может быть заказчиком
			await ValidateCustomerAsync(customerEmployeeId, token);

			// Парсим тип поломки
			var breakdown = TypeBreakdown.Parse(typeBreakdown);

			// Находим исполнителя по типу поломки
			Guid? executorEmployeeId = await GetEmployeeSpecializedAsync(breakdown, applicationType, token) ?? null;

			return (breakdown, executorEmployeeId);
		}
	}
}
