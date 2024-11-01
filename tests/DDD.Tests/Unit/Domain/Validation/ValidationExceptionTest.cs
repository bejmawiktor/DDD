using System;
using DDD.Domain.Validation;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
public class ValidationExceptionTest
{
    [Test]
    public void TestConstructing_WhenMessageIsGiven_ThenMessageIsSet()
    {
        string message = "Test message";

        ValidationException validationException = new(message);

        Assert.That(validationException.Message, Is.EqualTo(message));
    }

    [Test]
    public void TestConstructing_WhenNullMessageIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("message"),
            () => new ValidationException(null!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyMessageIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty message given."),
            () => new ValidationException(string.Empty)
        );
    }

    [Test]
    public void TestConstructing_WhenMessageAndFieldNameIsGiven_ThenMessageAndFieldNameIsSet()
    {
        string fieldName = "TestFieldName";
        string message = "Test message";

        ValidationException validationException = new(fieldName, message);

        Assert.Multiple(() =>
        {
            Assert.That(validationException.Message, Is.EqualTo(message));
            Assert.That(validationException.FieldName, Is.EqualTo(fieldName));
        });
    }

    [Test]
    public void TestConstructing_WhenNullMessageAndFieldNameIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("message"),
            () => new ValidationException("testField", null!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyMessageAndFieldNameIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty message given."),
            () => new ValidationException("testField", string.Empty)
        );
    }

    [Test]
    public void TestConstructing_WhenMessageAndNullFieldNameIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("fieldName"),
            () => new ValidationException(null!, "test message")
        );
    }

    [Test]
    public void TestConstructing_WhenMessageAndEmptyFieldNameIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty field name given."),
            () => new ValidationException(string.Empty, "Test message")
        );
    }

    [Test]
    public void TestToString_WhenMessageIsSet_ThenReturnMessage()
    {
        string message = "Test message";

        ValidationException validationException = new(message);

        Assert.That(
            validationException.ToString(),
            Is.EqualTo($"{typeof(ValidationException).FullName}: {message}")
        );
    }

    [Test]
    public void TestToString_WhenMessageAndFieldNameIsSet_ThenReturnFieldNameSeparatedWithDot()
    {
        string message = "Test message";
        string fieldName = "FieldName";

        ValidationException validationException = new(fieldName, message);

        Assert.That(
            validationException.ToString(),
            Is.EqualTo($"{typeof(ValidationException).FullName}: {fieldName}. {message}")
        );
    }
}
