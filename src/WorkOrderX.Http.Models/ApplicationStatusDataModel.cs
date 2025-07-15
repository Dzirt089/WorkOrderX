namespace WorkOrderX.Http.Models
{
	public record ApplicationStatusDataModel
	{
		public int Id { get; init; }
		public string Name { get; init; }
		public string Description { get; init; }
	}
}
