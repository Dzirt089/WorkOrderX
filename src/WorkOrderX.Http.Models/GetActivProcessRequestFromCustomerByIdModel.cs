namespace WorkOrderX.Http.Models
{
	public record GetActivProcessRequestFromCustomerByIdModel
	{
		/// <summary>
		/// Идентификатор сотрудника, для которого запрашиваются активные заявки
		/// </summary>
		public Guid Id { get; init; }
	}
}
