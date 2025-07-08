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

		public static EquipmentModel Parse(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new EnumerationValueNotFoundException("Name is null or empty");

			var match = GetAll<EquipmentModel>()
				.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))
				?? throw new EnumerationValueNotFoundException($"Unknown equipment type name '{name}'");

			return match;
		}

	}
}
