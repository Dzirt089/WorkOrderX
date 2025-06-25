using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Тип заявки
	/// </summary>
	public class ApplicationType : Enumeration
	{
		/// <summary>
		/// Хоз. работы
		/// </summary>
		public static ApplicationType HouseholdChores = new(1, nameof(HouseholdChores));

		/// <summary>
		/// Ремонт оборудования
		/// </summary>
		public static ApplicationType EquipmentRepair = new(2, nameof(EquipmentRepair));

		public ApplicationType(int id, string name) : base(id, name) { }

		/// <summary>
		/// Преобразование строки в тип пресета
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="DomainException"></exception>
		public static ApplicationType Parse(string name) => name?.ToLower() switch
		{
			"householdchores" => HouseholdChores,
			"equipmentrepair" => EquipmentRepair,
			_ => throw new DomainException("Unknown application type name")
		};
	}
}
