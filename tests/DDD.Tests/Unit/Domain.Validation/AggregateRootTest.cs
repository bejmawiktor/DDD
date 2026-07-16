using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using DDD.Tests.Unit.Utils;
using Utils.Validation;
using AggregateRootIncorrectDataCase = (
    string TextField,
    int IntField,
    System.AggregateException AggregateException
);
using AggregateRootOneOfExtendedValidatorsCase = (
    string TextField,
    int IntField,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.ExtendedValidatedAggregateRootFake> UpdateAction,
    System.AggregateException AggregateException
);
using AggregateRootOneOfValidatorsCase = (
    string TextField,
    int IntField,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.ValidatedAggregateRootFake> UpdateAction,
    System.AggregateException AggregateException
);

namespace DDD.Tests.Unit.Domain.Validation;

internal class AggregateRootTest
{
    public static IEnumerable<
        Func<TestDataRow<AggregateRootIncorrectDataCase>>
    > CreateIncorrectDataTestData()
    {
        yield return TestCase.Of<AggregateRootIncorrectDataCase>(
            (
                "",
                5,
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.TextField),
                        AggregateRootValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<AggregateRootIncorrectDataCase>(
            (
                "example text",
                -1,
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<AggregateRootIncorrectDataCase>(
            (
                "example text",
                -5,
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<AggregateRootIncorrectDataCase>(
            (
                "example text",
                11,
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
        yield return TestCase.Of<AggregateRootIncorrectDataCase>(
            (
                "",
                11,
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.TextField),
                        AggregateRootValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field greater than ten"
        );
        yield return TestCase.Of<AggregateRootIncorrectDataCase>(
            (
                "",
                -11,
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.TextField),
                        AggregateRootValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field less than zero"
        );
        yield return TestCase.Of<AggregateRootIncorrectDataCase>(
            (
                "",
                -5,
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.TextField),
                        AggregateRootValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field less than zero and forbidden value"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<AggregateRootOneOfValidatorsCase>>
    > CreateIncorrectDataForOneOfValidatorsTestData()
    {
        yield return TestCase.Of<AggregateRootOneOfValidatorsCase>(
            (
                "my example text",
                5,
                (ValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.TextField = "";
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.TextField),
                        AggregateRootValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<AggregateRootOneOfValidatorsCase>(
            (
                "example text",
                5,
                (ValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.IntField = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<AggregateRootOneOfValidatorsCase>(
            (
                "example text",
                5,
                (ValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.IntField = -5;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<AggregateRootOneOfValidatorsCase>(
            (
                "example text",
                5,
                (ValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.IntField = 11;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidatorFake.IntField),
                        AggregateRootValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<AggregateRootOneOfExtendedValidatorsCase>>
    > CreateIncorrectDataForOneOfExtendedValidatorsTestData()
    {
        yield return TestCase.Of<AggregateRootOneOfExtendedValidatorsCase>(
            (
                "my example text",
                5,
                (ExtendedValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.TextField = "";
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidationSource.TextField),
                        ExtendedAggregateRootValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<AggregateRootOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                (ExtendedValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.IntField = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidationSource.IntField),
                        ExtendedAggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<AggregateRootOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                (ExtendedValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.IntField = -5;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidationSource.IntField),
                        ExtendedAggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(AggregateRootValidationSource.IntField),
                        ExtendedAggregateRootValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<AggregateRootOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                (ExtendedValidatedAggregateRootFake aggregateRoot) =>
                {
                    aggregateRoot.IntField = 11;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(AggregateRootValidationSource.IntField),
                        ExtendedAggregateRootValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
    }

    [Test]
    public async Task TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedAggregateRootFake aggregateRoot = new(10, "example text", 5);

        using (Assert.Multiple())
        {
            await Assert.That(aggregateRoot.TextField).IsEqualTo("example text");
            await Assert.That(aggregateRoot.IntField).IsEqualTo(5);
        }
    }

    [Test]
    public async Task TestExtendedConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedAggregateRootFake aggregateRoot = new(10, "example text", 5);

        using (Assert.Multiple())
        {
            await Assert.That(aggregateRoot.TextField).IsEqualTo("example text");
            await Assert.That(aggregateRoot.IntField).IsEqualTo(5);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateIncorrectDataTestData))]
    public async Task TestValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ValidatedAggregateRootFake(10, textField, intField)
        );

        using (Assert.Multiple())
        {
            await Assert.That(exception).IsNotNull();
            await Assert
                .That(
                    exception?.Flatten().InnerExceptions.Select(exception => exception.ToString())
                )
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                );
            await Assert
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
    public async Task TestExtendedValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ExtendedValidatedAggregateRootFake(10, textField, intField)
        );

        using (Assert.Multiple())
        {
            await Assert.That(exception).IsNotNull();
            await Assert
                .That(
                    exception?.Flatten().InnerExceptions.Select(exception => exception.ToString())
                )
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                );
            await Assert
                .That(exception?.InnerExceptions.Select(exception => exception.GetType()))
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                );
        }
    }

    [Test]
    public async Task TestValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedAggregateRootFake aggregateRoot =
            new(10, "example text", 5) { TextField = "second text", IntField = 5 };

        using (Assert.Multiple())
        {
            await Assert.That(aggregateRoot.TextField).IsEqualTo("second text");
            await Assert.That(aggregateRoot.IntField).IsEqualTo(5);
        }
    }

    [Test]
    public async Task TestExtendedValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedAggregateRootFake aggregateRoot =
            new(10, "example text", 5) { TextField = "second text", IntField = 5 };

        using (Assert.Multiple())
        {
            await Assert.That(aggregateRoot.TextField).IsEqualTo("second text");
            await Assert.That(aggregateRoot.IntField).IsEqualTo(5);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateIncorrectDataForOneOfValidatorsTestData))]
    public async Task TestValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        Action<ValidatedAggregateRootFake> updateAction,
        AggregateException aggregateException
    )
    {
        ValidatedAggregateRootFake aggregateRoot = new(10, textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(aggregateRoot)
        );

        using (Assert.Multiple())
        {
            await Assert.That(exception).IsNotNull();
            await Assert
                .That(
                    exception?.Flatten().InnerExceptions.Select(exception => exception.ToString())
                )
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                );
            await Assert
                .That(exception?.InnerExceptions.Select(exception => exception.GetType()))
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                );
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateIncorrectDataForOneOfExtendedValidatorsTestData))]
    public async Task TestExtendedValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        Action<ExtendedValidatedAggregateRootFake> updateAction,
        AggregateException aggregateException
    )
    {
        ExtendedValidatedAggregateRootFake aggregateRoot = new(10, textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(aggregateRoot)
        );

        using (Assert.Multiple())
        {
            await Assert.That(exception).IsNotNull();
            await Assert
                .That(
                    exception?.Flatten().InnerExceptions.Select(exception => exception.ToString())
                )
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                );
            await Assert
                .That(exception?.InnerExceptions.Select(exception => exception.GetType()))
                .IsEquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                );
        }
    }
}
