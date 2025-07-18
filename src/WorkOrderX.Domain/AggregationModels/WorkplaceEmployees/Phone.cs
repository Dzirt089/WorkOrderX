﻿using WorkOrderX.Domain.Root;
using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.AggregationModels.WorkplaceEmployees
{
	/// <summary>
	/// Номер телефона сотрудника/рабочего места
	/// </summary>
	public class Phone : ValueObject
	{
		public string Value { get; }

		// Приватный конструктор без параметров для EF
		private Phone() { }

		private Phone(string phone)
		{
			Value = phone;
		}

		public static Phone Create(string phone)
		{
			if (string.IsNullOrEmpty(phone))
			{
				throw new DomainException($"Номер телефона сотрудника не должен быть пустым! {nameof(phone)}");
			}
			return new Phone(phone);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
