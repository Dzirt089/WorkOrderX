namespace WorkOrderX.Application.Models.DTOs
{
	public record ApplicationStatusDataDto
	{
		public int Id { get; init; }
		public string Name { get; init; }
		public string Description { get; init; }
	}
}
