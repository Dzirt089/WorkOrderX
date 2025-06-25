using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Модель оборудования
	/// </summary>
	public class EquipmentModel : Enumeration
	{
		/// <summary>
		/// Модель оборудования неизвестна
		/// </summary>
		public static readonly EquipmentModel Unknown = new EquipmentModel(1, nameof(Unknown));

		//TODO: Пришлют список - заполнить.
		public EquipmentModel(int id, string name) : base(id, name)
		{
		}

		public static EquipmentModel Parse(string name) => name?.ToLower() switch
		{
			"unknown" => Unknown,
			_ => throw new DomainException("Unknown equipment type name"),
		};
	}
}
