using MediatR;

using WorkOrderX.Application.Queries.GetEmployeeByAccount.Responses;

namespace WorkOrderX.Application.Queries.GetEmployeeByAccount
{
	public record GetEmployeeByAccountQuery : IRequest<GetEmployeeByAccountQueryResponse>
	{
		/// <summary>
		/// Учетная запись компьютера, с которого запрашивают инфу об сотруднике, у которого имеется эта учётная запись
		/// </summary>
		public string Account { get; set; }
	}
}
