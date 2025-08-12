using WorkOrderX.Domain.Root;

namespace WorkOrderX.Domain.AggregationModels.ProcessRequests
{
	/// <summary>
	/// Модель инструмента
	/// </summary>
	public class InstrumentModel : Enumeration
	{
		/// <summary>
		/// Модель инструмента неизвестна\не указана в списке
		/// </summary>
		public readonly static InstrumentModel Other = new InstrumentModel(1, nameof(Other), "Другое");


		// Приватный конструктор без параметров для EF
		private InstrumentModel() { }

		//TODO: Пришлют список - заполнить.
		public InstrumentModel(int id, string name, string descriptions) : base(id, name, descriptions) { }
	}
}
