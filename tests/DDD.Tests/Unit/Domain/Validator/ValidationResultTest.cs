using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Domain.Validator;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validator;

[TestFixture]
public class ValidationResultTest
{
    [Test]
    public void TestConstructing_WhenExceptionsGiven_ThenExceptionsAreSetAndResultIsNull()
    {
        IEnumerable<Exception> exceptions = [new Exception("My exception")];

        ValidationResult<object> validationResult = new(exceptions);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.Exceptions, Is.EqualTo(exceptions));
            Assert.That(validationResult.Result, Is.Null);
        });
    }

    [Test]
    public void TestConstructing_WhenNullExceptionsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exceptions"),
            () => new ValidationResult<object>(null!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyExceptionsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty exceptions given."),
            () => new ValidationResult<object>(Enumerable.Empty<Exception>())
        );
    }

    [Test]
    public void TestConstructing_WhenResultGiven_ThenResultIsSetAndExceptionsAreNull()
    {
        string result = "my result";

        ValidationResult<string> validationResult = new(result);

        Assert.Multiple(() =>
        {
            Assert.That(validationResult.Result, Is.EqualTo(result));
            Assert.That(validationResult.Exceptions, Is.Null);
        });
    }

    [Test]
    public void TestEquals_WhenExceptionsAreSet_ThenComparingToFailureIsTrue()
    {
        IEnumerable<Exception> exceptions = [new Exception("My exception")];

        ValidationResult<object> validationResult = new(exceptions);

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
    public void TestEquals_WhenResultIsSet_ThenComparingToSuccessIsTrue()
    {
        string result = "my result";

        ValidationResult<string> validationResult = new(result);

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
}
