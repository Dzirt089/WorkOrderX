using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	/// <summary>
	/// Учетная запись сотрудника/рабочего места
	/// </summary>
	public class Account : ValueObject
	{
		public string Value { get; }

		private Account(string account)
		{
			Value = account;
		}

		public static Account Create(string account)
		{
			if (string.IsNullOrEmpty(account))
			{
				throw new DomainException($"Учетная запись сотрудника не должен быть пустой! {nameof(account)}");
			}
			return new Account(account);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
