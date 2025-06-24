using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	/// <summary>
	/// Участок на котором сотрудник работает
	/// </summary>
	public class Department : ValueObject
	{
		public string Value { get; }

		private Department(string department)
		{
			Value = department;
		}

		public static Department Create(string department)
		{
			if (string.IsNullOrEmpty(department))
			{
				throw new DomainException($"Участок сотрудника не должен быть пустым! {nameof(department)}");
			}
			return new Department(department);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
