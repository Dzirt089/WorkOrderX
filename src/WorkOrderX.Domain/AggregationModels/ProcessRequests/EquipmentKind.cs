using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Вид оборудования
	/// </summary>
	public class EquipmentKind : Enumeration
	{

		//TODO: Пришлют список - заполнить.
		public EquipmentKind(int id, string name) : base(id, name)
		{
		}
	}
}
