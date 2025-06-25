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
		public static ApplicationStatus New = new(1, nameof(New));

		/// <summary>
		/// В работе
		/// </summary>
		public static ApplicationStatus InWork = new(2, nameof(InWork));

		/// <summary>
		/// Перенаправлена
		/// </summary>
		public static ApplicationStatus Redirected = new(3, nameof(Redirected));

		/// <summary>
		/// Отклонена
		/// </summary>
		public static ApplicationStatus Rejected = new(4, nameof(Rejected));

		/// <summary>
		/// Отложена
		/// </summary>
		public static ApplicationStatus Postponed = new(5, nameof(Postponed));

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
			_ => throw new DomainException("Unknown application status name")
		};
	}
}
