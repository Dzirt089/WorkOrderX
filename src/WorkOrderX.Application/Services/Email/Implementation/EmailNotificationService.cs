using System.Text.RegularExpressions;

using WorkOrderX.Application.Services.Email.Interfaces;
using WorkOrderX.Domain.AggregationModels.Employees;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.Models.Email;

namespace WorkOrderX.Application.Services.Email.Implementation
{
	/// <summary>
	/// Сервис для отправки уведомлений по электронной почте.
	/// </summary>
	public class EmailNotificationService : IEmailNotificationService
	{
		private readonly IProcessRequestRepository _processRequestRepository;
		private readonly IWorkplaceEmployeesRepository _workplaceEmployeesRepository;
		private readonly IMailService _mailService;

		private static readonly Regex TokenRegex = new(@"\{([^{}]+)\}", RegexOptions.Compiled);

		/// <summary>
		/// Конструктор сервиса уведомлений по электронной почте.
		/// </summary>
		/// <param name="processRequestRepository"></param>
		/// <param name="workplaceEmployeesRepository"></param>
		/// <param name="mailService"></param>
		public EmailNotificationService(IProcessRequestRepository processRequestRepository, IWorkplaceEmployeesRepository workplaceEmployeesRepository, IMailService mailService)
		{
			_processRequestRepository = processRequestRepository;
			_workplaceEmployeesRepository = workplaceEmployeesRepository;
			_mailService = mailService;
		}

		/// <summary>
		/// Отправляет письмо о смене статуса заявки.
		/// </summary>
		/// <param name="emailParams"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task SendStatusChangeEmailAsync(StatusChangeEmailParams emailParams, CancellationToken token)
		{
			var (request, customer, executor) = await GetProcessRequestEmployeesDataAsync(emailParams, token);
			EmailTemplate template = CreateEmailTemplateFromStatus(emailParams);

			var filledTemplate = FillTemplate(template.Body, new
			{
				executor = executor,
				request = request,
				customer = customer,
				notification = emailParams
			});
			template.Body = filledTemplate;

			var (recipients, recipientsCopy) = DetermineRecipients(emailParams.NewStatus, customer, executor);

			await _mailService.SendMailAsync(new MailerVKT.MailParameters
			{
				Recipients = recipients,
				RecipientsCopy = recipientsCopy,
				Subject = template.Subject,
				Text = template.Body,
				RecipientsBcc = ["progto@vkt-vent.ru"],
				SenderName = "WorkOrderX",
			});
		}

		/// <summary>
		/// Определяет получателей письма в зависимости от нового статуса заявки.
		/// </summary>
		/// <param name="newStatus"></param>
		/// <param name="customer"></param>
		/// <param name="executor"></param>
		/// <returns></returns>
		private (List<string> recipients, List<string> recipientsCopy) DetermineRecipients(ApplicationStatus newStatus, WorkplaceEmployees customer, WorkplaceEmployees executor)
		{
			return newStatus.Name switch
			{
				nameof(ApplicationStatus.New) or
				nameof(ApplicationStatus.Redirected) or
				nameof(ApplicationStatus.Changed) => (
				recipients: [executor.Email.Value],
				recipientsCopy: [customer.Email.Value]
				),

				nameof(ApplicationStatus.InWork) or
				nameof(ApplicationStatus.Rejected) or
				nameof(ApplicationStatus.Done) or
				nameof(ApplicationStatus.Postponed) or
				nameof(ApplicationStatus.Returned) => (
				recipients: [customer.Email.Value],
				recipientsCopy: [executor.Email.Value]
				),

				_ => (
				recipients: [customer.Email.Value],
				recipientsCopy: [executor.Email.Value]
				)
			};
		}

		/// <summary>
		/// Создает шаблон письма на основе статуса заявки.
		/// </summary>
		/// <param name="emailParams"></param>
		/// <returns></returns>
		private static EmailTemplate CreateEmailTemplateFromStatus(StatusChangeEmailParams emailParams)
		{
			EmailTemplate template = new();
			template.Name = emailParams.NewStatus.Name;
			template.Subject = EmailTemplateSubject.Parse(emailParams.NewStatus.Name).Name;
			template.Body = EmailTemplateBody.Parse(emailParams.NewStatus.Name).Name;
			return template;
		}

		/// <summary>
		/// Получает данные заявки и сотрудников (заказчика и исполнителя) для отправки письма.
		/// </summary>
		/// <param name="emailParams"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		private async Task<(ProcessRequest request, WorkplaceEmployees customer, WorkplaceEmployees executor)> GetProcessRequestEmployeesDataAsync(StatusChangeEmailParams emailParams, CancellationToken token)
		{
			//Находим заявку
			var requestResult = _processRequestRepository.GetByIdAsync(emailParams.RequestId, token);

			//Находим заказчика
			var customerResult = _workplaceEmployeesRepository.GetByIdAsync(emailParams.CustomerEmployeeId, token);

			//Находим исполнителя
			var executorResult = _workplaceEmployeesRepository.GetByIdAsync(emailParams.ExecutorEmployeeId, token);

			await Task.WhenAll(requestResult, customerResult, executorResult);
			return (requestResult.Result, customerResult.Result, executorResult.Result);
		}

		/// <summary>
		/// Заполняет шаблон письма данными из модели.
		/// </summary>
		/// <param name="template"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		private static string FillTemplate(string template, object model)
		{
			return TokenRegex.Replace(template, match =>
			{
				var path = match.Groups[1].Value.Split('.');
				object? current = model;


				foreach (var item in path)
				{
					if (current is null) return string.Empty;

					//var prop = current.GetType().GetProperty(item, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

					var prop = current.GetType().GetProperty(item);
					if (prop is null) return string.Empty;
					current = prop.GetValue(current);
				}

				return current?.ToString() ?? string.Empty;
			});
		}
	}
}
