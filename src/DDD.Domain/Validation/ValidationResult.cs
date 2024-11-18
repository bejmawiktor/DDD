using System;
using System.Collections.Generic;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

public class ValidationResult<TException> : Result<ValidationError<TException>>
    where TException : Exception
{
    public IEnumerable<TException>? Exceptions => this.Error?.Reasons;

    internal ValidationResult(IEnumerable<TException> exceptions)
        : base(new ValidationError<TException>(exceptions)) { }

    internal ValidationResult() { }

    public TMatchResult Match<TMatchResult>(
        Func<TMatchResult> onSuccess,
        Func<IEnumerable<TException>, TMatchResult> onFailure
    )
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return this.IsSuccess ? onSuccess() : onFailure(this.Error?.Reasons!);
    }
}

public class ValidationResult<TValue, TException> : Result<TValue, ValidationError<TException>>
    where TException : Exception
{
    public IEnumerable<TException>? Exceptions => this.Error?.Reasons;

    internal ValidationResult(TValue value)
        : base(value) { }

    internal ValidationResult(IEnumerable<TException> exceptions)
        : base(new ValidationError<TException>(exceptions)) { }

    public void Deconstruct(out TValue? value, out IEnumerable<TException>? exceptions)
    {
        value = this.Value;
        exceptions = this.Error?.Reasons;
    }

    public TMatchResult Match<TMatchResult>(
        Func<TValue, TMatchResult> onSuccess,
        Func<IEnumerable<TException>, TMatchResult> onFailure
    )
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return this.IsSuccess ? onSuccess(this.Value!) : onFailure(this.Error?.Reasons!);
    }

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return base.GetEqualityMembers();
        yield return this.Value;
    }
}
