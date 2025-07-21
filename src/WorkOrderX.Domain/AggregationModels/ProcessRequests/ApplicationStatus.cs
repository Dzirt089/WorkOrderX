using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Статус заявки
	/// </summary>
	public class ApplicationStatus : Enumeration
	{
		/// <summary>
		/// Новая
		/// </summary>
		public readonly static ApplicationStatus New = new(1, nameof(New), "Новая");

		/// <summary>
		/// В работе
		/// </summary>
		public readonly static ApplicationStatus InWork = new(2, nameof(InWork), "В работе");

		/// <summary>
		/// Перенаправлена другому исполнителю
		/// </summary>
		public readonly static ApplicationStatus Redirected = new(3, nameof(Redirected), "Перенаправлена другому исполнителю");

		/// <summary>
		/// Отклонена
		/// </summary>
		public readonly static ApplicationStatus Rejected = new(4, nameof(Rejected), "Отклонена");

		/// <summary>
		/// Отложена
		/// </summary>
		public readonly static ApplicationStatus Postponed = new(5, nameof(Postponed), "Отложена");

		/// <summary>
		/// Завершена
		/// </summary>
		public readonly static ApplicationStatus Done = new(6, nameof(Done), "Завершена");

		/// <summary>
		/// Возвращена заказчику
		/// </summary>
		public readonly static ApplicationStatus Returned = new(7, nameof(Returned), "Возвращена заказчику");

		/// <summary>
		/// Изменена заказчиком после возврата
		/// </summary>
		public readonly static ApplicationStatus Changed = new(8, nameof(Changed), "Изменена заказчиком после возврата");

		// Приватный конструктор без параметров для EF
		private ApplicationStatus() { }

		public ApplicationStatus(int id, string name, string descriptions) : base(id, name, descriptions) { }

	}
}
