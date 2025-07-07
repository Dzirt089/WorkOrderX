using MediatR;

using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses;

namespace WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById
{
	/// <summary>
	/// Запрос для получения активных заявок от клиента по идентификатору сотрудника
	/// </summary>
	public record GetActivProcessRequestFromEmployeeByIdQuery : IRequest<GetActivProcessRequestFromEmployeeByIdQueryResponse>
	{
		/// <summary>
		/// Идентификатор сотрудника, для которого запрашиваются активные заявки
		/// </summary>
		public Guid Id { get; init; }
	}
}
