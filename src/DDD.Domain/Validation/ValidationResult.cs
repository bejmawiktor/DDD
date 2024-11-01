using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Domain.Model;

namespace DDD.Domain.Validation;

public struct ValidationSuccess { }

public struct ValidationFailure { }

public class ValidationResult
    : ValueObject,
        IEquatable<ValidationSuccess>,
        IEquatable<ValidationFailure>
{
    public static ValidationSuccess Success { get; } = new ValidationSuccess();
    public static ValidationFailure Failure { get; } = new ValidationFailure();

    public IEnumerable<Exception>? Exceptions { get; }

    protected bool IsSuccess => this.Exceptions is null;

    internal ValidationResult(IEnumerable<Exception> exceptions)
    {
        ValidationResult.ValidateExceptions(exceptions);

        this.Exceptions = exceptions;
    }

    internal ValidationResult() { }

    private static void ValidateExceptions(IEnumerable<Exception> exceptions)
    {
        ArgumentNullException.ThrowIfNull(exceptions);

        if (!exceptions.Any())
        {
            throw new ArgumentException("Empty exceptions given.");
        }
    }

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Exceptions;
    }

    public bool Equals(ValidationSuccess other) => this.IsSuccess;

    public static bool operator ==(ValidationResult validationResult, ValidationFailure other) =>
        validationResult.Equals(other);

    public static bool operator !=(ValidationResult validationResult, ValidationFailure other) =>
        !(validationResult == other);

    public bool Equals(ValidationFailure other) => !this.IsSuccess;

    public static bool operator ==(ValidationResult validationResult, ValidationSuccess other) =>
        validationResult.Equals(other);

    public static bool operator !=(ValidationResult validationResult, ValidationSuccess other) =>
        !(validationResult == other);

    public override bool Equals(object? obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
}

public class ValidationResult<TResult> : ValidationResult
{
    public TResult? Result { get; }

    internal ValidationResult(TResult result)
        : base()
    {
        this.Result = result;
    }

    internal ValidationResult(IEnumerable<Exception> exceptions)
        : base(exceptions) { }

    public void Deconstruct(out TResult? result, out IEnumerable<Exception>? exceptions)
    {
        result = this.Result;
        exceptions = this.Exceptions;
    }

    public TMatchResult Match<TMatchResult>(
        Func<TResult, TMatchResult> onSuccess,
        Func<IEnumerable<Exception>, TMatchResult> onFailure
    )
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return this.IsSuccess ? onSuccess(this.Result!) : onFailure(this.Exceptions!);
    }

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return base.GetEqualityMembers();
        yield return this.Result;
    }
}
