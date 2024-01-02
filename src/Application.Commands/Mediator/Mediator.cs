namespace Application.Commands.Mediator;

using System;
using Application.Commands.Interfaces;
using FluentValidation;
using Infrastructure.CrossCutting.Exceptions;
using Microsoft.AspNetCore.Http;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> Send<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
    {
        ValidateCommand<TCommand, TResult>(command);

        // Find the appropriate handler for the command type
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = (dynamic)_serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler found for {typeof(TCommand)}");
        }

        // Call the handler's Handle method
        return handler.Handle(command);
    }

    private void ValidateCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
    {
        // Find the appropriate validator for the command type
        var validatorType = typeof(IValidator<>).MakeGenericType(typeof(TCommand));
        var validator = (IValidator)_serviceProvider.GetService(validatorType);

        if (validator != null)
        {
            var validationContext = new ValidationContext<TCommand>(command);
            var validationResult = validator.Validate(validationContext);

            if (!validationResult.IsValid)
            {
                throw new CustomValidationException(StatusCodes.Status400BadRequest, validationResult.Errors);
            }
        }
    }
}
