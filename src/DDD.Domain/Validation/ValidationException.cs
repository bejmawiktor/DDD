using System;

namespace DDD.Domain.Validation;

public class ValidationException : Exception
{
    public string? FieldName { get; }

    public ValidationException(string message)
        : base(message)
    {
        ValidationException.ValidateMessage(message);
    }

    private static void ValidateMessage(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message == string.Empty)
        {
            throw new ArgumentException("Empty message given.");
        }
    }

    public ValidationException(string fieldName, string message)
        : base(message)
    {
        ValidationException.ValidateMessage(message);
        ValidationException.ValidateFieldName(fieldName);

        this.FieldName = fieldName;
    }

    private static void ValidateFieldName(string fieldName)
    {
        ArgumentNullException.ThrowIfNull(fieldName);

        if (fieldName == string.Empty)
        {
            throw new ArgumentException("Empty field name given.");
        }
    }

    public override string ToString() =>
        this.FieldName is not null
            ? $"{typeof(ValidationException).FullName}: {this.FieldName}. {this.Message}"
            : base.ToString();
}
