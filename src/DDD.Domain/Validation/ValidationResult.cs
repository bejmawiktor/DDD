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
}

public class ValidationResult<TValue, TException> : Result<TValue, ValidationError<TException>>
    where TException : Exception
{
    public IEnumerable<TException>? Exceptions => this.Error?.Reasons;

    internal ValidationResult(TValue value)
        : base(value) { }

    internal ValidationResult(IEnumerable<TException> exceptions)
        : base(new ValidationError<TException>(exceptions)) { }

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return base.GetEqualityMembers();
        yield return this.Value;
    }
}
