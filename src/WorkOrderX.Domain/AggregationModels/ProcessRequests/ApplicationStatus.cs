using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

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
		public readonly static ApplicationStatus New = new(1, nameof(New));

		/// <summary>
		/// В работе
		/// </summary>
		public readonly static ApplicationStatus InWork = new(2, nameof(InWork));

		/// <summary>
		/// Перенаправлена другому исполнителю
		/// </summary>
		public readonly static ApplicationStatus Redirected = new(3, nameof(Redirected));

		/// <summary>
		/// Отклонена
		/// </summary>
		public readonly static ApplicationStatus Rejected = new(4, nameof(Rejected));

		/// <summary>
		/// Отложена
		/// </summary>
		public readonly static ApplicationStatus Postponed = new(5, nameof(Postponed));

		/// <summary>
		/// Завершена
		/// </summary>
		public readonly static ApplicationStatus Done = new(6, nameof(Done));

		/// <summary>
		/// Возвращена заказчику
		/// </summary>
		public readonly static ApplicationStatus Returned = new(7, nameof(Returned));

		/// <summary>
		/// Изменена заказчиком после возврата
		/// </summary>
		public readonly static ApplicationStatus Changed = new(8, nameof(Changed));

		public ApplicationStatus(int id, string name) : base(id, name)
		{
		}

		public static ApplicationStatus Parse(string name) => name?.ToLower() switch
		{
			"new" => New,
			"inwork" => InWork,
			"redirected" => Redirected,
			"rejected" => Rejected,
			"postponed" => Postponed,
			"done" => Done,
			"returned" => Returned,
			"changed" => Changed,
			_ => throw new DomainException("Unknown application status name")
		};
	}
}
