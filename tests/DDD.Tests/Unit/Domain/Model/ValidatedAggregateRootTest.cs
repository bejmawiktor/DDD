using System;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Model;

[TestFixture]
public class ValidatedAggregateRootTest
{
    [Test]
    public void TestConstructing_WhenValidatedObjectIsNotValid_ThenValidationExceptionsAreThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("field1"),
            () => new ValidatedAggregateRootFake("1", null)
        );
    }

    [Test]
    public void TestConstructing_WhenValidatedObjectIsValid_ThenNoExceptionsAreThrown() =>
        Assert.DoesNotThrow(() => new ValidatedAggregateRootFake("1", 1));
}
