using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	public class Specialized : Enumeration
	{
		/// <summary>
		/// Отв. за электрохозяйство
		/// </summary>
		public static Specialized Electrician = new Specialized(1, nameof(Electrician), "Отв. за электрохозяйство");

		/// <summary>
		/// Гл. Механик/Механик
		/// </summary>
		public static Specialized Mechanic = new Specialized(2, nameof(Mechanic), "Гл. Механик/Механик");

		/// <summary>
		/// Слесарь-сантехник
		/// </summary>
		public static Specialized Plumber = new Specialized(3, nameof(Plumber), "Слесарь-сантехник");

		/// <summary>
		/// Плотник-столяр
		/// </summary>
		public static Specialized Carpenter = new Specialized(4, nameof(Carpenter), "Плотник-столяр");

		public Specialized(int id, string name, string descriptions) : base(id, name)
		{
			Descriptions = descriptions;
		}

		public string Descriptions { get; }
	}
}
