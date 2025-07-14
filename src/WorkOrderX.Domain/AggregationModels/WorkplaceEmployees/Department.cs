using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	/// <summary>
	/// Участок на котором сотрудник работает
	/// </summary>
	public class Department : ValueObject
	{
		public string Value { get; }

		// Приватный конструктор без параметров для EF
		private Department() { }

		private Department(string department)
		{
			Value = department;
		}

		public static Department Create(string? department) =>
			department is null ? new Department(string.Empty)
				: new Department(department);


		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
