using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using NUnit.Framework;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
internal class IdentifierTest
{
    public static IEnumerable<TestCaseData> CreateIncorrectDataTestData(string testName)
    {
        yield return new TestCaseData(
            "",
            new AggregateException(
                [
                    new ValidationException(
                        nameof(IdentifierValidatorFake.Value),
                        IdentifierValidatorFake.EmptyValueErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Empty Value)");
        yield return new TestCaseData(
            "AB",
            new AggregateException(
                [
                    new ValidationException(
                        nameof(IdentifierValidatorFake.Value),
                        IdentifierValidatorFake.ValueCantBeABErrorMessage
                    ),
                    new ValidationException(
                        nameof(IdentifierValidatorFake.Value),
                        IdentifierValidatorFake.ValueLengthGreaterThanOndeErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Value AB, Longer than 1)");
    }

    [Test]
    public void TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedIdentifierFake identifier = new("b");

        Assert.Multiple(() =>
        {
            Assert.That(identifier.Value, Is.EqualTo("b"));
        });
    }

    [Test]
    public void TestExtendedConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedIdentifierFake identifier = new("b");

        Assert.Multiple(() =>
        {
            Assert.That(identifier.Value, Is.EqualTo("b"));
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataTestData),
        new object[]
        {
            nameof(TestValidateIdentifier_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown),
        }
    )]
    public void TestValidateIdentifier_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string value,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ValidatedIdentifierFake(value)
        );

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(
                exception?.Flatten().InnerExceptions.Select(exception => exception.ToString()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                )
            );
            Assert.That(
                exception?.InnerExceptions.Select(exception => exception.GetType()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                )
            );
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataTestData),
        new object[]
        {
            nameof(
                TestExtendedValidateIdentifier_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown
            ),
        }
    )]
    public void TestExtendedValidateIdentifier_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string value,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ExtendedValidatedIdentifierFake(value)
        );

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(
                exception?.Flatten().InnerExceptions.Select(exception => exception.ToString()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                )
            );
            Assert.That(
                exception?.InnerExceptions.Select(exception => exception.GetType()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                )
            );
        });
    }
}
