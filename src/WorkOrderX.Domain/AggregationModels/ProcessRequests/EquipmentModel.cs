using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Модель оборудования
	/// </summary>
	public class EquipmentModel : Enumeration
	{
		/// <summary>
		/// Модель оборудования неизвестна
		/// </summary>
		public static readonly EquipmentModel Unknown = new EquipmentModel(1, nameof(Unknown));

		//TODO: Пришлют список - заполнить.
		public EquipmentModel(int id, string name) : base(id, name)
		{
		}
	}
}
