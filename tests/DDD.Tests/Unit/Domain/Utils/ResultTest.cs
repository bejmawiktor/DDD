using System;
using System.Threading.Tasks;
using DDD.Domain.Utils;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Utils;

[TestFixture]
public class ResultTest
{
    [Test]
    public void TestConstructing_WhenErrorGiven_ThenErrorIsSet()
    {
        Error<Exception> error = new("My error");

        Result<Error<Exception>> result = new(error);

        Assert.That(result.Error, Is.EqualTo(error));
    }

    [Test]
    public void TestConstructingWithValue_WhenErrorGiven_ThenErrorIsSetAndValueIsNull()
    {
        Error<Exception> error = new("My error");

        Result<string, Error<Exception>> result = new(error);

        Assert.Multiple(() =>
        {
            Assert.That(result.Error, Is.EqualTo(error));
            Assert.That(result.Value, Is.Null);
        });
    }

    [Test]
    public void TestConstructing_WhenNullErrorGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("error"),
            () => new Result<Error<Exception>>(null!)
        );
    }

    [Test]
    public void TestConstructingWithValue_WhenNullErrorGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("error"),
            () => new Result<string, Error<Exception>>((null as Error<Exception>)!)
        );
    }

    [Test]
    public void TestConstructingWithValue_WhenValueGiven_ThenValueIsSetAndErrorIsNull()
    {
        string value = "my result";

        Result<string, Error<Exception>> result = new(value);

        Assert.Multiple(() =>
        {
            Assert.That(result.Value, Is.EqualTo(value));
            Assert.That(result.Error, Is.Null);
        });
    }

    [Test]
    public void TestConstructingWithValue_WhenErrorGiven_ThenErrorIsSet()
    {
        Error<Exception> error = new("My error");

        Result<int, Error<Exception>> result = new(error);

        Assert.Multiple(() =>
        {
            Assert.That(result.Value, Is.Zero);
            Assert.That(result.Error, Is.EqualTo(error));
        });
    }

    [Test]
    public void TestEquals_WhenErrorIsSet_ThenComparingToFailureIsTrue()
    {
        Error<Exception> error = new("My error");

        Result<Error<Exception>> result = new(error);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(Result.Failure));
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result == Result.Failure, Is.True);
            Assert.That(result, Is.Not.EqualTo(Result.Success));
            Assert.That(result == Result.Success, Is.False);
            Assert.That(result != Result.Failure, Is.False);
            Assert.That(result != Result.Success, Is.True);
        });
    }

    [Test]
    public void TestEquals_WhenErrorIsNotSet_ThenComparingToSuccessIsTrue()
    {
        Result<Error<Exception>> result = new();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(Result.Success));
            Assert.That(result.IsFailure, Is.False);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result == Result.Success, Is.True);
            Assert.That(result, Is.Not.EqualTo(Result.Failure));
            Assert.That(result == Result.Failure, Is.False);
            Assert.That(result != Result.Success, Is.False);
            Assert.That(result != Result.Failure, Is.True);
        });
    }

    [Test]
    public void TestEqualsWithValue_WhenErrorIsSet_ThenComparingToFailureIsTrue()
    {
        Error<Exception> error = new("My error");

        Result<object, Error<Exception>> result = new(error);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(Result.Failure));
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result == Result.Failure, Is.True);
            Assert.That(result, Is.Not.EqualTo(Result.Success));
            Assert.That(result == Result.Success, Is.False);
            Assert.That(result != Result.Failure, Is.False);
            Assert.That(result != Result.Success, Is.True);
        });
    }

    [Test]
    public void TestEqualsWithValue_WhenValueIsSet_ThenComparingToSuccessIsTrue()
    {
        string value = "my result";

        Result<string, Error<Exception>> result = new(value);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(Result.Success));
            Assert.That(result.IsFailure, Is.False);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result == Result.Success, Is.True);
            Assert.That(result, Is.Not.EqualTo(Result.Failure));
            Assert.That(result == Result.Failure, Is.False);
            Assert.That(result != Result.Failure, Is.True);
            Assert.That(result != Result.Success, Is.False);
        });
    }

    [Test]
    public void TestCasting_WhenErrorGiven_ThenCastItToFailureResult()
    {
        Error error = new("my error");

        Result<Error> result = error;

        Assert.Multiple(() =>
        {
            Assert.That(result.Error, Is.EqualTo(error));
            Assert.That(result.IsFailure, Is.True);
        });
    }

    [Test]
    public void TestCastingWithValue_WhenErrorGiven_ThenCastItToFailureResult()
    {
        Error error = new("my error");

        Result<int, Error> result = error;

        Assert.Multiple(() =>
        {
            Assert.That(result.Error, Is.EqualTo(error));
            Assert.That(result.Value, Is.Zero);
            Assert.That(result.IsFailure, Is.True);
        });
    }

    [Test]
    public void TestCastingWithValue_WhenValueGiven_ThenCastItToSuccessfulResult()
    {
        string value = "my value";

        Result<string, Error> result = value;

        Assert.Multiple(() =>
        {
            Assert.That(result.Error, Is.Null);
            Assert.That(result.Value, Is.EqualTo(value));
            Assert.That(result.IsSuccess, Is.True);
        });
    }

    [Test]
    public void TestDeconstruct_WhenErrorGiven_ThenErrorAndNullValueAreReturned()
    {
        Error<Exception> error = new("My error");

        (string? resultValue, Error<Exception>? resultError) = new Result<string, Error<Exception>>(
            error
        );

        Assert.Multiple(() =>
        {
            Assert.That(resultError, Is.EqualTo(error));
            Assert.That(resultValue, Is.Null);
        });
    }

    [Test]
    public void TestDeconstruct_WhenValueGiven_ThenNullExceptionsAndResultAreReturned()
    {
        string value = "My result";

        (string? resultValue, Error<Exception>? resultError) = new Result<string, Error<Exception>>(
            value
        );

        Assert.Multiple(() =>
        {
            Assert.That(resultError, Is.Null);
            Assert.That(resultValue, Is.EqualTo(value));
        });
    }

    [Test]
    public void TestMatch_WhenNullOnSuccessGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onSuccess"),
            () => new Result<Error<Exception>>().Match(null!, error => error.Message)
        );
    }

    [Test]
    public void TestMatch_WhenNullOnFailureGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onFailure"),
            () =>
                new Result<Error<Exception>>(new Error<Exception>("my error")).Match(
                    () => true,
                    null!
                )
        );
    }

    [Test]
    public void TestMatchAsync_WhenNullOnSuccessGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onSuccess"),
            async () =>
                await new Result<Error<Exception>>().MatchAsync(
                    null!,
                    error => Task.FromResult(error.Message)
                )
        );
    }

    [Test]
    public void TestMatchAsync_WhenNullOnFailureGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onFailure"),
            async () =>
                await new Result<Error<Exception>>(new Error<Exception>("my error")).Match(
                    () => Task.FromResult(true),
                    null!
                )
        );
    }

    [Test]
    public void TestMatch_WhenValueGiven_ThenInvokeFailureFunc()
    {
        string onSuccessValue = "Success";
        Result<Error<Exception>> result = new();

        string value = result.Match(() => onSuccessValue, error => error.Message);

        Assert.That(value, Is.EqualTo(onSuccessValue));
    }

    [Test]
    public void TestMatch_WhenErrorGiven_ThenInvokeFailureFunc()
    {
        Error<Exception> error = new("My error");
        Result<Error<Exception>> result = new(error);

        string value = result.Match(() => "Success", error => error.Message);

        Assert.That(value, Is.EqualTo("My error"));
    }

    [Test]
    public async Task TestMatchAsync_WhenValueGiven_ThenInvokeFailureFunc()
    {
        string onSuccessValue = "Success";
        Result<Error<Exception>> result = new();

        string value = await result.MatchAsync(
            () => Task.FromResult(onSuccessValue),
            error => Task.FromResult(error.Message)
        );

        Assert.That(value, Is.EqualTo(onSuccessValue));
    }

    [Test]
    public async Task TestMatchAsync_WhenErrorGiven_ThenInvokeFailureFunc()
    {
        Error<Exception> error = new("My error");
        Result<Error<Exception>> result = new(error);

        string value = await result.MatchAsync(
            () => Task.FromResult("Success"),
            error => Task.FromResult(error.Message)
        );

        Assert.That(value, Is.EqualTo("My error"));
    }

    [Test]
    public void TestMatchWithValue_WhenNullOnSuccessGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onSuccess"),
            () =>
                new Result<string, Error<Exception>>("test result").Match(
                    null!,
                    error => error.Message
                )
        );
    }

    [Test]
    public void TestMatchWithValue_WhenNullOnFailureGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onFailure"),
            () =>
                new Result<string, Error<Exception>>(new Error<Exception>("my error")).Match(
                    value => value,
                    null!
                )
        );
    }

    [Test]
    public void TestMatchWithValueAsync_WhenNullOnSuccessGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onSuccess"),
            async () =>
                await new Result<string, Error<Exception>>("test result").MatchAsync(
                    null!,
                    error => Task.FromResult(error.Message)
                )
        );
    }

    [Test]
    public void TestMatchWithValueAsync_WhenNullOnFailureGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onFailure"),
            async () =>
                await new Result<string, Error<Exception>>(
                    new Error<Exception>("my error")
                ).MatchAsync(value => Task.FromResult(value), null!)
        );
    }

    [Test]
    public void TestMatchWithValue_WhenValueGiven_ThenInvokeFailureFunc()
    {
        string testValue = "My result";
        Result<string, Error<Exception>> result = new(testValue);

        string matchValue = result.Match(value => value, error => error.Message);

        Assert.That(matchValue, Is.EqualTo(testValue));
    }

    [Test]
    public void TestMatchWithValue_WhenErrorGiven_ThenInvokeFailureFunc()
    {
        Error<Exception> error = new("My error");
        Result<string, Error<Exception>> result = new(error);

        string matchValue = result.Match(value => value, error => error.Message);

        Assert.That(matchValue, Is.EqualTo("My error"));
    }

    [Test]
    public async Task TestMatchWithValueAsync_WhenValueGiven_ThenInvokeFailureFunc()
    {
        string testValue = "My result";
        Result<string, Error<Exception>> result = new(testValue);

        string matchValue = await result.MatchAsync(
            value => Task.FromResult(value),
            error => Task.FromResult(error.Message)
        );

        Assert.That(matchValue, Is.EqualTo(testValue));
    }

    [Test]
    public async Task TestMatchWithValueAsync_WhenErrorGiven_ThenInvokeFailureFunc()
    {
        Error<Exception> error = new("My error");
        Result<string, Error<Exception>> result = new(error);

        string matchValue = await result.MatchAsync(
            value => Task.FromResult(value),
            error => Task.FromResult(error.Message)
        );

        Assert.That(matchValue, Is.EqualTo("My error"));
    }
}
