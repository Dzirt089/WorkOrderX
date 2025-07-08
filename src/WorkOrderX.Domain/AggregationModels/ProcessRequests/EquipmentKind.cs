using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

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
		public static EquipmentKind ElectricKnives = new(1, nameof(ElectricKnives), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Вид оборудования - Лобзик
		/// </summary>
		public static EquipmentKind Jigsaw = new(2, nameof(Jigsaw), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Вид оборудования - Дрель
		/// </summary>
		public static EquipmentKind Drill = new(3, nameof(Drill), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Вид оборудования - Шуруповерт
		/// </summary>
		public static EquipmentKind Screwdriver = new(4, nameof(Screwdriver), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Вид оборудования - Болгарка 
		/// </summary>
		public static EquipmentKind Grinder = new(5, nameof(Grinder), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Вид оборудования - Эксцентриковая шлифмашина
		/// </summary>
		public static EquipmentKind EccentricGrinder = new(6, nameof(EccentricGrinder), EquipmentType.ElectricInstrument);
		#endregion

		#region Ударный

		/// <summary>
		/// Вид оборудования - Слесарный
		/// </summary>
		public static EquipmentKind Locksmith = new(7, nameof(Locksmith), EquipmentType.PercussiveInstrument);

		/// <summary>
		/// Вид оборудования - Киянка
		/// </summary>
		public static EquipmentKind Mallet = new(8, nameof(Mallet), EquipmentType.PercussiveInstrument);

		/// <summary>
		/// Вид оборудования - Кувалда
		/// </summary>
		public static EquipmentKind Sledgehammer = new(9, nameof(Sledgehammer), EquipmentType.PercussiveInstrument);
		#endregion

		#region Зажимной

		/// <summary>
		/// Вид оборудования - Пассатижи
		/// </summary>
		public static EquipmentKind Pliers = new(10, nameof(Pliers), EquipmentType.ClampInstrument);

		/// <summary>
		/// Вид оборудования - Круглогубцы
		/// </summary>
		public static EquipmentKind RoundPliers = new(11, nameof(RoundPliers), EquipmentType.ClampInstrument);

		/// <summary>
		/// Вид оборудования - Клещи
		/// </summary>
		public static EquipmentKind Pincers = new(12, nameof(Pincers), EquipmentType.ClampInstrument);

		/// <summary>
		/// Вид оборудования - Тиски Настольные
		/// </summary>
		public static EquipmentKind TableVise = new(13, nameof(TableVise), EquipmentType.ClampInstrument);

		/// <summary>
		/// Вид оборудования - Тиски Стуловые
		/// </summary>
		public static EquipmentKind ChairVise = new(14, nameof(ChairVise), EquipmentType.ClampInstrument);
		#endregion

		#region Измерительный

		/// <summary>
		/// Вид оборудования - Линейка Металлическая
		/// </summary>
		public static EquipmentKind MetalRuler = new(15, nameof(MetalRuler), EquipmentType.MeasuringInstrument);

		/// <summary>
		/// Вид оборудования - Складной метр
		/// </summary>
		public static EquipmentKind FoldingMeter = new(16, nameof(FoldingMeter), EquipmentType.MeasuringInstrument);
		#endregion

		#region Крепёжный

		/// <summary>
		/// Вид оборудования - Отвертка Плоская
		/// </summary>
		public static EquipmentKind ScrewdriverFlat = new(17, nameof(ScrewdriverFlat), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Вид оборудования - Отвертка крестовая
		/// </summary>
		public static EquipmentKind PhillipsScrewdriver = new(18, nameof(PhillipsScrewdriver), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Вид оборудования - Отвертка шестигранная
		/// </summary>
		public static EquipmentKind HexScrewdriver = new(19, nameof(HexScrewdriver), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Вид оборудования - Ключ Накидной
		/// </summary>
		public static EquipmentKind HingedKey = new(20, nameof(HingedKey), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Вид оборудования - Ключ рожковый
		/// </summary>
		public static EquipmentKind HornKey = new(21, nameof(HornKey), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Вид оборудования - Ключ разводной
		/// </summary>
		public static EquipmentKind AdjustableKey = new(22, nameof(AdjustableKey), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Вид оборудования - Ключ газовый
		/// </summary>
		public static EquipmentKind GasKey = new(23, nameof(GasKey), EquipmentType.FastenindInstrument);
		#endregion

		#region Сверлильный

		/// <summary>
		/// Вид оборудования - Сверло по металлу
		/// </summary>
		public static EquipmentKind MetalDrillBit = new(24, nameof(MetalDrillBit), EquipmentType.DrillingInstrument);

		/// <summary>
		/// Вид оборудования - Сверло по дереву
		/// </summary>
		public static EquipmentKind WoodDrillBit = new(25, nameof(WoodDrillBit), EquipmentType.DrillingInstrument);

		/// <summary>
		/// Вид оборудования - Сверло по бетону
		/// </summary>
		public static EquipmentKind ConcreteDrillBit = new(26, nameof(ConcreteDrillBit), EquipmentType.DrillingInstrument);
		#endregion

		#region Слесарный

		/// <summary>
		/// Вид оборудования - Ножницы по металлу
		/// </summary>
		public static EquipmentKind MetalShears = new(27, nameof(MetalShears), EquipmentType.LocksmithInstrument);

		/// <summary>
		/// Вид оборудования - Ножовки
		/// </summary>
		public static EquipmentKind Hacksaws = new(28, nameof(Hacksaws), EquipmentType.LocksmithInstrument);

		#endregion

		#region Пневматический

		/// <summary>
		/// Вид оборудования - Клепальник
		/// </summary>
		public static EquipmentKind Riveter = new(29, nameof(Riveter), EquipmentType.PneumaticInstrument);

		/// <summary>
		/// Вид оборудования - Бонговщик
		/// </summary>
		public static EquipmentKind BongPlayer = new(30, nameof(BongPlayer), EquipmentType.PneumaticInstrument);

		/// <summary>
		/// Вид оборудования - Пневмошуруповерт
		/// </summary>
		public static EquipmentKind PneumaticScrewdriver = new(31, nameof(PneumaticScrewdriver), EquipmentType.PneumaticInstrument);

		#endregion

		public EquipmentKind(int id, string name, EquipmentType type) : base(id, name)
		{
			EquipmentType = type;
		}

		public EquipmentType EquipmentType { get; }

		public static EquipmentKind Parse(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new EnumerationValueNotFoundException("Name is null or empty");

			var match = GetAll<EquipmentKind>()
				.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))
				?? throw new EnumerationValueNotFoundException($"Unknown equipment type name '{name}'");

			return match;
		}
	}
}
