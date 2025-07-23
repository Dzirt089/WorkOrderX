using CommunityToolkit.Mvvm.ComponentModel;

using System.ComponentModel.DataAnnotations;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	public partial class ActiveRequestProcess : ViewModelBase
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
		private string _createdAt;

		/// <summary>
		/// Дата и время, когда планируется исполнение заявки
		/// </summary>
		[ObservableProperty]
		private string _plannedAt;

		/// <summary>
		/// Важность заявки
		/// </summary>
		[ObservableProperty]
		[Required]
		[NotifyDataErrorInfo]
		[CustomValidation(typeof(ProcessRequest), nameof(ValidateValueString))]
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

		public static ValidationResult? ValidateValueString(string valueString, ValidationContext context)
		{
			return string.IsNullOrWhiteSpace(valueString)
				? new ValidationResult("Поле не должно быть пустое. Выберите или введите текст")
				: ValidationResult.Success;
		}
	}
}
