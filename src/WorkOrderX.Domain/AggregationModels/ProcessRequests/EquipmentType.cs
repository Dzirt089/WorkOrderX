using WorkOrderX.Domain.Root;

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
		public static EquipmentType ElectricInstrument = new(1, nameof(ElectricInstrument), "Электрический инструмент");

		/// <summary>
		/// Тип оборудования - Ударный инструмент
		/// </summary>
		public static EquipmentType PercussiveInstrument = new(2, nameof(PercussiveInstrument), "Ударный инструмент");

		/// <summary>
		/// Тип оборудования - Зажимной инструмент
		/// </summary>
		public static EquipmentType ClampInstrument = new(3, nameof(ClampInstrument), "Зажимной инструмент");

		/// <summary>
		/// Тип оборудования - Измерительный инструмент
		/// </summary>
		public static EquipmentType MeasuringInstrument = new(4, nameof(MeasuringInstrument), "Измерительный инструмент");

		/// <summary>
		/// Тип оборудования - Крепежный инструмент
		/// </summary>
		public static EquipmentType FastenindInstrument = new(5, nameof(FastenindInstrument), "Крепежный инструмент");

		/// <summary>
		/// Тип оборудования - Сверлильный инструмент
		/// </summary>
		public static EquipmentType DrillingInstrument = new(6, nameof(DrillingInstrument), "Сверлильный инструмент");

		/// <summary>
		/// Тип оборудования - Слесарный инструмент
		/// </summary>
		public static EquipmentType LocksmithInstrument = new(7, nameof(LocksmithInstrument), "Слесарный инструмент");

		/// <summary>
		/// Тип оборудования - Пневматический инструмент
		/// </summary>
		public static EquipmentType PneumaticInstrument = new(8, nameof(PneumaticInstrument), "Пневматический инструмент");

		/// <summary>
		/// Тип оборудования - не предусмотрен (например, для хоз. работ)
		/// </summary>
		public static EquipmentType None = new(9, nameof(None), "");

		public EquipmentType(int id, string name, string descriptions) : base(id, name)
		{
			Descriptions = descriptions;
		}


		public string Descriptions { get; }
	}
}
