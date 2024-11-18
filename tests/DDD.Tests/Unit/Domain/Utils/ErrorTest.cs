using System;
using System.Collections.Generic;
using DDD.Domain.Utils;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Utils;

[TestFixture]
public class ErrorTest
{
    [Test]
    public void TestConstructing_WhenMessageIsGiven_ThenMessageIsSet()
    {
        string message = "Test message";

        Error error = new(message);

        Assert.That(error.Message, Is.EqualTo(message));
    }

    [Test]
    public void TestConstructingWithReason_WhenMessageIsGiven_ThenMessageIsSet()
    {
        string message = "Test message";

        Error<Exception> error = new(message);

        Assert.That(error.Message, Is.EqualTo(message));
    }

    [Test]
    public void TestConstructing_WhenNullMessageIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("message"),
            () => new Error(null!)
        );
    }

    [Test]
    public void TestConstructingWithReason_WhenNullMessageIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("message"),
            () => new Error<Exception>(null!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyMessageIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty message given."),
            () => new Error(string.Empty)
        );
    }

    [Test]
    public void TestConstructingWithReason_WhenEmptyMessageIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty message given."),
            () => new Error<Exception>(string.Empty)
        );
    }

    [Test]
    public void TestConstructingWithReason_WhenMessageAndReasonsIsGiven_ThenMessageAndReasonsIsSet()
    {
        string message = "Test message";
        IEnumerable<Exception> reasons = [new Exception("my reason")];

        Error<Exception> error = new(message, reasons);

        Assert.Multiple(() =>
        {
            Assert.That(error.Message, Is.EqualTo(message));
            Assert.That(error.Reasons, Is.EqualTo(reasons));
        });
    }

    [Test]
    public void TestConstructingWithReason_WhenNullMessageAndReasonsIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("message"),
            () => new Error<Exception>(null!, [new Exception("my reason")])
        );
    }

    [Test]
    public void TestConstructingWithReason_WhenEmptyMessageAndReasonsIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty message given."),
            () => new Error<Exception>(string.Empty, [new Exception("my reason")])
        );
    }

    [Test]
    public void TestConstructingWithReason_WhenMessageAndNullReasonsIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("reasons"),
            () => new Error<Exception>("test message", null!)
        );
    }

    [Test]
    public void TestConstructingWithReason_WhenMessageAndEmptyReasonsIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty reasons given."),
            () => new Error<Exception>("Test message", [])
        );
    }

    [Test]
    public void TestToStringWithReason_WhenMessageIsSet_ThenReturnMessage()
    {
        string message = "Test message";

        Error<Exception> error = new(message);

        Assert.That(error.ToString(), Is.EqualTo(message));
    }
}
