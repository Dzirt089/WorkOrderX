using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Тип поломки
	/// </summary>
	public class TypeBreakdown : Enumeration
	{
		#region Тип заявок - Ремонт оборудования

		#region Электро	

		/// <summary>
		/// Тип поломки - Износ щеток двигателя
		/// </summary>
		public static TypeBreakdown EngineBrushWear = new(1, nameof(EngineBrushWear), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Тип поломки - Перегорание обмоток
		/// </summary>
		public static TypeBreakdown WindingBurnout = new(2, nameof(WindingBurnout), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Тип поломки - Разрушение подшипников
		/// </summary>
		public static TypeBreakdown BearingFailure = new(3, nameof(BearingFailure), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Тип поломки - Выход из строя кнопки включения
		/// </summary>
		public static TypeBreakdown PowerButtonFailure = new(4, nameof(PowerButtonFailure), EquipmentType.ElectricInstrument);

		/// <summary>
		/// Тип поломки - Деградация аккумулятора 
		/// </summary>
		public static TypeBreakdown BatteryDegradation = new(5, nameof(BatteryDegradation), EquipmentType.ElectricInstrument);

		#endregion

		#region Ударный

		/// <summary>
		/// Тип поломки - Раскол бойка
		/// </summary>
		public static TypeBreakdown SplittingStriker = new(6, nameof(SplittingStriker), EquipmentType.PercussiveInstrument);

		/// <summary>
		/// Тип поломки - Разрушение рукояти
		/// </summary>
		public static TypeBreakdown DestructionHandle = new(7, nameof(DestructionHandle), EquipmentType.PercussiveInstrument);

		/// <summary>
		/// Тип поломки - Люфт головки
		/// </summary>
		public static TypeBreakdown HeadBacklash = new(8, nameof(HeadBacklash), EquipmentType.PercussiveInstrument);

		/// <summary>
		/// Тип поломки - Деформация ударной части
		/// </summary>
		public static TypeBreakdown DeformationImpactPart = new(9, nameof(DeformationImpactPart), EquipmentType.PercussiveInstrument);

		/// <summary>
		/// Тип поломки - Коррозия металла
		/// </summary>
		public static TypeBreakdown MetalCorrosion = new(10, nameof(MetalCorrosion), EquipmentType.PercussiveInstrument);
		#endregion

		#region Зажимной

		/// <summary>
		/// Тип поломки - Износ губок
		/// </summary>
		public static TypeBreakdown SpongeWear = new(11, nameof(SpongeWear), EquipmentType.ClampInstrument);

		/// <summary>
		/// Тип поломки - Ослабление пружины
		/// </summary>
		public static TypeBreakdown SpringLoosening = new(12, nameof(SpringLoosening), EquipmentType.ClampInstrument);

		/// <summary>
		/// Тип поломки - Деформация рамки
		/// </summary>
		public static TypeBreakdown FrameDeformation = new(13, nameof(FrameDeformation), EquipmentType.ClampInstrument);

		/// <summary>
		/// Тип поломки - Разболтанность шарнира
		/// </summary>
		public static TypeBreakdown LooseHinges = new(14, nameof(LooseHinges), EquipmentType.ClampInstrument);

		/// <summary>
		/// Тип поломки -  Срыв резьбы регулировочного винта
		/// </summary>
		public static TypeBreakdown ThreadBreakageAdjustingScrew = new(15, nameof(ThreadBreakageAdjustingScrew), EquipmentType.ClampInstrument);
		#endregion

		#region Измерительный

		/// <summary>
		/// Тип поломки - Разгерметизация корпуса
		/// </summary>
		public static TypeBreakdown HullDepressurization = new(16, nameof(HullDepressurization), EquipmentType.MeasuringInstrument);

		/// <summary>
		/// Тип поломки - Износ измерительных поверхностей
		/// </summary>
		public static TypeBreakdown WearMeasuringSurfaces = new(17, nameof(WearMeasuringSurfaces), EquipmentType.MeasuringInstrument);

		/// <summary>
		/// Тип поломки - Размагничивание
		/// </summary>
		public static TypeBreakdown Demagnetization = new(18, nameof(Demagnetization), EquipmentType.MeasuringInstrument);

		/// <summary>
		/// Тип поломки - Деформация линейных элементов
		/// </summary>
		public static TypeBreakdown DeformationLinearElements = new(19, nameof(DeformationLinearElements), EquipmentType.MeasuringInstrument);

		/// <summary>
		/// Тип поломки - Загрязнение шкалы
		/// </summary>
		public static TypeBreakdown ScaleContamination = new(20, nameof(ScaleContamination), EquipmentType.MeasuringInstrument);
		#endregion

		#region Крепёжный

		/// <summary>
		/// Тип поломки - Срыв граней
		/// </summary>
		public static TypeBreakdown BreakingEdges = new(21, nameof(BreakingEdges), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Тип поломки - Износ рабочего профиля
		/// </summary>
		public static TypeBreakdown WearWorkingProfile = new(22, nameof(WearWorkingProfile), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Тип поломки - Трещины в металле
		/// </summary>
		public static TypeBreakdown CracksInMetal = new(23, nameof(CracksInMetal), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Тип поломки - Деформация стержня
		/// </summary>
		public static TypeBreakdown DeformationRod = new(24, nameof(DeformationRod), EquipmentType.FastenindInstrument);

		/// <summary>
		/// Тип поломки - Разрушение храпового механизма
		/// </summary>
		public static TypeBreakdown DestructionRatchetMechanism = new(25, nameof(DestructionRatchetMechanism), EquipmentType.FastenindInstrument);

		#endregion

		#region Сверлильный

		/// <summary>
		/// Тип поломки - Затупление режущей кромки
		/// </summary>
		public static TypeBreakdown BluntingCuttingEdge = new(26, nameof(BluntingCuttingEdge), EquipmentType.DrillingInstrument);

		/// <summary>
		/// Тип поломки - Искривление оси
		/// </summary>
		public static TypeBreakdown AxisCurvature = new(27, nameof(AxisCurvature), EquipmentType.DrillingInstrument);

		/// <summary>
		/// Тип поломки - Выкрашивание победитовых напаек
		/// </summary>
		public static TypeBreakdown CultivationVictoriousRations = new(28, nameof(CultivationVictoriousRations), EquipmentType.DrillingInstrument);

		/// <summary>
		/// Тип поломки - Засорение спиральных канавок
		/// </summary>
		public static TypeBreakdown CloggingSpiralGrooves = new(29, nameof(CloggingSpiralGrooves), EquipmentType.DrillingInstrument);

		/// <summary>
		/// Тип поломки - Отлом хвостовика
		/// </summary>
		public static TypeBreakdown ShankBreakage = new(30, nameof(ShankBreakage), EquipmentType.DrillingInstrument);

		#endregion

		#region Слесарный

		/// <summary>
		/// Тип поломки - Разрушение зубьев ножовок
		/// </summary>
		public static TypeBreakdown DestructionHacksawTeeth = new(31, nameof(DestructionHacksawTeeth), EquipmentType.LocksmithInstrument);

		/// <summary>
		/// Тип поломки - Замятие режущих кромок
		/// </summary>
		public static TypeBreakdown JammingCuttingEdges = new(32, nameof(JammingCuttingEdges), EquipmentType.LocksmithInstrument);

		/// <summary>
		/// Тип поломки - Коррозия рабочих поверхностей
		/// </summary>
		public static TypeBreakdown CorrosionWorkSurfaces = new(33, nameof(CorrosionWorkSurfaces), EquipmentType.LocksmithInstrument);

		/// <summary>
		/// Тип поломки - Отслоение покрытий
		/// </summary>
		public static TypeBreakdown PeelingCoatings = new(34, nameof(PeelingCoatings), EquipmentType.LocksmithInstrument);

		/// <summary>
		/// Тип поломки - Поломка регулировочных механизмов
		/// </summary>
		public static TypeBreakdown FailureAdjustmentMechanisms = new(35, nameof(FailureAdjustmentMechanisms), EquipmentType.LocksmithInstrument);

		#endregion

		#region Пневматический

		/// <summary>
		/// Тип поломки - Износ уплотнений
		/// </summary>
		public static TypeBreakdown WearSeals = new(36, nameof(WearSeals), EquipmentType.PneumaticInstrument);

		/// <summary>
		/// Тип поломки - Засорение воздушных каналов
		/// </summary>
		public static TypeBreakdown CloggingAirDucts = new(37, nameof(CloggingAirDucts), EquipmentType.PneumaticInstrument);

		/// <summary>
		/// Тип поломки - Коррозия цилиндров
		/// </summary>
		public static TypeBreakdown CylinderCorrosion = new(38, nameof(CylinderCorrosion), EquipmentType.PneumaticInstrument);

		/// <summary>
		/// Тип поломки - Разрушение ударного механизма
		/// </summary>
		public static TypeBreakdown DestructionImpactMechanism = new(39, nameof(DestructionImpactMechanism), EquipmentType.PneumaticInstrument);

		/// <summary>
		/// Тип поломки - Люфт ротора
		/// </summary>
		public static TypeBreakdown RotorBacklash = new(40, nameof(RotorBacklash), EquipmentType.PneumaticInstrument);

		#endregion

		#endregion

		#region Тип заявок - Хозяйственные работы

		/// <summary>
		/// Тип хозяйственные поломки
		/// </summary>
		public static readonly TypeBreakdown РouseholdСhores = new(41, nameof(РouseholdСhores), EquipmentType.None);

		#endregion

		public TypeBreakdown(int id, string name, EquipmentType type) : base(id, name)
		{
			EquipmentType = type;
		}

		public EquipmentType EquipmentType { get; }

		public static TypeBreakdown Parse(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new EnumerationValueNotFoundException("Name is null or empty");

			var match = GetAll<TypeBreakdown>()
				.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))
				?? throw new EnumerationValueNotFoundException($"Unknown equipment type name '{name}'");

			return match;
		}
	}
}
