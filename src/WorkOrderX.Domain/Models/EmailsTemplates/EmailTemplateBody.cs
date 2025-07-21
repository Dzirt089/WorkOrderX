using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.Models.EmailsTemplates
{
	public class EmailTemplateBody : Enumeration
	{
		/// <summary>
		/// Шаблон тела письма "Новая"
		/// </summary>
		public static EmailTemplateBody New = new(
			id: 1,
			name: @"Уважаемый {executor.Name.Value},
				Появилась новая заявка №{request.ApplicationNumber.Value} на выполнение.
				Важность: {notification.Importance.Descriptions}.
				Комментарий: {notification.Comment ?? string.Empty}
				Заказчик: {customer.Name.Value}
				Номер телефона заказчика: {customer.Phone.Value}
				С уважением,Команда WorkOrderX",
			description: "Шаблон тела письма \"Новая\"");

		/// <summary>
		/// Шаблон тела письма "Изменена заказчиком после возврата"
		/// </summary>
		public static EmailTemplateBody Changed = new(
			id: 2,
			name: @"Уважаемый {executor.Name.Value},
				Статус заявки №{request.ApplicationNumber.Value} изменен на {notification.NewStatus}.
				Важность: {notification.Importance.Descriptions}.
				Комментарий: {notification.Comment ?? string.Empty}
				Заказчик: {customer.Name.Value}
				Номер телефона заказчика: {customer.Phone.Value}
				С уважением,Команда WorkOrderX",
			description: "Шаблон тела письма \"Изменена заказчиком после возврата\"");

		/// <summary>
		/// Шаблон тела письма "В работе"
		/// </summary>
		public static EmailTemplateBody InWork = new(
			id: 3,
			name: @"Уважаемый {customer.Name.Value},
				Заявка №{request.ApplicationNumber.Value} взята в работу.
				Важность: {notification.Importance.Descriptions}.
				Комментарий: {notification.Comment ?? string.Empty}
				Исполнитель: {executor.Name.Value}
				Номер телефона исполнителя: {executor.Phone.Value}
				С уважением, Команда WorkOrderX",
			description: "Шаблон тела письма \"В работе\"");

		/// <summary>
		/// Шаблон тела письма "Перенаправлена другому исполнителю"
		/// </summary>
		public static EmailTemplateBody Redirected = new(
			id: 4,
			name: @"Уважаемый {executor.Name.Value},
				Заявка №{request.ApplicationNumber.Value} была перенаправлена Вам.
				Важность: {notification.Importance.Descriptions}.
				Комментарий: {notification.Comment ?? string.Empty}
				Заказчик: {customer.Name.Value}
				Номер телефона заказчика: {customer.Phone.Value}
				С уважением, Команда WorkOrderX",
			description: "Шаблон тела письма \"Перенаправлена другому исполнителю\"");

		/// <summary>
		/// Шаблон тела письма "Отклонена"
		/// </summary>
		public static EmailTemplateBody Rejected = new(
			id: 5,
			name: @"Уважаемый {customer.Name.Value},
					Заявка №{request.ApplicationNumber.Value} была отклонена.
					Важность: {notification.Importance.Descriptions}.
					Комментарий: {notification.Comment ?? string.Empty}
					Исполнитель: {executor.Name.Value}
					Номер телефона исполнителя: {executor.Phone.Value}
					С уважением, Команда WorkOrderX",
			description: "Шаблон тела письма \"Отклонена\"");

		/// <summary>
		/// Шаблон тела письма "Завершена"
		/// </summary>
		public static EmailTemplateBody Done = new(
			id: 6,
			name: @"Уважаемый {customer.Name.Value},
				Заявка №{request.ApplicationNumber.Value} была успешно выполнена.
				Важность: {notification.Importance.Descriptions}.
				Исполнитель: {executor.Name.Value}
				Номер телефона исполнителя: {executor.Phone.Value}
				С уважением, Команда WorkOrderX",
			description: "Шаблон тела письма \"Завершена\"");

		/// <summary>
		/// Шаблон тела письма "Отложена"
		/// </summary>
		public static EmailTemplateBody Postponed = new(
			id: 7,
			name: @"Уважаемый {customer.Name.Value},
				Заявка №{request.ApplicationNumber.Value} была отложена.
				Важность: {notification.Importance.Descriptions}.
				Комментарий: {notification.Comment ?? string.Empty}
				Исполнитель: {executor.Name.Value}
				Номер телефона исполнителя: {executor.Phone.Value}
				С уважением, Команда WorkOrderX"
			, description: "Шаблон тела письма \"Отложена\"");

		/// <summary>
		/// Шаблон тела письма "Возвращена заказчику"
		/// </summary>
		public static EmailTemplateBody Returned = new(
			id: 8,
			name: @"Уважаемый {customer.Name.Value},
				Заявка №{request.ApplicationNumber.Value} была Вам возвращена для исправлений \ уточнений.
				Важность: {notification.Importance.Descriptions}.
				Комментарий: {notification.Comment ?? string.Empty}
				Исполнитель: {executor.Name.Value}
				Номер телефона исполнителя: {executor.Phone.Value}
				С уважением, Команда WorkOrderX",
			description: "Шаблон тела письма \"Возвращена заказчику\"");

		public EmailTemplateBody(int id, string name, string description) : base(id, name, description) { }
	}

}
