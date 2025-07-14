using MailerVKT;

using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace WorkOrderX.API.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILogger<ExceptionHandlingMiddleware> logger,
			IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");
				var _mailService = context.RequestServices.GetRequiredService<Sender>();
				await _mailService.SendAsync(new MailParameters
				{
					Text = TextMail(ex),
					Recipients = ["teho19@vkt-vent.ru" /*, "teho12@vkt-vent.ru"*/],
					Subject = "Errors in WorkOrderX.API",
					SenderName = "WorkOrderX.API",
				});
				await HandleExceptionAsync(context, ex);
			}
		}

		private static string TextMail(Exception ex)
		{
			return $@"
<pre>
WorkOrderX.API,
Время: {DateTime.Now},
Обработка исключений в промежуточном программном обеспечении (Middleware).


Сводка об ошибке: 

Message: {ex.Message}.


StackTrace: {ex.StackTrace}.


Source: {ex.Source}.


InnerException: {ex?.InnerException}.

</pre>";
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			// Логируем с контекстом запроса
			_logger.LogError(exception, "Request failed: {Method} {Path}",
				context.Request.Method, context.Request.Path);

			// Защита от записи в уже начатый ответ
			if (context.Response.HasStarted)
			{
				_logger.LogWarning("Cannot write error response - response already started");
				return;
			}

			// Определение HTTP-статуса в зависимости от типа исключения
			var statusCode = exception switch
			{
				ArgumentNullException => StatusCodes.Status400BadRequest,
				ValidationException => StatusCodes.Status422UnprocessableEntity,
				UnauthorizedAccessException => StatusCodes.Status403Forbidden,
				_ => StatusCodes.Status500InternalServerError
			};

			// Формируем объект ошибки JSON-ответа
			var problem = new ProblemDetails
			{
				Title = "An error occurred",
				Status = statusCode,
				Instance = context.Request.Path,
				Extensions = { ["traceId"] = context.TraceIdentifier } // ID для отслеживания
			};

			// Детали в зависимости от среды
			if (_env.IsDevelopment())
			{
				problem.Detail = exception.ToString();
				problem.Extensions["stack"] = exception.StackTrace; // Отдельно стек
			}
			else
			{
				problem.Detail = "Please contact support";
			}

			// Дополнительные расширения для специфических исключений
			if (exception is ValidationException validationEx)
			{
				problem.Extensions["errors"] = validationEx.ValidationResult.ErrorMessage; // Детали валидации
			}

			// Записываем ответ
			context.Response.ContentType = "application/problem+json";
			context.Response.StatusCode = statusCode;
			await context.Response.WriteAsJsonAsync(problem);
		}
	}
}
