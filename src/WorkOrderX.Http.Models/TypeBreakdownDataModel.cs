namespace WorkOrderX.Http.Models
{
	public record TypeBreakdownDataModel
	{
		public int Id { get; init; }
		public string Name { get; init; }

		public EquipmentTypeDataModel Type { get; init; }
		public string Description { get; init; }
	}
}
