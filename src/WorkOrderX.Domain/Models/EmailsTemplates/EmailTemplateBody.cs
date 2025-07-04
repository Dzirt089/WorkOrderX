using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.Models.EmailsTemplates
{
	public class EmailTemplateBody : Enumeration
	{
		/// <summary>
		/// Шаблон тела письма "Новая"
		/// </summary>
		public static EmailTemplateBody New = new(1, @"Уважаемый {executor.Name.Value},
						   Появилась новая заявка №{request.ApplicationNumber.Value} на выполнение.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Заказчик: {customer.Name.Value}
						   Номер телефона заказчика: {customer.Phone.Value}
						   С уважением,Команда WorkOrderX");

		/// <summary>
		/// Шаблон тела письма "Изменена заказчиком после возврата"
		/// </summary>
		public static EmailTemplateBody Changed = new(2, @"Уважаемый {executor.Name.Value},
						   Статус заявки №{request.ApplicationNumber.Value} изменен на {notification.NewStatus}.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Заказчик: {customer.Name.Value}
						   Номер телефона заказчика: {customer.Phone.Value}
						   С уважением,Команда WorkOrderX");

		/// <summary>
		/// Шаблон тела письма "В работе"
		/// </summary>
		public static EmailTemplateBody InWork = new(3, @"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} взята в работу.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX");

		/// <summary>
		/// Шаблон тела письма "Перенаправлена другому исполнителю"
		/// </summary>
		public static EmailTemplateBody Redirected = new(4, @"Уважаемый {executor.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была перенаправлена Вам.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Заказчик: {customer.Name.Value}
						   Номер телефона заказчика: {customer.Phone.Value}
						   С уважением, Команда WorkOrderX");

		/// <summary>
		/// Шаблон тела письма "Отклонена"
		/// </summary>
		public static EmailTemplateBody Rejected = new(5, @"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была отклонена.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX");

		/// <summary>
		/// Шаблон тела письма "Завершена"
		/// </summary>
		public static EmailTemplateBody Done = new(6, @"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была успешно выполнена.
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX");

		/// <summary>
		/// Шаблон тела письма "Отложена"
		/// </summary>
		public static EmailTemplateBody Postponed = new(7, @"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была отложена.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX");

		/// <summary>
		/// Шаблон тела письма "Возвращена заказчику"
		/// </summary>
		public static EmailTemplateBody Returned = new(8, @"Уважаемый {customer.Name.Value},
						   Заявка №{request.ApplicationNumber.Value} была Вам возвращена для исправлений \ уточнений.
						   Комментарий: {notification.Comment ?? string.Empty}
						   Исполнитель: {executor.Name.Value}
						   Номер телефона исполнителя: {executor.Phone.Value}
						   С уважением, Команда WorkOrderX");

		public EmailTemplateBody(int id, string name) : base(id, name)
		{
		}

		public static EmailTemplateBody Parse(string name) => name?.ToLower() switch
		{
			"new" => New,
			"inwork" => InWork,
			"redirected" => Redirected,
			"rejected" => Rejected,
			"done" => Done,
			"postponed" => Postponed,
			"returned" => Returned,
			"changed" => Changed,
			_ => throw new EnumerationValueNotFoundException($"Unknown Email Template Body name {nameof(name)}")
		};
	}
}
