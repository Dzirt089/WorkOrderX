using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.Models.EmailsTemplates
{
	public class EmailTemplateSubject : Enumeration
	{
		/// <summary>
		/// Шаблон темы письма "Новая"
		/// </summary>
		public static EmailTemplateSubject New = new(1,
			"Зарегистрирована новая заявка на выполнение", "Шаблон темы письма \"Новая\"");

		/// <summary>
		/// Шаблон темы письма "Изменена заказчиком после возврата"
		/// </summary>
		public static EmailTemplateSubject Changed = new(2, "Заявку изменили и отправили на выполнение", "Шаблон темы письма \"Изменена заказчиком после возврата\"");

		/// <summary>
		/// Шаблон темы письма "В работе"
		/// </summary>
		public static EmailTemplateSubject InWork = new(3, "Заявка взята в работу", "Шаблон темы письма \"В работе\"");

		/// <summary>
		/// Шаблон темы письма "Перенаправлена другому исполнителю"
		/// </summary>
		public static EmailTemplateSubject Redirected = new(4, "Заявка перенаправлена", "Шаблон темы письма \"Перенаправлена другому исполнителю\"");

		/// <summary>
		/// Шаблон темы письма "Отклонена"
		/// </summary>
		public static EmailTemplateSubject Rejected = new(5, "Заявка отклонена", "Шаблон темы письма \"Отклонена\"");

		/// <summary>
		/// Шаблон темы письма "Завершена"
		/// </summary>
		public static EmailTemplateSubject Done = new(6, "Заявка выполнена", "Шаблон темы письма \"Завершена\"");

		/// <summary>
		/// Шаблон темы письма "Отложена"
		/// </summary>
		public static EmailTemplateSubject Postponed = new(7, "Заявка отложена", " Шаблон темы письма \"Отложена\"");

		/// <summary>
		/// Шаблон темы письма "Возвращена заказчику"
		/// </summary>
		public static EmailTemplateSubject Returned = new(8, "Заявка возвращена", "Шаблон темы письма \"Возвращена заказчику\"");

		public EmailTemplateSubject(int id, string name, string description) : base(id, name, description)
		{
		}

	}
}
