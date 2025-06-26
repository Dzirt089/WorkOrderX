using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Комментарий о заявке, который могут указывать друг другу заказчик/исполнитель.
	/// </summary>
	public class InternalComment : ValueObject
	{
		public string Value { get; }

		private InternalComment(string text)
		{
			Value = text;
		}

		public static InternalComment Create(string text) =>
			string.IsNullOrWhiteSpace(text) ? new InternalComment(string.Empty)
				: new InternalComment(text);


		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
