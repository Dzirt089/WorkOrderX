using MailerVKT;

namespace WorkOrderX.Application.Services.Email.Interfaces
{
	public interface IMailService
	{
		Task SendMailAsync(MailParameters parameters);
	}
}
