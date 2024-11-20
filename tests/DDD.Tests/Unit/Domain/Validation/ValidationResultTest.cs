using System;
using System.Collections.Generic;
using DDD.Domain.Validation;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
public class ResultTest
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
}
