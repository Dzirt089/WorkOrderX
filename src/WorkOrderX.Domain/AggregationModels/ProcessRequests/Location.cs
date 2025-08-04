using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Локация в заявке хоз. работ , который могут указывать друг другу заказчик/исполнитель.
	/// </summary>
	public class Location : ValueObject
	{
		public string Value { get; }

		// Приватный конструктор без параметров для EF
		private Location() { }

		private Location(string text)
		{
			Value = text;
		}

		public static Location Create(string? text) =>
			text is null ? new Location(string.Empty)
				: new Location(text);


		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
