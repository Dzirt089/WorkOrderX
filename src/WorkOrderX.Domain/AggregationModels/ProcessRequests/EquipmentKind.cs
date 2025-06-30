using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Вид оборудования
	/// </summary>
	public class EquipmentKind : Enumeration
	{
		/// <summary>
		/// Тип оборудования неопределенный
		/// </summary>
		public static EquipmentKind Undefined = new(1, nameof(Undefined));


		//TODO: Пришлют список - заполнить.
		public EquipmentKind(int id, string name) : base(id, name) { }

		public static EquipmentKind Parse(string name) => name?.ToLower() switch
		{
			"undefined" => Undefined,
			_ => throw new EnumerationValueNotFoundException($"Unknown equipment type name {nameof(name)}"),
		};
	}
}
