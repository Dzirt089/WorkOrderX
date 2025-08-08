using WorkOrderX.Application.Commands.Interfaces;

namespace WorkOrderX.Application.Commands.ProcessRequest
{
	public record UpdateStatusRedirectedRequestCommand : ICommand<bool>
	{
		/// <summary>
		/// Идентификатор заявки.
		/// </summary>
		public Guid Id { get; init; }

		/// <summary>
		/// Статус заявки, который будет установлен.
		/// </summary>
		public string ApplicationStatus { get; init; }

		/// <summary>
		/// Внутренний комментарий к заявке, который могут указывать друг другу заказчик/исполнитель.
		/// </summary>
		public string? InternalComment { get; init; }

		/// <summary>
		/// Идентификатор исполнителя, которому перенаправляется заявка.
		/// </summary>
		public Guid ExecutorEmployeeId { get; init; }
	}
}
