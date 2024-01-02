namespace Application.Commands.Interfaces;

public interface ICommandHandler<TCommand, TReponse> where TCommand : ICommand<TReponse>
{
    Task<TReponse> Handle(TCommand command);
}
