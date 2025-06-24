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
		public static ApplicationStatus New = new(1, "New");

		/// <summary>
		/// В работе
		/// </summary>
		public static ApplicationStatus InWork = new(2, "In_Work");

		/// <summary>
		/// Перенаправлена
		/// </summary>
		public static ApplicationStatus Redirected = new(3, "Redirected");

		/// <summary>
		/// Отклонена
		/// </summary>
		public static ApplicationStatus Rejected = new(4, "Rejected");

		/// <summary>
		/// Отложена
		/// </summary>
		public static ApplicationStatus Postponed = new(5, "Postponed");

		public ApplicationStatus(int id, string name) : base(id, name)
		{
		}

		public static ApplicationStatus Parse(string name) => name?.ToLower() switch
		{
			"new" => New,
			"in_work" => InWork,
			"redirected" => Redirected,
			"rejected" => Rejected,
			"postponed" => Postponed,
			_ => throw new DomainException("Unknown application status name")
		};
	}
}
