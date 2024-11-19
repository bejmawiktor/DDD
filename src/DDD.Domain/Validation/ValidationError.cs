using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

public class ValidationError<TException> : Error<TException>, IValidationError<TException>
    where TException : Exception
{
    public ValidationError(string message)
        : base(message) { }

    public ValidationError(IEnumerable<TException> exceptions)
        : base()
    {
        ValidateExceptions(exceptions);

        this.Message = ValidationError<TException>.FormatMessage(exceptions);
        this.Reasons = exceptions;
    }

    internal static void ValidateExceptions(IEnumerable<TException> exceptions)
    {
        ArgumentNullException.ThrowIfNull(exceptions);

        if (!exceptions.Any())
        {
            throw new ArgumentException("Empty exceptions given.");
        }
    }

    internal static string FormatMessage(IEnumerable<TException> exceptions)
    {
        if (exceptions.Count() == 1)
        {
            return exceptions.First().Message;
        }

        StringBuilder messagesBuilder = new();

        foreach (Exception exception in exceptions)
        {
            _ = messagesBuilder.AppendLine($"  - {exception.Message}");
        }

        string message = messagesBuilder.ToString();

        return $"""
            Multiple errors found:
            {message[..^Environment.NewLine.Length]}
            """;
    }
}

public class ValidationError : ValidationError<ValidationException>
{
    public ValidationError(string message)
        : base(message) { }

    public ValidationError(IEnumerable<ValidationException> exceptions)
        : base(exceptions) { }
}
