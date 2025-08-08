using WorkOrderX.Application.Commands.Interfaces;

namespace WorkOrderX.Application.Commands.ProcessRequest
{
	public record UpdateInternalCommentCommand : ICommand<bool>
	{
		/// <summary>
		/// Идентификатор заявки.
		/// </summary>
		public Guid Id { get; init; }

		/// <summary>
		/// Внутренний комментарий к заявке, который могут указывать друг другу заказчик/исполнитель.
		/// </summary>
		public string? InternalComment { get; init; }
	}
}
