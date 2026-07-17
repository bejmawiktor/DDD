using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using DDD.Tests.Unit.Utils;
using Utils.Validation;
using EntityIncorrectDataCase = (
    string TextField,
    int IntField,
    System.AggregateException AggregateException
);
using EntityOneOfExtendedValidatorsCase = (
    string TextField,
    int IntField,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.ExtendedValidatedEntityFake> UpdateAction,
    System.AggregateException AggregateException
);
using EntityOneOfValidatorsCase = (
    string TextField,
    int IntField,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.ValidatedEntityFake> UpdateAction,
    System.AggregateException AggregateException
);

namespace DDD.Tests.Unit.Domain.Validation;

internal class EntityTest
{
    public static IEnumerable<
        Func<TestDataRow<EntityIncorrectDataCase>>
    > CreateIncorrectDataTestData()
    {
        yield return TestCase.Of<EntityIncorrectDataCase>(
            (
                "",
                5,
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.TextField),
                        EntityValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<EntityIncorrectDataCase>(
            (
                "example text",
                -1,
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<EntityIncorrectDataCase>(
            (
                "example text",
                -5,
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<EntityIncorrectDataCase>(
            (
                "example text",
                11,
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
        yield return TestCase.Of<EntityIncorrectDataCase>(
            (
                "",
                11,
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.TextField),
                        EntityValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field greater than ten"
        );
        yield return TestCase.Of<EntityIncorrectDataCase>(
            (
                "",
                -11,
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.TextField),
                        EntityValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field less than zero"
        );
        yield return TestCase.Of<EntityIncorrectDataCase>(
            (
                "",
                -5,
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.TextField),
                        EntityValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field less than zero and forbidden value"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<EntityOneOfValidatorsCase>>
    > CreateIncorrectDataForOneOfValidatorsTestData()
    {
        yield return TestCase.Of<EntityOneOfValidatorsCase>(
            (
                "my example text",
                5,
                entity =>
                {
                    entity.TextField = "";
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.TextField),
                        EntityValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<EntityOneOfValidatorsCase>(
            (
                "example text",
                5,
                entity =>
                {
                    entity.IntField = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<EntityOneOfValidatorsCase>(
            (
                "example text",
                5,
                entity =>
                {
                    entity.IntField = -5;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<EntityOneOfValidatorsCase>(
            (
                "example text",
                5,
                entity =>
                {
                    entity.IntField = 11;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidatorFake.IntField),
                        EntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<EntityOneOfExtendedValidatorsCase>>
    > CreateIncorrectDataForOneOfExtendedValidatorsTestData()
    {
        yield return TestCase.Of<EntityOneOfExtendedValidatorsCase>(
            (
                "my example text",
                5,
                entity =>
                {
                    entity.TextField = "";
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidationSource.TextField),
                        ExtendedEntityValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<EntityOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                entity =>
                {
                    entity.IntField = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidationSource.IntField),
                        ExtendedEntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<EntityOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                entity =>
                {
                    entity.IntField = -5;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidationSource.IntField),
                        ExtendedEntityValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(EntityValidationSource.IntField),
                        ExtendedEntityValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<EntityOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                entity =>
                {
                    entity.IntField = 11;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(EntityValidationSource.IntField),
                        ExtendedEntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
    }

    [Test]
    public async Task TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedEntityFake entity = new(10, "example text", 5);

        using (Assert.Multiple())
        {
            _ = await Assert.That(entity.TextField).IsEqualTo("example text");
            _ = await Assert.That(entity.IntField).IsEqualTo(5);
        }
    }

    [Test]
    public async Task TestExtendedConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedEntityFake entity = new(10, "example text", 5);

        using (Assert.Multiple())
        {
            _ = await Assert.That(entity.TextField).IsEqualTo("example text");
            _ = await Assert.That(entity.IntField).IsEqualTo(5);
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
        AggregateException? exception = Assert.Throws<AggregateException>(() =>
            new ValidatedEntityFake(10, textField, intField)
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
    public async Task TestExtendedValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(() =>
            new ExtendedValidatedEntityFake(10, textField, intField)
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
    public async Task TestValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedEntityFake entity = new(10, "example text", 5)
        {
            TextField = "second text",
            IntField = 5,
        };

        using (Assert.Multiple())
        {
            _ = await Assert.That(entity.TextField).IsEqualTo("second text");
            _ = await Assert.That(entity.IntField).IsEqualTo(5);
        }
    }

    [Test]
    public async Task TestExtendedValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedEntityFake entity = new(10, "example text", 5)
        {
            TextField = "second text",
            IntField = 5,
        };

        using (Assert.Multiple())
        {
            _ = await Assert.That(entity.TextField).IsEqualTo("second text");
            _ = await Assert.That(entity.IntField).IsEqualTo(5);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateIncorrectDataForOneOfValidatorsTestData))]
    public async Task TestValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        Action<ValidatedEntityFake> updateAction,
        AggregateException aggregateException
    )
    {
        ValidatedEntityFake entity = new(10, textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(() =>
            updateAction(entity)
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
    [MethodDataSource(nameof(CreateIncorrectDataForOneOfExtendedValidatorsTestData))]
    public async Task TestExtendedValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        Action<ExtendedValidatedEntityFake> updateAction,
        AggregateException aggregateException
    )
    {
        ExtendedValidatedEntityFake entity = new(10, textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(() =>
            updateAction(entity)
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
