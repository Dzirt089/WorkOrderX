using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.Employees
{
	public class Specialized : Enumeration
	{
		/// <summary>
		/// Электрик
		/// </summary>
		public static Specialized Electrician = new Specialized(1, nameof(Electrician));

		/// <summary>
		/// Механик
		/// </summary>
		public static Specialized Mechanic = new Specialized(2, nameof(Mechanic));

		/// <summary>
		/// Слесарь-сантехник
		/// </summary>
		public static Specialized Plumber = new Specialized(3, nameof(Plumber));

		/// <summary>
		/// Плотник-столяр
		/// </summary>
		public static Specialized Carpenter = new Specialized(4, nameof(Carpenter));

		public Specialized(int id, string name) : base(id, name)
		{
		}

		public static Specialized Parse(string name) => name?.ToLower() switch
		{
			"electrician" => Electrician,
			"mechanic" => Mechanic,
			"plumber" => Plumber,
			"carpenter" => Carpenter,
			_ => throw new EnumerationValueNotFoundException($"Unknown specialized name {nameof(name)}")
		};
	}
}
