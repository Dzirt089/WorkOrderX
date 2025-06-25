using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Тип поломки
	/// </summary>
	public class TypeBreakdown : Enumeration
	{
		/// <summary>
		/// Тип поломки неизвестен
		/// </summary>
		public static readonly TypeBreakdown Unknown = new(1, nameof(Unknown));

		/// <summary>
		/// Тип поломки механический
		/// </summary>
		public static readonly TypeBreakdown Mechanical = new(2, nameof(Mechanical));

		/// <summary>
		/// Тип поломки электрический
		/// </summary>
		public static readonly TypeBreakdown Electrical = new(3, nameof(Electrical));

		/// <summary>
		/// Тип хозяйственные поломки
		/// </summary>
		public static readonly TypeBreakdown РouseholdСhores = new(4, nameof(РouseholdСhores));

		//TODO: Пришлют список - заполнить.
		public TypeBreakdown(int id, string name) : base(id, name)
		{
		}

		public static TypeBreakdown Parse(string name) => name?.ToLower() switch
		{
			"unknown" => Unknown,
			"mechanical" => Mechanical,
			"electrical" => Electrical,
			_ => throw new DomainException("Unknown equipment type name"),
		};
	}
}
