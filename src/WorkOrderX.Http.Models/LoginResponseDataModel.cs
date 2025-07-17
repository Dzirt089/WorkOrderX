namespace WorkOrderX.Http.Models
{
	public record LoginResponseDataModel
	{
		public string Token { get; init; }

		public EmployeeDataModel Employee { get; init; }
	}
}
