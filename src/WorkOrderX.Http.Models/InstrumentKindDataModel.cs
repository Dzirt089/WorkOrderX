namespace WorkOrderX.Http.Models
{
	public record InstrumentKindDataModel
	{
		public int Id { get; init; }
		public string Name { get; init; }

		public InstrumentTypeDataModel Type { get; init; }
		public string Description { get; init; }
	}
}
