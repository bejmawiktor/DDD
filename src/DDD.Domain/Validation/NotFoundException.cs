using System;

namespace DDD.Domain.Validation;

public class NotFoundException : Exception
{
    public NotFoundException(string? message)
        : base(message)
    {
        NotFoundException.ValidateMessage(message);
    }

    private static void ValidateMessage(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message == string.Empty)
        {
            throw new ArgumentException("Empty message given.");
        }
    }
}
