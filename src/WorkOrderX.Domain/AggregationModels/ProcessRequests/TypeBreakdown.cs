using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Тип поломки
	/// </summary>
	public class TypeBreakdown : Enumeration
	{

		//TODO: Пришлют список - заполнить.
		public TypeBreakdown(int id, string name) : base(id, name)
		{
		}
	}
}
