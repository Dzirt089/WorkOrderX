﻿using MediatR;

namespace WorkOrderX.Application.Commands.ProcessRequest
{
	public record CreateProcessRequestCommand : IRequest<bool>
	{
		/// <summary>
		/// Номер заявки
		/// </summary>
		public long ApplicationNumber { get; init; }

		/// <summary>
		/// Тип заявки
		/// </summary>
		public string ApplicationType { get; init; }

		/// <summary>
		/// Дата создания заявки
		/// </summary>
		public string CreatedAt { get; init; }

		/// <summary>
		/// Плановая дата завершения заявки
		/// </summary>
		public string PlannedAt { get; init; }

		/// <summary>
		/// Тип оборудования
		/// </summary>
		public string? EquipmentType { get; init; }

		/// <summary>
		/// Вид оборудования
		/// </summary>
		public string? EquipmentKind { get; init; }

		/// <summary>
		/// Модель оборудования
		/// </summary>
		public string? EquipmentModel { get; init; }

		/// <summary>
		/// Серийный номер оборудования
		/// </summary>
		public string? SerialNumber { get; init; }

		/// <summary>
		/// Тип поломки
		/// </summary>
		public string TypeBreakdown { get; init; }

		/// <summary>
		/// Описание неисправности
		/// </summary>
		public string DescriptionMalfunction { get; init; }

		/// <summary>
		/// Статус заявки
		/// </summary>
		public string ApplicationStatus { get; init; }

		/// <summary>
		/// Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.
		/// </summary>
		public string? InternalComment { get; init; }

		/// <summary>
		/// ID заказчика заявки
		/// </summary>
		public Guid CustomerEmployeeId { get; init; }
	}
}
