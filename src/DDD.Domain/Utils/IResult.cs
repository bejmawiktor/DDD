using System;

namespace DDD.Domain.Utils;

public interface IResult<out TError>
    : IEquatable<Result.SuccessResult>,
        IEquatable<Result.FailureResult>
    where TError : IError
{
    TError? Error { get; }
    bool IsSuccess { get; }
    bool IsFailure { get; }
}

public interface IResult<out TValue, out TError> : IResult<TError>
    where TError : IError
{
    TValue? Value { get; }
}
