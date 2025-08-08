using MediatR;

namespace WorkOrderX.Application.Commands.Interfaces
{
	public interface ICommand<T> : IRequest<T>
	{
	}
}
