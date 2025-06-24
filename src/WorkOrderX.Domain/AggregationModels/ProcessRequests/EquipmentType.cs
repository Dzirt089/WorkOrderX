using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Тип оборудования
	/// </summary>
	public class EquipmentType : Enumeration
	{

		//TODO: Пришлют список - заполнить.
		public EquipmentType(int id, string name) : base(id, name)
		{
		}
	}
}
