using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;
using System;

namespace DDD.Tests.Unit.Domain.Model;

[TestFixture]
public class ValidatedEntityTest
{
    [Test]
    public void TestConstructing_WhenValidatedObjectIsNotValid_ThenValidationExceptionsAreThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("field1"),
            () => new ValidatedEntityFake("1", null)
        );
    }

    [Test]
    public void TestConstructing_WhenValidatedObjectIsValid_ThenNoExceptionsAreThrown() => Assert.DoesNotThrow(() => new ValidatedEntityFake("1", 1));
}