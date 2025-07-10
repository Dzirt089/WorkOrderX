using System.Reflection;

using WorkOrderX.Domain.Root.Exceptions;

namespace WorkOrderX.Domain.Root
{
	/// <summary>
	/// Класс перечесления вместо типов перечисления
	/// Шаблон взят от сюда https://learn.microsoft.com/ru-ru/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types
	/// </summary>
	public abstract class Enumeration : IComparable
	{
		/// <summary>
		/// Название перечисления
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Идентификатор перечисления
		/// </summary>
		public int Id { get; private set; }

		/// <summary>
		/// Конструктор перечисления
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		protected Enumeration(int id, string name) => (Id, Name) = (id, name);

		protected Enumeration() { }

		/// <summary>
		/// Преобразование в строку
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Name;

		/// <summary>
		/// Получить все значения перечисления
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
			typeof(T).GetFields(BindingFlags.Public |
								BindingFlags.Static |
								BindingFlags.DeclaredOnly)
			.Select(f => f.GetValue(null))
			.Cast<T>();

		/// <summary>
		/// Получить перечисление по идентификатору
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="DomainException"></exception>
		public static T FromId<T>(int id) where T : Enumeration =>
			GetAll<T>().FirstOrDefault(_ => _.Id == id)
			?? throw new DomainException($"Invalid id: {id}");

		/// <summary>
		/// Получить перечисление по имени
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="DomainException"></exception>
		public static T FromName<T>(string name) where T : Enumeration =>
			GetAll<T>().FirstOrDefault(_ => _.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
			?? throw new DomainException($"Invalid name: {name}");

		/// <summary>
		/// Сравнение сущностей
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object? obj)
		{
			if (obj is not Enumeration otherValue) return false;

			var typeMatches = GetType().Equals(obj.GetType());
			var valueMathes = Id.Equals(otherValue.Id);

			return typeMatches && valueMathes;
		}

		/// <summary>
		/// Сравнение сущностей
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
	}
}
