namespace Application.Commands.Mediator;

using Application.Commands.Interfaces;

public interface IMediator
{
    Task<TResponse> Send<TCommand, TResponse>(TCommand command) where TCommand : ICommand<TResponse>;
}
