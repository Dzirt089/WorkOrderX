using MediatR;

using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses;

namespace WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById
{
	/// <summary>
	/// Запрос для получения историй заявок от клиента по идентификатору сотрудника
	/// </summary>
	public record GetHistoryProcessRequestFromEmployeeByIdQuery : IRequest<GetHistoryProcessRequestFromEmployeeByIdQueryResponse>
	{
		/// <summary>
		/// Идентификатор сотрудника, для которого запрашиваются активные заявки
		/// </summary>
		public Guid Id { get; init; }
	}
}
