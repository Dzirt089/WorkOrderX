namespace WorkOrderX.Http.Models
{
	public record UpdateStatusDoneOrRejectedModel
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
		/// Дата и время завершения заявки 
		/// </summary>
		public string CompletedAt { get; init; }
	}
}
