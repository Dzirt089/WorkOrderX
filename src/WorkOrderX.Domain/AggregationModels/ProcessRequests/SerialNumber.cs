using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	public class SerialNumber : ValueObject
	{
		public string Value { get; }

		// Приватный конструктор без параметров для EF
		private SerialNumber() { }

		private SerialNumber(string text)
		{
			Value = text;
		}

		public static SerialNumber Create(string text) =>
			string.IsNullOrWhiteSpace(text) ? new SerialNumber(string.Empty)
				: new SerialNumber(text);


		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
