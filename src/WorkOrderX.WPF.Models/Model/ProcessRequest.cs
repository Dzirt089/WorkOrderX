using CommunityToolkit.Mvvm.ComponentModel;

using System.ComponentModel.DataAnnotations;

using WorkOrderX.WPF.Models.Model.Base;

namespace WorkOrderX.WPF.Models.Model
{
	public partial class ProcessRequest : ViewModelBase
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
		/// Дата и время, когда заявка была исполнена
		/// </summary>
		[ObservableProperty]
		private string? _completionAt;

		/// <summary>
		/// Наименование оборудования, по которому создана заявка
		/// </summary>
		[ObservableProperty]
		[Required]
		[NotifyDataErrorInfo]
		[CustomValidation(typeof(ProcessRequest), nameof(ValidateValueString))]
		private string? _equipmentType;

		/// <summary>
		/// Наименование вида оборудования, по которому создана заявка
		/// </summary>
		[ObservableProperty]
		[Required]
		[NotifyDataErrorInfo]
		[CustomValidation(typeof(ProcessRequest), nameof(ValidateValueString))]
		private string? _equipmentKind;

		/// <summary>
		/// Модель оборудования, по которому создана заявка
		/// </summary>
		[ObservableProperty]
		[Required]
		[NotifyDataErrorInfo]
		[CustomValidation(typeof(ProcessRequest), nameof(ValidateValueString))]
		private string? _equipmentModel;

		/// <summary>
		/// Серийный номер оборудования, по которому создана заявка
		/// </summary>
		[ObservableProperty]
		[Required]
		[NotifyDataErrorInfo]
		[CustomValidation(typeof(ProcessRequest), nameof(ValidateValueString))]
		private string? _serialNumber;

		/// <summary>
		/// Тип поломки, по которому создана заявка
		/// </summary>
		[ObservableProperty]
		[Required]
		[NotifyDataErrorInfo]
		[CustomValidation(typeof(ProcessRequest), nameof(ValidateValueString))]
		private string? _typeBreakdown;

		/// <summary>
		/// Описание поломки, по которому создана заявка
		/// </summary>
		[ObservableProperty]
		[Required]
		[NotifyDataErrorInfo]
		[CustomValidation(typeof(ProcessRequest), nameof(ValidateValueString))]
		private string _descriptionMalfunction;


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
		/// Внутренний комментарий к заявке
		/// </summary>
		[ObservableProperty]
		private string? _internalComment;

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


		//private

		public static ValidationResult? ValidateValueString(string valueString, ValidationContext context)
		{
			return string.IsNullOrWhiteSpace(valueString)
				? new ValidationResult("Поле не должно быть пустое. Выберите или введите текст")
				: ValidationResult.Success;
		}
	}
}
