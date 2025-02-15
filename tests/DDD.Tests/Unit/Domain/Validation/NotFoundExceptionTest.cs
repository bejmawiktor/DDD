using System;
using DDD.Domain.Validation;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
public class NotFoundExceptionTest
{
    [Test]
    public void TestConstructing_WhenMessageIsGiven_ThenMessageIsSet()
    {
        string message = "Test message";

        NotFoundException notFoundException = new(message);

        Assert.That(notFoundException.Message, Is.EqualTo(message));
    }

    [Test]
    public void TestConstructing_WhenNullMessageIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("message"),
            () => new NotFoundException(null!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyMessageIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty message given."),
            () => new NotFoundException(string.Empty)
        );
    }
}
