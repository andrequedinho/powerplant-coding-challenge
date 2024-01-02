namespace Infrastructure.CrossCutting.Exceptions;

using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

public class CustomValidationException : ValidationException
{
    public CustomValidationException(int statusCode, IEnumerable<ValidationFailure> errors)
        : base(errors)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; private set; }
}