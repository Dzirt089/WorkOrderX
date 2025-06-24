using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Заявка
	/// </summary>
	public class ProcessRequest : Entity
	{
		/// <summary>
		/// Номер заявки
		/// </summary>
		public ApplicationNumber ApplicationNumber { get; set; }

		/// <summary>
		/// Тип заявки
		/// </summary>
		public ApplicationType ApplicationType { get; set; }

		/// <summary>
		/// Дата создания заявки
		/// </summary>
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Дата завершения заявки
		/// </summary>
		public DateTime CompletionAt { get; set; }

		/// <summary>
		/// Плановая дата завершения
		/// </summary>
		public DateTime PlannedAt { get; set; }

		/// <summary>
		/// Тип оборудования
		/// </summary>
		public EquipmentType EquipmentType { get; set; }

		/// <summary>
		/// Вид оборудования
		/// </summary>
		public EquipmentKind EquipmentKind { get; set; }

		/// <summary>
		/// Модель оборудования
		/// </summary>
		public EquipmentModel EquipmentModel { get; set; }

		/// <summary>
		/// Серийный номер
		/// </summary>
		public string SerialNumber { get; set; }//TODO: Если пришлют список - сделать списком. Иначе строка

		/// <summary>
		/// Тип поломки
		/// </summary>
		public TypeBreakdown TypeBreakdown { get; set; }

		/// <summary>
		/// Описание неисправности
		/// </summary>
		public DescriptionMalfunction DescriptionMalfunction { get; set; }

		/// <summary>
		/// Статус заявки
		/// </summary>
		public ApplicationStatus ApplicationStatus { get; set; }

		/// <summary>
		/// Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.
		/// </summary>
		public InternalComment InternalComment { get; set; }

		/// <summary>
		/// Заказчик заявки (кто создал)
		/// </summary>
		public Guid CustomerEmployee { get; set; }

		/// <summary>
		/// Исполнитель заявки (кому прислали на исполнение заявки)
		/// </summary>
		public Guid ExecutorEmployee { get; set; }
	}
}
