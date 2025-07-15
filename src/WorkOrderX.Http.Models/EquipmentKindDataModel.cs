namespace WorkOrderX.Http.Models
{
	public record EquipmentKindDataModel
	{
		public int Id { get; init; }
		public string Name { get; init; }

		public EquipmentTypeDataModel Type { get; init; }
		public string Description { get; init; }
	}
}
