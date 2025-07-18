﻿using MediatR;

namespace WorkOrderX.Application.Commands.ProcessRequest
{
	/// <summary>
	/// Команда для обновления статуса заявки в работе, возвращенной или отложенной.
	/// </summary>
	public record UpdateStatusInWorkOrReturnedOrPostponedRequestCommand : IRequest<bool>
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
	}
}
