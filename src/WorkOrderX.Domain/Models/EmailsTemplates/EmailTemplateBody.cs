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
			name: nameof(New),
			description: @"
<pre>
Уважаемый {executor.Name.Value},

Появилась новая заявка №{request.ApplicationNumber.Value} на выполнение.
Важность: {notification.Importance.Descriptions}.
Описание проблемы: {notification.Comment}

Заказчик: {customer.Name.Value}
Номер телефона заказчика: {customer.Phone.Value}

С уважением,Команда WorkOrderX
</pre>");

		/// <summary>
		/// Шаблон тела письма "Изменена заказчиком после возврата"
		/// </summary>
		public static EmailTemplateBody Changed = new(
			id: 2,
			name: nameof(Changed),
			description: @"
<pre>
Уважаемый {executor.Name.Value},

Статус заявки №{request.ApplicationNumber.Value} изменен на {notification.NewStatus.Descriptions}.

Важность: {notification.Importance.Descriptions}.

Комментарий: {notification.Comment}

Заказчик: {customer.Name.Value}
Номер телефона заказчика: {customer.Phone.Value}

С уважением,Команда WorkOrderX
</pre>");

		/// <summary>
		/// Шаблон тела письма "В работе"
		/// </summary>
		public static EmailTemplateBody InWork = new(
			id: 3,
			name: nameof(InWork),
			description: @"
<pre>
Уважаемый {customer.Name.Value},

Заявка №{request.ApplicationNumber.Value} взята в работу.

Важность: {notification.Importance.Descriptions}.

Комментарий: {notification.Comment}

Исполнитель: {executor.Name.Value}
Номер телефона исполнителя: {executor.Phone.Value}

С уважением, Команда WorkOrderX
</pre>");

		/// <summary>
		/// Шаблон тела письма "Перенаправлена другому исполнителю"
		/// </summary>
		public static EmailTemplateBody Redirected = new(
			id: 4,
			name: nameof(Redirected),
			description: @"
<pre>
Уважаемый {executor.Name.Value},

Заявка №{request.ApplicationNumber.Value} была перенаправлена Вам.

Важность: {notification.Importance.Descriptions}.

Комментарий: {notification.Comment}

Заказчик: {customer.Name.Value}
Номер телефона заказчика: {customer.Phone.Value}

С уважением, Команда WorkOrderX
</pre>");

		/// <summary>
		/// Шаблон тела письма "Отклонена"
		/// </summary>
		public static EmailTemplateBody Rejected = new(
			id: 5,
			name: nameof(Rejected),
			description: @"
<pre>
Уважаемый {customer.Name.Value},

Заявка №{request.ApplicationNumber.Value} была отклонена.

Важность: {notification.Importance.Descriptions}.

Комментарий: {notification.Comment}

Исполнитель: {executor.Name.Value}
Номер телефона исполнителя: {executor.Phone.Value}

С уважением, Команда WorkOrderX
</pre>");

		/// <summary>
		/// Шаблон тела письма "Завершена"
		/// </summary>
		public static EmailTemplateBody Done = new(
			id: 6,
			name: nameof(Done),
			description: @"
<pre>
Уважаемый {customer.Name.Value},

Заявка №{request.ApplicationNumber.Value} была успешно выполнена.

Важность: {notification.Importance.Descriptions}.

Комментарий: {notification.Comment}

Исполнитель: {executor.Name.Value}
Номер телефона исполнителя: {executor.Phone.Value}

С уважением, Команда WorkOrderX
</pre>");

		/// <summary>
		/// Шаблон тела письма "Отложена"
		/// </summary>
		public static EmailTemplateBody Postponed = new(
			id: 7,
			name: nameof(Postponed),
			description: @"
<pre>
Уважаемый {customer.Name.Value},

Заявка №{request.ApplicationNumber.Value} была отложена.

Важность: {notification.Importance.Descriptions}.

Комментарий: {notification.Comment}

Исполнитель: {executor.Name.Value}
Номер телефона исполнителя: {executor.Phone.Value}

С уважением, Команда WorkOrderX
</pre>");

		/// <summary>
		/// Шаблон тела письма "Возвращена заказчику"
		/// </summary>
		public static EmailTemplateBody Returned = new(
			id: 8,
			name: nameof(Returned),
			description: @"
<pre>
Уважаемый {customer.Name.Value},

Заявка №{request.ApplicationNumber.Value} была Вам возвращена для исправлений \ уточнений.

Важность: {notification.Importance.Descriptions}.

Комментарий: {notification.Comment}

Исполнитель: {executor.Name.Value}
Номер телефона исполнителя: {executor.Phone.Value}

С уважением, Команда WorkOrderX
</pre>");

		public EmailTemplateBody(int id, string name, string description) : base(id, name, description) { }
	}

}
