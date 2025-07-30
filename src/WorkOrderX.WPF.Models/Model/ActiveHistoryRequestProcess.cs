using CommunityToolkit.Mvvm.ComponentModel;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	/// <summary>
	/// Заявка, для отображения в истории и активных заявок
	/// </summary>
	public partial class ActiveHistoryRequestProcess : ViewModelBase
	{
		/// <summary>
		/// Идентификатор заявки
		/// </summary>
		[ObservableProperty]
		private Guid? _id;

		/// <summary>
		/// Номер заявки
		/// </summary>
		[ObservableProperty]
		private long _applicationNumber;

		/// <summary>
		/// Тип заявки
		/// </summary>
		[ObservableProperty]
		private string _applicationType;

		/// <summary>
		/// Дата и время создания заявки
		/// </summary>
		[ObservableProperty]
		private DateTime _createdAt;

		/// <summary>
		/// Дата и время, когда планируется исполнение заявки
		/// </summary>
		[ObservableProperty]
		private DateTime _plannedAt;

		/// <summary>
		/// Дата и время, когда заявка была обновлена
		/// </summary>
		[ObservableProperty]
		private DateTime? _updatedAt;

		/// <summary>
		/// Дата и время, когда заявка была исполнена
		/// </summary>
		[ObservableProperty]
		private string? _completionAt;

		/// <summary>
		/// Важность заявки
		/// </summary>
		[ObservableProperty]
		private string? _importance;

		/// <summary>
		/// Статус заявки
		/// </summary>
		[ObservableProperty]
		private string _applicationStatus;

		/// <summary>
		/// Заказчик, сотрудник создавший заявку
		/// </summary>
		[ObservableProperty]
		private Employee _customerEmployee;

		/// <summary>
		/// Исполнитель, сотрудник назначенный на исполнение заявки
		/// </summary>
		[ObservableProperty]
		private Employee _executorEmployee;
	}
}
