using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Уровень важности заявки
	/// </summary>
	public class Importance : Enumeration
	{
		/// <summary>
		/// Обычный уровень важности заявки
		/// </summary>
		public readonly static Importance Normal = new(1, nameof(Normal), "Обычная");

		/// <summary>
		/// Высокий уровень важности заявки
		/// </summary>
		public readonly static Importance High = new(2, nameof(High), "Высокая");

		///// <summary>
		///// Критический уровень важности заявки
		///// </summary>
		//public readonly static Importance Critical = new(3, nameof(Critical), "Критический");

		// Приватный конструктор без параметров для EF
		private Importance() { }

		public Importance(int id, string name, string descriptions) : base(id, name, descriptions) { }
	}
}
