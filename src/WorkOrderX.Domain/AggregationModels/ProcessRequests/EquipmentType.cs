using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Тип оборудования
	/// </summary>
	public class EquipmentType : Enumeration
	{
		/// <summary>
		/// Тип оборудования электрический инструмент
		/// </summary>
		public static EquipmentType ElectricInstrument = new(1, nameof(ElectricInstrument));

		//TODO: Пришлют список - заполнить.
		public EquipmentType(int id, string name) : base(id, name)
		{
		}

		public static EquipmentType Parse(string name) => name?.ToLower() switch
		{
			"electricinstrument" => ElectricInstrument,
			_ => throw new EnumerationValueNotFoundException($"Unknown equipment type name {nameof(name)}"),
		};
	}
}
