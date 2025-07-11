using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Модель оборудования
	/// </summary>
	public class EquipmentModel : Enumeration
	{
		/// <summary>
		/// Модель оборудования неизвестна\не указана в списке
		/// </summary>
		public readonly static EquipmentModel Other = new EquipmentModel(1, nameof(Other), "Другое");

		public string Descriptions { get; }

		// Приватный конструктор без параметров для EF
		private EquipmentModel() { }

		//TODO: Пришлют список - заполнить.
		public EquipmentModel(int id, string name, string descriptions) : base(id, name)
		{
			Descriptions = descriptions;
		}
	}
}
