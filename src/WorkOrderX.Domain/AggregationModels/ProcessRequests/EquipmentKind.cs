using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Вид оборудования
	/// </summary>
	public class EquipmentKind : Enumeration
	{
		#region Электро

		/// <summary>
		/// Вид оборудования - Электроножницы
		/// </summary>
		public static EquipmentKind ElectricKnives = new(1, nameof(ElectricKnives), EquipmentType.ElectricInstrument, "Электроножницы");

		/// <summary>
		/// Вид оборудования - Лобзик
		/// </summary>
		public static EquipmentKind Jigsaw = new(2, nameof(Jigsaw), EquipmentType.ElectricInstrument, "Лобзик");

		/// <summary>
		/// Вид оборудования - Дрель
		/// </summary>
		public static EquipmentKind Drill = new(3, nameof(Drill), EquipmentType.ElectricInstrument, "Дрель");

		/// <summary>
		/// Вид оборудования - Шуруповерт
		/// </summary>
		public static EquipmentKind Screwdriver = new(4, nameof(Screwdriver), EquipmentType.ElectricInstrument, "Шуруповерт");

		/// <summary>
		/// Вид оборудования - Болгарка 
		/// </summary>
		public static EquipmentKind Grinder = new(5, nameof(Grinder), EquipmentType.ElectricInstrument, "Болгарка");

		/// <summary>
		/// Вид оборудования - Эксцентриковая шлифмашина
		/// </summary>
		public static EquipmentKind EccentricGrinder = new(6, nameof(EccentricGrinder), EquipmentType.ElectricInstrument, "Эксцентриковая шлифмашина");
		#endregion

		#region Ударный

		/// <summary>
		/// Вид оборудования - Слесарный
		/// </summary>
		public static EquipmentKind Locksmith = new(7, nameof(Locksmith), EquipmentType.PercussiveInstrument, "Слесарный");

		/// <summary>
		/// Вид оборудования - Киянка
		/// </summary>
		public static EquipmentKind Mallet = new(8, nameof(Mallet), EquipmentType.PercussiveInstrument, "Киянка");

		/// <summary>
		/// Вид оборудования - Кувалда
		/// </summary>
		public static EquipmentKind Sledgehammer = new(9, nameof(Sledgehammer), EquipmentType.PercussiveInstrument, "Кувалда");
		#endregion

		#region Зажимной

		/// <summary>
		/// Вид оборудования - Пассатижи
		/// </summary>
		public static EquipmentKind Pliers = new(10, nameof(Pliers), EquipmentType.ClampInstrument, "Пассатижи");

		/// <summary>
		/// Вид оборудования - Круглогубцы
		/// </summary>
		public static EquipmentKind RoundPliers = new(11, nameof(RoundPliers), EquipmentType.ClampInstrument, "Круглогубцы");

		/// <summary>
		/// Вид оборудования - Клещи
		/// </summary>
		public static EquipmentKind Pincers = new(12, nameof(Pincers), EquipmentType.ClampInstrument, "Клещи");

		/// <summary>
		/// Вид оборудования - Тиски Настольные
		/// </summary>
		public static EquipmentKind TableVise = new(13, nameof(TableVise), EquipmentType.ClampInstrument, "Тиски Настольные");

		/// <summary>
		/// Вид оборудования - Тиски Стуловые
		/// </summary>
		public static EquipmentKind ChairVise = new(14, nameof(ChairVise), EquipmentType.ClampInstrument, "Тиски Стуловые");
		#endregion

		#region Измерительный

		/// <summary>
		/// Вид оборудования - Линейка Металлическая
		/// </summary>
		public static EquipmentKind MetalRuler = new(15, nameof(MetalRuler), EquipmentType.MeasuringInstrument, "Линейка Металлическая");

		/// <summary>
		/// Вид оборудования - Складной метр
		/// </summary>
		public static EquipmentKind FoldingMeter = new(16, nameof(FoldingMeter), EquipmentType.MeasuringInstrument, "Складной метр");
		#endregion

		#region Крепёжный

		/// <summary>
		/// Вид оборудования - Отвертка Плоская
		/// </summary>
		public static EquipmentKind ScrewdriverFlat = new(17, nameof(ScrewdriverFlat), EquipmentType.FastenindInstrument, "Отвертка Плоская");

		/// <summary>
		/// Вид оборудования - Отвертка крестовая
		/// </summary>
		public static EquipmentKind PhillipsScrewdriver = new(18, nameof(PhillipsScrewdriver), EquipmentType.FastenindInstrument, "Отвертка крестовая");

		/// <summary>
		/// Вид оборудования - Отвертка шестигранная
		/// </summary>
		public static EquipmentKind HexScrewdriver = new(19, nameof(HexScrewdriver), EquipmentType.FastenindInstrument, "Отвертка шестигранная");

		/// <summary>
		/// Вид оборудования - Ключ Накидной
		/// </summary>
		public static EquipmentKind HingedKey = new(20, nameof(HingedKey), EquipmentType.FastenindInstrument, "Ключ Накидной");

		/// <summary>
		/// Вид оборудования - Ключ рожковый
		/// </summary>
		public static EquipmentKind HornKey = new(21, nameof(HornKey), EquipmentType.FastenindInstrument, "Ключ рожковый");

		/// <summary>
		/// Вид оборудования - Ключ разводной
		/// </summary>
		public static EquipmentKind AdjustableKey = new(22, nameof(AdjustableKey), EquipmentType.FastenindInstrument, "Ключ разводной");

		/// <summary>
		/// Вид оборудования - Ключ газовый
		/// </summary>
		public static EquipmentKind GasKey = new(23, nameof(GasKey), EquipmentType.FastenindInstrument, "Ключ газовый");
		#endregion

		#region Сверлильный

		/// <summary>
		/// Вид оборудования - Сверло по металлу
		/// </summary>
		public static EquipmentKind MetalDrillBit = new(24, nameof(MetalDrillBit), EquipmentType.DrillingInstrument, "Сверло по металлу");

		/// <summary>
		/// Вид оборудования - Сверло по дереву
		/// </summary>
		public static EquipmentKind WoodDrillBit = new(25, nameof(WoodDrillBit), EquipmentType.DrillingInstrument, "Сверло по дереву");

		/// <summary>
		/// Вид оборудования - Сверло по бетону
		/// </summary>
		public static EquipmentKind ConcreteDrillBit = new(26, nameof(ConcreteDrillBit), EquipmentType.DrillingInstrument, "Сверло по бетону");
		#endregion

		#region Слесарный

		/// <summary>
		/// Вид оборудования - Ножницы по металлу
		/// </summary>
		public static EquipmentKind MetalShears = new(27, nameof(MetalShears), EquipmentType.LocksmithInstrument, "Ножницы по металлу");

		/// <summary>
		/// Вид оборудования - Ножовки
		/// </summary>
		public static EquipmentKind Hacksaws = new(28, nameof(Hacksaws), EquipmentType.LocksmithInstrument, "Ножовки");

		#endregion

		#region Пневматический

		/// <summary>
		/// Вид оборудования - Клепальник
		/// </summary>
		public static EquipmentKind Riveter = new(29, nameof(Riveter), EquipmentType.PneumaticInstrument, "Клепальник");

		/// <summary>
		/// Вид оборудования - Бонговщик
		/// </summary>
		public static EquipmentKind BongPlayer = new(30, nameof(BongPlayer), EquipmentType.PneumaticInstrument, "Бонговщик");

		/// <summary>
		/// Вид оборудования - Пневмошуруповерт
		/// </summary>
		public static EquipmentKind PneumaticScrewdriver = new(31, nameof(PneumaticScrewdriver), EquipmentType.PneumaticInstrument, "Пневмошуруповерт");

		#endregion

		public EquipmentKind(int id, string name, EquipmentType type, string descriptions) : base(id, name)
		{
			EquipmentType = type;
			EquipmentTypeId = type.Id;
			Descriptions = descriptions;
		}

		public string Descriptions { get; }

		/// <summary>
		/// Тип оборудования
		/// </summary>
		public EquipmentType EquipmentType { get; }

		/// <summary>
		/// Внешний ключ <see cref="EquipmentType"/>
		/// </summary>
		public int EquipmentTypeId { get; private set; }
	}
}
