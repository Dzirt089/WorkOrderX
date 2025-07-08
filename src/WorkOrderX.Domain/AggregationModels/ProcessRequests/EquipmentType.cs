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
		/// Тип оборудования - Электрический инструмент
		/// </summary>
		public static EquipmentType ElectricInstrument = new(1, nameof(ElectricInstrument));

		/// <summary>
		/// Тип оборудования - Ударный инструмент
		/// </summary>
		public static EquipmentType PercussiveInstrument = new(2, nameof(PercussiveInstrument));

		/// <summary>
		/// Тип оборудования - Зажимной инструмент
		/// </summary>
		public static EquipmentType ClampInstrument = new(3, nameof(ClampInstrument));

		/// <summary>
		/// Тип оборудования - Измерительный инструмент
		/// </summary>
		public static EquipmentType MeasuringInstrument = new(4, nameof(MeasuringInstrument));

		/// <summary>
		/// Тип оборудования - Крепежный инструмент
		/// </summary>
		public static EquipmentType FastenindInstrument = new(5, nameof(FastenindInstrument));

		/// <summary>
		/// Тип оборудования - Сверлильный инструмент
		/// </summary>
		public static EquipmentType DrillingInstrument = new(6, nameof(DrillingInstrument));

		/// <summary>
		/// Тип оборудования - Слесарный инструмент
		/// </summary>
		public static EquipmentType LocksmithInstrument = new(7, nameof(LocksmithInstrument));

		/// <summary>
		/// Тип оборудования - Пневматический инструмент
		/// </summary>
		public static EquipmentType PneumaticInstrument = new(8, nameof(PneumaticInstrument));

		/// <summary>
		/// Тип оборудования - не предусмотрен (например, для хоз. работ)
		/// </summary>
		public static EquipmentType None = new(9, nameof(None));

		public EquipmentType(int id, string name) : base(id, name)
		{
		}

		public static EquipmentType Parse(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new EnumerationValueNotFoundException("Name is null or empty");

			var match = GetAll<EquipmentType>()
				.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))
				?? throw new EnumerationValueNotFoundException($"Unknown equipment type name '{name}'");

			return match;
		}
	}
}
