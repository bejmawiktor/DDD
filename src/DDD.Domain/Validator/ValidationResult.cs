using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Domain.Model;

namespace DDD.Domain.Validator;

public struct ValidationSuccess { }

public struct ValidationFailure { }

public abstract class ValidationResult : ValueObject
{
    public static ValidationSuccess Success { get; } = new ValidationSuccess();
    public static ValidationFailure Failure { get; } = new ValidationFailure();

    protected ValidationResult() { }
}

public class ValidationResult<TResult>
    : ValidationResult,
        IEquatable<ValidationSuccess>,
        IEquatable<ValidationFailure>
{
    public TResult? Result { get; }
    public IEnumerable<Exception>? Exceptions { get; }

    private bool IsSuccess => this.Exceptions is null;

    internal ValidationResult(TResult result)
    {
        this.Result = result;
    }

    internal ValidationResult(IEnumerable<Exception> exceptions)
    {
        ValidationResult<TResult>.ValidateExceptions(exceptions);

        this.Exceptions = exceptions;
    }

    private static void ValidateExceptions(IEnumerable<Exception> exceptions)
    {
        ArgumentNullException.ThrowIfNull(exceptions);

        if (!exceptions.Any())
        {
            throw new ArgumentException("Empty exceptions given.");
        }
    }

    private ValidationResult() { }

    public bool Equals(ValidationSuccess other) => this.IsSuccess;

    public static bool operator ==(
        ValidationResult<TResult> validationResult,
        ValidationFailure other
    ) => validationResult.Equals(other);

    public static bool operator !=(
        ValidationResult<TResult> validationResult,
        ValidationFailure other
    ) => !(validationResult == other);

    public bool Equals(ValidationFailure other) => !this.IsSuccess;

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Exceptions;
        yield return this.Result;
    }

    public static bool operator ==(
        ValidationResult<TResult> validationResult,
        ValidationSuccess other
    ) => validationResult.Equals(other);

    public static bool operator !=(
        ValidationResult<TResult> validationResult,
        ValidationSuccess other
    ) => !(validationResult == other);

    public override bool Equals(object? obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
}
