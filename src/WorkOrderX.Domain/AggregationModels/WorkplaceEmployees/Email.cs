﻿using System.Text.RegularExpressions;

using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	/// <summary>
	/// Почта сотрудника/рабочего места
	/// </summary>
	public class Email : ValueObject
	{
		/// <summary>
		/// Метод создания Email с проверкой валидности
		/// </summary>
		/// <param name="emailString"></param>
		/// <returns></returns>
		/// <exception cref="DomainException"></exception>
		public static Email Create(string emailString)
		{
			if (IsValidEmail(emailString))
			{
				return new Email(emailString);
			}

			//TODO: сделать конечно нормальное исключение.
			throw new DomainException($"Email is invalid: {emailString}");
		}

		/// <summary>
		/// Значение email
		/// </summary>
		public string Value { get; }

		// Приватный конструктор без параметров для EF
		private Email() { }

		/// <summary>
		/// Приватный конструктор Email
		/// </summary>
		/// <param name="emailString"></param>
		private Email(string emailString) => Value = emailString;

		/// <summary>
		/// Переопределение ToString для удобного отображения
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Value;

		/// <summary>
		/// Проверка валидности email
		/// </summary>
		/// <param name="emailString"></param>
		/// <returns></returns>
		private static bool IsValidEmail(string emailString) =>
			Regex.IsMatch(emailString, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
