using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	/// <summary>
	/// Тут будет "Мастер станочного участка"/"Мастер 049 уч."
	/// </summary>
	public class Name : ValueObject
	{
		public string Value { get; }

		// Приватный конструктор без параметров для EF
		private Name() { }

		private Name(string name)
		{
			Value = name;
		}

		public static Name Create(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new DomainException($"Имя сотрудника не должно быть пустым! {nameof(name)}");
			}
			return new Name(name);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
