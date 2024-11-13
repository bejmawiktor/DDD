using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Domain.Validation;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
public class ValidationResultTest
{
    [Test]
    public void TestConstructing_WhenExceptionsGiven_ThenExceptionsAreSetAndResultIsNull()
    {
        IEnumerable<Exception> exceptions = [new Exception("My exception")];

        ValidationResult<Exception> validationResult = new(exceptions);

        Assert.That(validationResult.Exceptions, Is.EqualTo(exceptions));
    }

    [Test]
    public void TestConstructingWithValue_WhenExceptionsGiven_ThenExceptionsAreSetAndResultIsNull()
    {
        IEnumerable<Exception> exceptions = [new Exception("My exception")];

        ValidationResult<object, Exception> validationResult = new(exceptions);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.Exceptions, Is.EqualTo(exceptions));
            Assert.That(validationResult.Value, Is.Null);
        });
    }

    [Test]
    public void TestConstructing_WhenNullExceptionsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exceptions"),
            () => new ValidationResult<Exception>(null!)
        );
    }

    [Test]
    public void TestConstructingWithValue_WhenNullExceptionsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exceptions"),
            () => new ValidationResult<object, Exception>(null!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyExceptionsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty exceptions given."),
            () => new ValidationResult<Exception>([])
        );
    }

    [Test]
    public void TestConstructingWithValue_WhenEmptyExceptionsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty exceptions given."),
            () => new ValidationResult<object, Exception>([])
        );
    }

    [Test]
    public void TestConstructingWithValue_WhenResultGiven_ThenResultIsSetAndExceptionsAreNull()
    {
        string value = "my result";

        ValidationResult<object, Exception> validationResult = new(value);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.Value, Is.EqualTo(value));
            Assert.That(validationResult.Exceptions, Is.Null);
        });
    }

    [Test]
    public void TestConstructingWithValue_WhenExceptionsGiven_ThenExceptionsAreSet()
    {
        IEnumerable<Exception> exceptions = [new Exception("My exception")];

        ValidationResult<object, Exception> validationResult = new(exceptions);

        Assert.That(validationResult.Exceptions, Is.EqualTo(exceptions));
    }

    [Test]
    public void TestEquals_WhenExceptionsAreSet_ThenComparingToFailureIsTrue()
    {
        IEnumerable<Exception> exceptions = [new Exception("My exception")];

        ValidationResult<Exception> validationResult = new(exceptions);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult, Is.EqualTo(ValidationResult.Failure));
            Assert.That(validationResult == ValidationResult.Failure, Is.True);
            Assert.That(validationResult, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(validationResult == ValidationResult.Success, Is.False);
            Assert.That(validationResult != ValidationResult.Failure, Is.False);
            Assert.That(validationResult != ValidationResult.Success, Is.True);
        });
    }

    [Test]
    public void TestEquals_WhenExceptionsAreNotSet_ThenComparingToSuccessIsTrue()
    {
        ValidationResult<Exception> validationResult = new();

        Assert.Multiple(() =>
        {
            Assert.That(validationResult, Is.EqualTo(ValidationResult.Success));
            Assert.That(validationResult == ValidationResult.Success, Is.True);
            Assert.That(validationResult, Is.Not.EqualTo(ValidationResult.Failure));
            Assert.That(validationResult == ValidationResult.Failure, Is.False);
            Assert.That(validationResult != ValidationResult.Success, Is.False);
            Assert.That(validationResult != ValidationResult.Failure, Is.True);
        });
    }

    [Test]
    public void TestEqualsWithValue_WhenExceptionsAreSet_ThenComparingToFailureIsTrue()
    {
        IEnumerable<Exception> exceptions = [new Exception("My exception")];

        ValidationResult<object, Exception> validationResult = new(exceptions);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult, Is.EqualTo(ValidationResult.Failure));
            Assert.That(validationResult == ValidationResult.Failure, Is.True);
            Assert.That(validationResult, Is.Not.EqualTo(ValidationResult.Success));
            Assert.That(validationResult == ValidationResult.Success, Is.False);
            Assert.That(validationResult != ValidationResult.Failure, Is.False);
            Assert.That(validationResult != ValidationResult.Success, Is.True);
        });
    }

    [Test]
    public void TestEqualsWithValue_WhenResultIsSet_ThenComparingToSuccessIsTrue()
    {
        string value = "my result";

        ValidationResult<object, Exception> validationResult = new(value);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult, Is.EqualTo(ValidationResult.Success));
            Assert.That(validationResult == ValidationResult.Success, Is.True);
            Assert.That(validationResult, Is.Not.EqualTo(ValidationResult.Failure));
            Assert.That(validationResult == ValidationResult.Failure, Is.False);
            Assert.That(validationResult != ValidationResult.Failure, Is.True);
            Assert.That(validationResult != ValidationResult.Success, Is.False);
        });
    }

    [Test]
    public void TestDeconstruct_WhenExceptionGiven_ThenExceptionsAndNullResultAreReturned()
    {
        IEnumerable<Exception> testExceptions = [new Exception("My exception")];

        (object? value, IEnumerable<Exception>? exceptions) = new ValidationResult<
            object,
            Exception
        >(testExceptions);

        Assert.Multiple(() =>
        {
            Assert.That(exceptions, Is.EqualTo(exceptions));
            Assert.That(value, Is.Null);
        });
    }

    [Test]
    public void TestDeconstruct_WhenResultGiven_ThenNullExceptionsAndResultAreReturned()
    {
        string testValue = "My result";

        (object? value, IEnumerable<Exception>? exceptions) = new ValidationResult<
            object,
            Exception
        >(testValue);

        Assert.Multiple(() =>
        {
            Assert.That(exceptions, Is.Null);
            Assert.That(value, Is.EqualTo(testValue));
        });
    }

    [Test]
    public void TestMatch_WhenNullOnSuccessGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onSuccess"),
            () =>
                new ValidationResult<Exception>().Match(
                    null!,
                    exceptions => exceptions.First().Message
                )
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
                new ValidationResult<Exception>([new Exception("example exception")]).Match(
                    () => true,
                    null!
                )
        );
    }

    [Test]
    public void TestMatch_WhenResultGiven_ThenInvokeFailureFunc()
    {
        string onSuccessValue = "Success";
        ValidationResult<Exception> validationResult = new();

        string value = validationResult.Match(
            () => onSuccessValue,
            exceptions => exceptions.First().Message
        );

        Assert.That(value, Is.EqualTo(onSuccessValue));
    }

    [Test]
    public void TestMatch_WhenExceptionsGiven_ThenInvokeFailureFunc()
    {
        IEnumerable<Exception> testExceptions = [new Exception("My exception")];
        ValidationResult<Exception> validationResult = new(testExceptions);

        string value = validationResult.Match(
            () => "Success",
            exceptions => exceptions.First().Message
        );

        Assert.That(value, Is.EqualTo("My exception"));
    }

    [Test]
    public void TestMatchWithValue_WhenNullOnSuccessGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("onSuccess"),
            () =>
                new ValidationResult<object, Exception>("test result").Match(
                    null!,
                    exceptions => exceptions.First().Message
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
                new ValidationResult<object, Exception>([new Exception("example exception")]).Match(
                    value => value,
                    null!
                )
        );
    }

    [Test]
    public void TestMatchWithValue_WhenResultGiven_ThenInvokeFailureFunc()
    {
        string testValue = "My result";
        ValidationResult<string, Exception> validationResult = new(testValue);

        string value = validationResult.Match(
            value => value,
            exceptions => exceptions.First().Message
        );

        Assert.That(value, Is.EqualTo(testValue));
    }

    [Test]
    public void TestMatchWithValue_WhenExceptionsGiven_ThenInvokeFailureFunc()
    {
        IEnumerable<Exception> testExceptions = [new Exception("My exception")];
        ValidationResult<string, Exception> validationResult = new(testExceptions);

        string result = validationResult.Match(
            value => value,
            exceptions => exceptions.First().Message
        );

        Assert.That(result, Is.EqualTo("My exception"));
    }
}
