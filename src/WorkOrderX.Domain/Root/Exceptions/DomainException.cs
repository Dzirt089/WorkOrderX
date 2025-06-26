namespace WorkOrderX.Domain.Root.Exceptions
{
	/// <summary>
	/// Общее исключение доменной модели
	/// TODO: лучше использовать определенные исключения для конкретных случаев
	/// </summary>
	[Serializable]
	public class DomainException : Exception
	{
		public DomainException()
		{
		}
		public DomainException(string message)
			: base(message)
		{
		}
		public DomainException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	/// <summary>
	/// Общее исключение доменной сервисной модели
	/// TODO: лучше использовать определенные исключения для конкретных случаев
	/// </summary>
	[Serializable]
	public class DomainServiceException : Exception
	{
		public DomainServiceException()
		{
		}
		public DomainServiceException(string message)
			: base(message)
		{
		}
		public DomainServiceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
