using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Тип инструмента
	/// </summary>
	public class InstrumentType : Enumeration
	{
		/// <summary>
		/// Тип инструмента - Электрический инструмент
		/// </summary>
		public readonly static InstrumentType ElectricInstrument = new(1, nameof(ElectricInstrument), "Электрический инструмент");

		/// <summary>
		/// Тип инструмента - Ударный инструмент
		/// </summary>
		public readonly static InstrumentType PercussiveInstrument = new(2, nameof(PercussiveInstrument), "Ударный инструмент");

		/// <summary>
		/// Тип инструмента - Зажимной инструмент
		/// </summary>
		public readonly static InstrumentType ClampInstrument = new(3, nameof(ClampInstrument), "Зажимной инструмент");

		/// <summary>
		/// Тип инструмента - Измерительный инструмент
		/// </summary>
		public readonly static InstrumentType MeasuringInstrument = new(4, nameof(MeasuringInstrument), "Измерительный инструмент");

		/// <summary>
		/// Тип инструмента - Крепежный инструмент
		/// </summary>
		public readonly static InstrumentType FastenindInstrument = new(5, nameof(FastenindInstrument), "Крепежный инструмент");

		/// <summary>
		/// Тип инструмента - Сверлильный инструмент
		/// </summary>
		public readonly static InstrumentType DrillingInstrument = new(6, nameof(DrillingInstrument), "Сверлильный инструмент");

		/// <summary>
		/// Тип инструмента - Слесарный инструмент
		/// </summary>
		public readonly static InstrumentType LocksmithInstrument = new(7, nameof(LocksmithInstrument), "Слесарный инструмент");

		/// <summary>
		/// Тип инструмента - Пневматический инструмент
		/// </summary>
		public readonly static InstrumentType PneumaticInstrument = new(8, nameof(PneumaticInstrument), "Пневматический инструмент");

		/// <summary>
		/// Тип инструмента не указан\ отсутствует в списке
		/// </summary>
		public readonly static InstrumentType Other = new(9, nameof(Other), "Другое");

		// Приватный конструктор без параметров для EF
		private InstrumentType() { }

		public InstrumentType(int id, string name, string descriptions) : base(id, name, descriptions) { }


	}
}
