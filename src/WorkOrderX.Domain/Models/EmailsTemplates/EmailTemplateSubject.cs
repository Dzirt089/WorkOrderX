using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.Models.EmailsTemplates
{
	public class EmailTemplateSubject : Enumeration
	{
		/// <summary>
		/// Шаблон темы письма "Новая"
		/// </summary>
		public static EmailTemplateSubject New = new(1, "Зарегистрирована новая заявка на выполнение");

		/// <summary>
		/// Шаблон темы письма "Изменена заказчиком после возврата"
		/// </summary>
		public static EmailTemplateSubject Changed = new(2, "Заявку изменили и отправили на выполнение");

		/// <summary>
		/// Шаблон темы письма "В работе"
		/// </summary>
		public static EmailTemplateSubject InWork = new(3, "Заявка взята в работу");

		/// <summary>
		/// Шаблон темы письма "Перенаправлена другому исполнителю"
		/// </summary>
		public static EmailTemplateSubject Redirected = new(4, "Заявка перенаправлена");

		/// <summary>
		/// Шаблон темы письма "Отклонена"
		/// </summary>
		public static EmailTemplateSubject Rejected = new(5, "Заявка отклонена");

		/// <summary>
		/// Шаблон темы письма "Завершена"
		/// </summary>
		public static EmailTemplateSubject Done = new(6, "Заявка выполнена");

		/// <summary>
		/// Шаблон темы письма "Отложена"
		/// </summary>
		public static EmailTemplateSubject Postponed = new(7, "Заявка отложена");

		/// <summary>
		/// Шаблон темы письма "Возвращена заказчику"
		/// </summary>
		public static EmailTemplateSubject Returned = new(8, "Заявка возвращена");

		public EmailTemplateSubject(int id, string name) : base(id, name)
		{
		}

		public static EmailTemplateSubject Parse(string name) => name?.ToLower() switch
		{
			"new" => New,
			"inwork" => InWork,
			"redirected" => Redirected,
			"rejected" => Rejected,
			"done" => Done,
			"postponed" => Postponed,
			"returned" => Returned,
			"changed" => Changed,
			_ => throw new EnumerationValueNotFoundException($"Unknown Email Template Subject name {nameof(name)}")
		};
	}
}
