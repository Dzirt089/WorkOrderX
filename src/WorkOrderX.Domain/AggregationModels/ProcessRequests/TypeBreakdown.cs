using WorkOrderX.Domain.Root;

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
		public readonly static TypeBreakdown EngineBrushWear = new(1, nameof(EngineBrushWear), EquipmentType.ElectricInstrument, "Износ щеток двигателя");

		/// <summary>
		/// Тип поломки - Перегорание обмоток
		/// </summary>
		public readonly static TypeBreakdown WindingBurnout = new(2, nameof(WindingBurnout), EquipmentType.ElectricInstrument, "Перегорание обмоток");

		/// <summary>
		/// Тип поломки - Разрушение подшипников
		/// </summary>
		public readonly static TypeBreakdown BearingFailure = new(3, nameof(BearingFailure), EquipmentType.ElectricInstrument, "Разрушение подшипников");

		/// <summary>
		/// Тип поломки - Выход из строя кнопки включения
		/// </summary>
		public readonly static TypeBreakdown PowerButtonFailure = new(4, nameof(PowerButtonFailure), EquipmentType.ElectricInstrument, "Выход из строя кнопки включения");

		/// <summary>
		/// Тип поломки - Деградация аккумулятора 
		/// </summary>
		public readonly static TypeBreakdown BatteryDegradation = new(5, nameof(BatteryDegradation), EquipmentType.ElectricInstrument, "Деградация аккумулятора");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherElectro = new(6, nameof(OtherElectro), EquipmentType.ElectricInstrument, "Другое");

		#endregion

		#region Ударный

		/// <summary>
		/// Тип поломки - Раскол бойка
		/// </summary>
		public readonly static TypeBreakdown SplittingStriker = new(7, nameof(SplittingStriker), EquipmentType.PercussiveInstrument, "Раскол бойка");

		/// <summary>
		/// Тип поломки - Разрушение рукояти
		/// </summary>
		public readonly static TypeBreakdown DestructionHandle = new(8, nameof(DestructionHandle), EquipmentType.PercussiveInstrument, "Разрушение рукояти");

		/// <summary>
		/// Тип поломки - Люфт головки
		/// </summary>
		public readonly static TypeBreakdown HeadBacklash = new(9, nameof(HeadBacklash), EquipmentType.PercussiveInstrument, "Люфт головки");

		/// <summary>
		/// Тип поломки - Деформация ударной части
		/// </summary>
		public readonly static TypeBreakdown DeformationImpactPart = new(10, nameof(DeformationImpactPart), EquipmentType.PercussiveInstrument, "Деформация ударной части");

		/// <summary>
		/// Тип поломки - Коррозия металла
		/// </summary>
		public readonly static TypeBreakdown MetalCorrosion = new(11, nameof(MetalCorrosion), EquipmentType.PercussiveInstrument, "Коррозия металла");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherShock = new(12, nameof(OtherShock), EquipmentType.PercussiveInstrument, "Другое");
		#endregion

		#region Зажимной

		/// <summary>
		/// Тип поломки - Износ губок
		/// </summary>
		public readonly static TypeBreakdown SpongeWear = new(13, nameof(SpongeWear), EquipmentType.ClampInstrument, "Износ губок");

		/// <summary>
		/// Тип поломки - Ослабление пружины
		/// </summary>
		public readonly static TypeBreakdown SpringLoosening = new(14, nameof(SpringLoosening), EquipmentType.ClampInstrument, "Ослабление пружины");

		/// <summary>
		/// Тип поломки - Деформация рамки
		/// </summary>
		public readonly static TypeBreakdown FrameDeformation = new(15, nameof(FrameDeformation), EquipmentType.ClampInstrument, "Деформация рамки");

		/// <summary>
		/// Тип поломки - Разболтанность шарнира
		/// </summary>
		public readonly static TypeBreakdown LooseHinges = new(16, nameof(LooseHinges), EquipmentType.ClampInstrument, "Разболтанность шарнира");

		/// <summary>
		/// Тип поломки -  Срыв резьбы регулировочного винта
		/// </summary>
		public readonly static TypeBreakdown ThreadBreakageAdjustingScrew = new(17, nameof(ThreadBreakageAdjustingScrew), EquipmentType.ClampInstrument, "Срыв резьбы регулировочного винта");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherClamping = new(18, nameof(OtherClamping), EquipmentType.ClampInstrument, "Другое");
		#endregion

		#region Измерительный

		/// <summary>
		/// Тип поломки - Разгерметизация корпуса
		/// </summary>
		public readonly static TypeBreakdown HullDepressurization = new(19, nameof(HullDepressurization), EquipmentType.MeasuringInstrument, "Разгерметизация корпуса");

		/// <summary>
		/// Тип поломки - Износ измерительных поверхностей
		/// </summary>
		public readonly static TypeBreakdown WearMeasuringSurfaces = new(20, nameof(WearMeasuringSurfaces), EquipmentType.MeasuringInstrument, "Износ измерительных поверхностей");

		/// <summary>
		/// Тип поломки - Размагничивание
		/// </summary>
		public readonly static TypeBreakdown Demagnetization = new(21, nameof(Demagnetization), EquipmentType.MeasuringInstrument, "Размагничивание");

		/// <summary>
		/// Тип поломки - Деформация линейных элементов
		/// </summary>
		public readonly static TypeBreakdown DeformationLinearElements = new(22, nameof(DeformationLinearElements), EquipmentType.MeasuringInstrument, "Деформация линейных элементов");

		/// <summary>
		/// Тип поломки - Загрязнение шкалы
		/// </summary>
		public readonly static TypeBreakdown ScaleContamination = new(23, nameof(ScaleContamination), EquipmentType.MeasuringInstrument, "Тип поломки - Загрязнение шкалы");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherMeasuring = new(24, nameof(OtherMeasuring), EquipmentType.MeasuringInstrument, "Другое");
		#endregion

		#region Крепёжный

		/// <summary>
		/// Тип поломки - Срыв граней
		/// </summary>
		public readonly static TypeBreakdown BreakingEdges = new(25, nameof(BreakingEdges), EquipmentType.FastenindInstrument, "Срыв граней");

		/// <summary>
		/// Тип поломки - Износ рабочего профиля
		/// </summary>
		public readonly static TypeBreakdown WearWorkingProfile = new(26, nameof(WearWorkingProfile), EquipmentType.FastenindInstrument, "Износ рабочего профиля");

		/// <summary>
		/// Тип поломки - Трещины в металле
		/// </summary>
		public readonly static TypeBreakdown CracksInMetal = new(27, nameof(CracksInMetal), EquipmentType.FastenindInstrument, "Трещины в металле");

		/// <summary>
		/// Тип поломки - Деформация стержня
		/// </summary>
		public readonly static TypeBreakdown DeformationRod = new(28, nameof(DeformationRod), EquipmentType.FastenindInstrument, "Деформация стержня");

		/// <summary>
		/// Тип поломки - Разрушение храпового механизма
		/// </summary>
		public readonly static TypeBreakdown DestructionRatchetMechanism = new(29, nameof(DestructionRatchetMechanism), EquipmentType.FastenindInstrument, "Разрушение храпового механизма");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherFastening = new(30, nameof(OtherFastening), EquipmentType.FastenindInstrument, "Другое");

		#endregion

		#region Сверлильный

		/// <summary>
		/// Тип поломки - Затупление режущей кромки
		/// </summary>
		public readonly static TypeBreakdown BluntingCuttingEdge = new(31, nameof(BluntingCuttingEdge), EquipmentType.DrillingInstrument, "Затупление режущей кромки");

		/// <summary>
		/// Тип поломки - Искривление оси
		/// </summary>
		public readonly static TypeBreakdown AxisCurvature = new(32, nameof(AxisCurvature), EquipmentType.DrillingInstrument, "Искривление оси");

		/// <summary>
		/// Тип поломки - Выкрашивание победитовых напаек
		/// </summary>
		public readonly static TypeBreakdown CultivationVictoriousRations = new(33, nameof(CultivationVictoriousRations), EquipmentType.DrillingInstrument, "Выкрашивание победитовых напаек");

		/// <summary>
		/// Тип поломки - Засорение спиральных канавок
		/// </summary>
		public readonly static TypeBreakdown CloggingSpiralGrooves = new(34, nameof(CloggingSpiralGrooves), EquipmentType.DrillingInstrument, "Засорение спиральных канавок");

		/// <summary>
		/// Тип поломки - Отлом хвостовика
		/// </summary>
		public readonly static TypeBreakdown ShankBreakage = new(35, nameof(ShankBreakage), EquipmentType.DrillingInstrument, "Отлом хвостовика");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherDrilling = new(36, nameof(OtherDrilling), EquipmentType.DrillingInstrument, "Другое");

		#endregion

		#region Слесарный

		/// <summary>
		/// Тип поломки - Разрушение зубьев ножовок
		/// </summary>
		public readonly static TypeBreakdown DestructionHacksawTeeth = new(37, nameof(DestructionHacksawTeeth), EquipmentType.LocksmithInstrument, "Разрушение зубьев ножовок");

		/// <summary>
		/// Тип поломки - Замятие режущих кромок
		/// </summary>
		public readonly static TypeBreakdown JammingCuttingEdges = new(38, nameof(JammingCuttingEdges), EquipmentType.LocksmithInstrument, "Замятие режущих кромок");

		/// <summary>
		/// Тип поломки - Коррозия рабочих поверхностей
		/// </summary>
		public readonly static TypeBreakdown CorrosionWorkSurfaces = new(39, nameof(CorrosionWorkSurfaces), EquipmentType.LocksmithInstrument, "Коррозия рабочих поверхностей");

		/// <summary>
		/// Тип поломки - Отслоение покрытий
		/// </summary>
		public readonly static TypeBreakdown PeelingCoatings = new(40, nameof(PeelingCoatings), EquipmentType.LocksmithInstrument, "Отслоение покрытий");

		/// <summary>
		/// Тип поломки - Поломка регулировочных механизмов
		/// </summary>
		public readonly static TypeBreakdown FailureAdjustmentMechanisms = new(41, nameof(FailureAdjustmentMechanisms), EquipmentType.LocksmithInstrument, "Поломка регулировочных механизмов");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherLocksmith = new(42, nameof(OtherLocksmith), EquipmentType.LocksmithInstrument, "Другое");

		#endregion

		#region Пневматический

		/// <summary>
		/// Тип поломки - Износ уплотнений
		/// </summary>
		public readonly static TypeBreakdown WearSeals = new(43, nameof(WearSeals), EquipmentType.PneumaticInstrument, "Износ уплотнений");

		/// <summary>
		/// Тип поломки - Засорение воздушных каналов
		/// </summary>
		public readonly static TypeBreakdown CloggingAirDucts = new(44, nameof(CloggingAirDucts), EquipmentType.PneumaticInstrument, "Засорение воздушных каналов");

		/// <summary>
		/// Тип поломки - Коррозия цилиндров
		/// </summary>
		public readonly static TypeBreakdown CylinderCorrosion = new(45, nameof(CylinderCorrosion), EquipmentType.PneumaticInstrument, "Коррозия цилиндров");

		/// <summary>
		/// Тип поломки - Разрушение ударного механизма
		/// </summary>
		public readonly static TypeBreakdown DestructionImpactMechanism = new(46, nameof(DestructionImpactMechanism), EquipmentType.PneumaticInstrument, "Разрушение ударного механизма");

		/// <summary>
		/// Тип поломки - Люфт ротора
		/// </summary>
		public readonly static TypeBreakdown RotorBacklash = new(47, nameof(RotorBacklash), EquipmentType.PneumaticInstrument, "Люфт ротора");

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown OtherPneumatic = new(48, nameof(OtherPneumatic), EquipmentType.PneumaticInstrument, "Другое");

		#endregion

		/// <summary>
		/// Тип поломки не указан\нет подходящего в списке
		/// </summary>
		public readonly static TypeBreakdown Other = new(49, nameof(Other), EquipmentType.Other, "Другое");

		#endregion





		// Приватный конструктор без параметров для EF
		private TypeBreakdown() { }

		public TypeBreakdown(int id, string name, EquipmentType type, string descriptions) : base(id, name, descriptions)
		{
			EquipmentType = type;
			EquipmentTypeId = type.Id;
		}

		/// <summary>
		/// Тип оборудования
		/// </summary>
		public EquipmentType EquipmentType { get; private set; }

		/// <summary>
		/// Внешний ключ <see cref="EquipmentType"/>
		/// </summary>
		public int EquipmentTypeId { get; private set; }
	}
}
