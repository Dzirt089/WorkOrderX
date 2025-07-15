namespace WorkOrderX.Application.Models.DTOs
{
	public record EquipmentKindDataDto
	{
		public int Id { get; init; }
		public string Name { get; init; }

		public EquipmentTypeDataDto Type { get; init; }
		public string Description { get; init; }
	}
}
