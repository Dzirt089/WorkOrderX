using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Модель оборудования
	/// </summary>
	public class EquipmentModel : Enumeration
	{

		//TODO: Пришлют список - заполнить.
		public EquipmentModel(int id, string name) : base(id, name)
		{
		}
	}
}
