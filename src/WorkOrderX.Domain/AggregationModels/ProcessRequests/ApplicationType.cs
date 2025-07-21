using WorkOrderX.Domain.Root;

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
		public readonly static ApplicationType HouseholdChores = new(1, nameof(HouseholdChores), "Хоз. работы");

		/// <summary>
		/// Ремонт оборудования
		/// </summary>
		public readonly static ApplicationType EquipmentRepair = new(2, nameof(EquipmentRepair), "Ремонт оборудования");

		// Приватный конструктор без параметров для EF
		private ApplicationType() { }

		public ApplicationType(int id, string name, string descriptions) : base(id, name, descriptions) { }
	}
}
