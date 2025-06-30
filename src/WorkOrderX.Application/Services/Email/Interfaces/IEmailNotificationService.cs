using WorkOrderX.Domain.Models.Email;

namespace WorkOrderX.Application.Services.Email.Interfaces
{
	public interface IEmailNotificationService
	{
		Task SendStatusChangeEmailAsync(StatusChangeEmailParams emailParams, CancellationToken token);
	}
}
