using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.Models.EmailsTemplates
{
	public class EmailTemplateSubject : Enumeration
	{
		/// <summary>
		/// Шаблон темы письма "Новая"
		/// </summary>
		public static EmailTemplateSubject New = new(1, nameof(New), "Зарегистрирована новая заявка на выполнение");

		/// <summary>
		/// Шаблон темы письма "Изменена заказчиком после возврата"
		/// </summary>
		public static EmailTemplateSubject Changed = new(2, nameof(Changed), "Заявку изменили и отправили на выполнение");

		/// <summary>
		/// Шаблон темы письма "В работе"
		/// </summary>
		public static EmailTemplateSubject InWork = new(3, nameof(InWork), "Заявка взята в работу");

		/// <summary>
		/// Шаблон темы письма "Перенаправлена другому исполнителю"
		/// </summary>
		public static EmailTemplateSubject Redirected = new(4, nameof(Redirected), "Заявка перенаправлена");

		/// <summary>
		/// Шаблон темы письма "Отклонена"
		/// </summary>
		public static EmailTemplateSubject Rejected = new(5, nameof(Rejected), "Заявка отклонена");

		/// <summary>
		/// Шаблон темы письма "Завершена"
		/// </summary>
		public static EmailTemplateSubject Done = new(6, nameof(Done), "Заявка выполнена");

		/// <summary>
		/// Шаблон темы письма "Отложена"
		/// </summary>
		public static EmailTemplateSubject Postponed = new(7, nameof(Postponed), "Заявка отложена");

		/// <summary>
		/// Шаблон темы письма "Возвращена заказчику"
		/// </summary>
		public static EmailTemplateSubject Returned = new(8, nameof(Returned), "Заявка возвращена");

		public EmailTemplateSubject(int id, string name, string description) : base(id, name, description)
		{
		}

	}
}
