using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using DDD.Tests.Unit.Utils;
using Utils.Validation;
using IdentifierIncorrectDataCase = (string Value, System.AggregateException AggregateException);

namespace DDD.Tests.Unit.Domain.Validation;

internal class IdentifierTest
{
    public static IEnumerable<
        Func<TestDataRow<IdentifierIncorrectDataCase>>
    > CreateIncorrectDataTestData()
    {
        yield return TestCase.Of<IdentifierIncorrectDataCase>(
            (
                "",
                new AggregateException(
                    new ValidationException(
                        nameof(IdentifierValidatorFake.Value),
                        IdentifierValidatorFake.EmptyValueErrorMessage
                    )
                )
            ),
            "Empty value"
        );
        yield return TestCase.Of<IdentifierIncorrectDataCase>(
            (
                "AB",
                new AggregateException(
                    new ValidationException(
                        nameof(IdentifierValidatorFake.Value),
                        IdentifierValidatorFake.ValueCantBeABErrorMessage
                    ),
                    new ValidationException(
                        nameof(IdentifierValidatorFake.Value),
                        IdentifierValidatorFake.ValueLengthGreaterThanOndeErrorMessage
                    )
                )
            ),
            "Value AB, longer than 1"
        );
    }

    [Test]
    public async Task TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedIdentifierFake identifier = new("b");

        _ = await Assert.That(identifier.Value).IsEqualTo("b");
    }

    [Test]
    public async Task TestExtendedConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedIdentifierFake identifier = new("b");

        _ = await Assert.That(identifier.Value).IsEqualTo("b");
    }

    [Test]
    [MethodDataSource(nameof(CreateIncorrectDataTestData))]
    public async Task TestValidateIdentifier_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string value,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(() =>
            new ValidatedIdentifierFake(value)
        );

        using (Assert.Multiple())
        {
            _ = await Assert.That(exception).IsNotNull();
            _ = await Assert
                .That(
                    exception?.Flatten().InnerExceptions.Select(exception => exception.ToString())
                )
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                );
            _ = await Assert
                .That(exception?.InnerExceptions.Select(exception => exception.GetType()))
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                );
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateIncorrectDataTestData))]
    public async Task TestExtendedValidateIdentifier_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string value,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(() =>
            new ExtendedValidatedIdentifierFake(value)
        );

        using (Assert.Multiple())
        {
            _ = await Assert.That(exception).IsNotNull();
            _ = await Assert
                .That(
                    exception?.Flatten().InnerExceptions.Select(exception => exception.ToString())
                )
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                );
            _ = await Assert
                .That(exception?.InnerExceptions.Select(exception => exception.GetType()))
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                );
        }
    }
}
