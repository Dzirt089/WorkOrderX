using System.Net.Http;
using System.Net.Http.Headers;

using WorkOrderX.WPF.Models.Model.Global;

namespace WorkOrderX.WPF.InternalServices
{
	/// <summary>
	/// Класс для обработки заголовка авторизации в HTTP-запросах.
	/// </summary>
	public class AuthHeaderHandler : DelegatingHandler
	{
		/// <summary>
		/// Глобальный объект сотрудника, содержащий токен авторизации.
		/// </summary>
		private readonly GlobalEmployeeForApp _globalEmployee;

		/// <summary>
		/// Конструктор класса AuthHeaderHandler, принимающий глобального сотрудника.
		/// </summary>
		/// <param name="globalEmployee"></param>
		public AuthHeaderHandler(GlobalEmployeeForApp globalEmployee)
		{
			_globalEmployee = globalEmployee;
		}

		/// <summary>
		/// Метод для отправки HTTP-запроса с добавлением заголовка авторизации.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			// Если есть токен, добавляем его в заголовки
			if (!string.IsNullOrEmpty(_globalEmployee.Token))
			{
				// Установка заголовка авторизации с типом Bearer и токеном
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _globalEmployee.Token);
			}

			// Отправка запроса с добавленным заголовком авторизации
			return await base.SendAsync(request, cancellationToken);
		}
	}
}