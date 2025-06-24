using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Номер заявки
	/// </summary>
	public class ApplicationNumber : ValueObject
	{
		public long Value { get; }

		private ApplicationNumber(long number)
		{
			Value = number;
		}

		public static ApplicationNumber Create(long number)
		{
			if (number <= 0)
			{
				throw new DomainException("Номер заявки должен быть больше 0");
			}
			return new ApplicationNumber(number);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
