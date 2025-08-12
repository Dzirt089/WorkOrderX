using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Вид инструмента
	/// </summary>
	public class InstrumentKind : Enumeration
	{
		#region Электро

		/// <summary>
		/// Вид инструмента - Электроножницы
		/// </summary>
		public readonly static InstrumentKind ElectricKnives = new(1, nameof(ElectricKnives), InstrumentType.ElectricInstrument, "Электроножницы");

		/// <summary>
		/// Вид инструмента - Лобзик
		/// </summary>
		public readonly static InstrumentKind Jigsaw = new(2, nameof(Jigsaw), InstrumentType.ElectricInstrument, "Лобзик");

		/// <summary>
		/// Вид инструмента - Дрель
		/// </summary>
		public readonly static InstrumentKind Drill = new(3, nameof(Drill), InstrumentType.ElectricInstrument, "Дрель");

		/// <summary>
		/// Вид инструмента - Шуруповерт
		/// </summary>
		public readonly static InstrumentKind Screwdriver = new(4, nameof(Screwdriver), InstrumentType.ElectricInstrument, "Шуруповерт");

		/// <summary>
		/// Вид инструмента - Болгарка 
		/// </summary>
		public readonly static InstrumentKind Grinder = new(5, nameof(Grinder), InstrumentType.ElectricInstrument, "Болгарка");

		/// <summary>
		/// Вид инструмента - Эксцентриковая шлифмашина
		/// </summary>
		public readonly static InstrumentKind EccentricGrinder = new(6, nameof(EccentricGrinder), InstrumentType.ElectricInstrument, "Эксцентриковая шлифмашина");
		#endregion

		#region Ударный

		/// <summary>
		/// Вид инструмента - Слесарный
		/// </summary>
		public readonly static InstrumentKind Locksmith = new(7, nameof(Locksmith), InstrumentType.PercussiveInstrument, "Слесарный");

		/// <summary>
		/// Вид инструмента - Киянка
		/// </summary>
		public readonly static InstrumentKind Mallet = new(8, nameof(Mallet), InstrumentType.PercussiveInstrument, "Киянка");

		/// <summary>
		/// Вид инструмента - Кувалда
		/// </summary>
		public readonly static InstrumentKind Sledgehammer = new(9, nameof(Sledgehammer), InstrumentType.PercussiveInstrument, "Кувалда");
		#endregion

		#region Зажимной

		/// <summary>
		/// Вид инструмента - Пассатижи
		/// </summary>
		public readonly static InstrumentKind Pliers = new(10, nameof(Pliers), InstrumentType.ClampInstrument, "Пассатижи");

		/// <summary>
		/// Вид инструмента - Круглогубцы
		/// </summary>
		public readonly static InstrumentKind RoundPliers = new(11, nameof(RoundPliers), InstrumentType.ClampInstrument, "Круглогубцы");

		/// <summary>
		/// Вид инструмента - Клещи
		/// </summary>
		public readonly static InstrumentKind Pincers = new(12, nameof(Pincers), InstrumentType.ClampInstrument, "Клещи");

		/// <summary>
		/// Вид инструмента - Тиски Настольные
		/// </summary>
		public readonly static InstrumentKind TableVise = new(13, nameof(TableVise), InstrumentType.ClampInstrument, "Тиски Настольные");

		/// <summary>
		/// Вид инструмента - Тиски Стуловые
		/// </summary>
		public readonly static InstrumentKind ChairVise = new(14, nameof(ChairVise), InstrumentType.ClampInstrument, "Тиски Стуловые");
		#endregion

		#region Измерительный

		/// <summary>
		/// Вид инструмента - Линейка Металлическая
		/// </summary>
		public readonly static InstrumentKind MetalRuler = new(15, nameof(MetalRuler), InstrumentType.MeasuringInstrument, "Линейка Металлическая");

		/// <summary>
		/// Вид инструмента - Складной метр
		/// </summary>
		public readonly static InstrumentKind FoldingMeter = new(16, nameof(FoldingMeter), InstrumentType.MeasuringInstrument, "Складной метр");
		#endregion

		#region Крепёжный

		/// <summary>
		/// Вид инструмента - Отвертка Плоская
		/// </summary>
		public readonly static InstrumentKind ScrewdriverFlat = new(17, nameof(ScrewdriverFlat), InstrumentType.FastenindInstrument, "Отвертка Плоская");

		/// <summary>
		/// Вид инструмента - Отвертка крестовая
		/// </summary>
		public readonly static InstrumentKind PhillipsScrewdriver = new(18, nameof(PhillipsScrewdriver), InstrumentType.FastenindInstrument, "Отвертка крестовая");

		/// <summary>
		/// Вид инструмента - Отвертка шестигранная
		/// </summary>
		public readonly static InstrumentKind HexScrewdriver = new(19, nameof(HexScrewdriver), InstrumentType.FastenindInstrument, "Отвертка шестигранная");

		/// <summary>
		/// Вид инструмента - Ключ Накидной
		/// </summary>
		public readonly static InstrumentKind HingedKey = new(20, nameof(HingedKey), InstrumentType.FastenindInstrument, "Ключ Накидной");

		/// <summary>
		/// Вид инструмента - Ключ рожковый
		/// </summary>
		public readonly static InstrumentKind HornKey = new(21, nameof(HornKey), InstrumentType.FastenindInstrument, "Ключ рожковый");

		/// <summary>
		/// Вид инструмента - Ключ разводной
		/// </summary>
		public readonly static InstrumentKind AdjustableKey = new(22, nameof(AdjustableKey), InstrumentType.FastenindInstrument, "Ключ разводной");

		/// <summary>
		/// Вид инструмента - Ключ газовый
		/// </summary>
		public readonly static InstrumentKind GasKey = new(23, nameof(GasKey), InstrumentType.FastenindInstrument, "Ключ газовый");
		#endregion

		#region Сверлильный

		/// <summary>
		/// Вид инструмента - Сверло по металлу
		/// </summary>
		public readonly static InstrumentKind MetalDrillBit = new(24, nameof(MetalDrillBit), InstrumentType.DrillingInstrument, "Сверло по металлу");

		/// <summary>
		/// Вид инструмента - Сверло по дереву
		/// </summary>
		public readonly static InstrumentKind WoodDrillBit = new(25, nameof(WoodDrillBit), InstrumentType.DrillingInstrument, "Сверло по дереву");

		/// <summary>
		/// Вид инструмента - Сверло по бетону
		/// </summary>
		public readonly static InstrumentKind ConcreteDrillBit = new(26, nameof(ConcreteDrillBit), InstrumentType.DrillingInstrument, "Сверло по бетону");
		#endregion

		#region Слесарный

		/// <summary>
		/// Вид инструмента - Ножницы по металлу
		/// </summary>
		public readonly static InstrumentKind MetalShears = new(27, nameof(MetalShears), InstrumentType.LocksmithInstrument, "Ножницы по металлу");

		/// <summary>
		/// Вид инструмента - Ножовки
		/// </summary>
		public readonly static InstrumentKind Hacksaws = new(28, nameof(Hacksaws), InstrumentType.LocksmithInstrument, "Ножовки");

		#endregion

		#region Пневматический

		/// <summary>
		/// Вид инструмента - Клепальник
		/// </summary>
		public readonly static InstrumentKind Riveter = new(29, nameof(Riveter), InstrumentType.PneumaticInstrument, "Клепальник");

		/// <summary>
		/// Вид инструмента - Бонговщик
		/// </summary>
		public readonly static InstrumentKind BongPlayer = new(30, nameof(BongPlayer), InstrumentType.PneumaticInstrument, "Бонговщик");

		/// <summary>
		/// Вид инструмента - Пневмошуруповерт
		/// </summary>
		public readonly static InstrumentKind PneumaticScrewdriver = new(31, nameof(PneumaticScrewdriver), InstrumentType.PneumaticInstrument, "Пневмошуруповерт");

		#endregion

		/// <summary>
		/// Вид инструмента неизвестен\не указан в списке
		/// </summary>
		public readonly static InstrumentKind Other = new(32, nameof(Other), InstrumentType.Other, "Другое");

		/// <summary>
		/// Дополнительный набор вида инструмента у электро инструмента
		/// </summary>
		public readonly static InstrumentKind ElectroOther = new(33, nameof(ElectroOther), InstrumentType.ElectricInstrument, "Другое");



		// Приватный конструктор без параметров для EF
		private InstrumentKind() { }

		public InstrumentKind(int id, string name, InstrumentType type, string descriptions) : base(id, name, descriptions)
		{
			InstrumentType = type;
			InstrumentTypeId = type.Id;
		}


		/// <summary>
		/// Тип инструмента
		/// </summary>
		public InstrumentType InstrumentType { get; private set; }

		/// <summary>
		/// Внешний ключ <see cref="InstrumentType"/>
		/// </summary>
		public int InstrumentTypeId { get; private set; }

		public void SetInstrumentType(InstrumentType trackedType)
		{
			InstrumentType = trackedType;
			InstrumentTypeId = trackedType.Id;
		}
	}
}
