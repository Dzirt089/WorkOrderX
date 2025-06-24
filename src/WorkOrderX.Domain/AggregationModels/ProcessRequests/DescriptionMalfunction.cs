using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Описание неисправности
	/// </summary>
	public class DescriptionMalfunction : ValueObject
	{
		public string Value { get; }

		private DescriptionMalfunction(string text)
		{
			Value = text;
		}

		public static DescriptionMalfunction Create(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new DomainException("Описание заявки не должно быть пустым");
			}
			return new DescriptionMalfunction(text);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
