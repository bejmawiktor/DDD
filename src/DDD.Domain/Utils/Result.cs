using System;
using System.Collections.Generic;
using DDD.Domain.Model;

namespace DDD.Domain.Utils;

public static class Result
{
    public struct SuccessResult { }

    public struct FailureResult { }

    public static SuccessResult Success { get; } = new();
    public static FailureResult Failure { get; } = new();
}

public class Result<TError> : ValueObject, IResult<TError>
    where TError : IError
{
    public TError? Error { get; }

    public bool IsFailure => !this.IsSuccess;

    public bool IsSuccess { get; }

    public Result(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        this.Error = error;
        this.IsSuccess = false;
    }

    public Result()
    {
        this.Error = default;
        this.IsSuccess = true;
    }

    public bool Equals(Result.SuccessResult other) => this.IsSuccess;

    public static bool operator ==(Result<TError> result, Result.FailureResult other) =>
        result.Equals(other);

    public static bool operator !=(Result<TError> result, Result.FailureResult other) =>
        !(result == other);

    public bool Equals(Result.FailureResult other) => !this.IsSuccess;

    public static bool operator ==(Result<TError> result, Result.SuccessResult other) =>
        result.Equals(other);

    public static bool operator !=(Result<TError> result, Result.SuccessResult other) =>
        !(result == other);

    public static implicit operator Result<TError>(TError error) => new(error);

    public TMatchResult Match<TMatchResult>(
        Func<TMatchResult> onSuccess,
        Func<TError, TMatchResult> onFailure
    )
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return this.IsSuccess ? onSuccess() : onFailure(this.Error!);
    }

    public override bool Equals(object? obj) => base.Equals(obj);

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Error;
    }

    public override int GetHashCode() => base.GetHashCode();
}

public class Result<TValue, TError> : Result<TError>, IResult<TValue, TError>
    where TError : IError
{
    public TValue? Value { get; }

    public Result(TValue value)
        : base()
    {
        this.Value = value;
    }

    public Result(TError error)
        : base(error)
    {
        this.Value = default;
    }

    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public void Deconstruct(out TValue? value, out TError? error)
    {
        value = this.Value;
        error = this.Error;
    }

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return base.GetEqualityMembers();
        yield return this.Value;
    }

    public TMatchResult Match<TMatchResult>(
        Func<TValue, TMatchResult> onSuccess,
        Func<TError, TMatchResult> onFailure
    )
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return this.IsSuccess ? onSuccess(this.Value!) : onFailure(this.Error!);
    }
}
