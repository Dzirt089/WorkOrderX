using MailerVKT;

using WorkOrderX.Application.Services.Email.Interfaces;

namespace WorkOrderX.Application.Services.Email.Implementation
{
	public class MailService(Sender sender) : IMailService
	{
		public async Task SendMailAsync(MailParameters parameters)
		{
			await sender.SendAsync(parameters);
		}
	}
}
