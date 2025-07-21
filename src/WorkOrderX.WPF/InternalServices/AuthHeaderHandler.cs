using System.Net.Http;
using System.Net.Http.Headers;

using WorkOrderX.WPF.Models.Model.Global;

namespace WorkOrderX.WPF.InternalServices
{
	// Добавляем новый класс-обработчик
	public class AuthHeaderHandler : DelegatingHandler
	{
		private readonly GlobalEmployeeForApp _globalEmployee;

		public AuthHeaderHandler(GlobalEmployeeForApp globalEmployee)
		{
			_globalEmployee = globalEmployee;
		}

		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			// Если есть токен, добавляем его в заголовки
			if (!string.IsNullOrEmpty(_globalEmployee.Token))
			{
				request.Headers.Authorization =
					new AuthenticationHeaderValue("Bearer", _globalEmployee.Token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}