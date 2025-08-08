using MediatR;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace WorkOrderX.Application.Behaviors
{
	public class LoggingBehavior<TIn, TOut> : IPipelineBehavior<TIn, TOut>
		where TIn : IRequest<TOut>
	{

		private readonly ILogger<LoggingBehavior<TIn, TOut>> _logger;

		public LoggingBehavior(ILogger<LoggingBehavior<TIn, TOut>> logger)
		{
			_logger = logger;
		}

		public async Task<TOut> Handle(TIn request, RequestHandlerDelegate<TOut> next, CancellationToken cancellationToken)
		{
			var requestId = Guid.NewGuid();
			var requestType = request.GetType().Name;

			_logger.LogDebug("Выполняется операция {OperationType} с ID {OperationId} и телом запроса: {Request}",
				requestType, requestId, TrySerializeResult(request));

			var result = await next();

			_logger.LogDebug("Тело ответа {Response}.", TrySerializeResult(result));

			return result;
		}

		private static string TrySerializeResult(object? request)
		{
			if (request == null)
			{
				return "Возвращаемого значения нет";
			}
			try
			{
				return JsonConvert.SerializeObject(request);
			}
			catch
			{
				return "ошибка сериализации данных";
			}
		}
	}
}
