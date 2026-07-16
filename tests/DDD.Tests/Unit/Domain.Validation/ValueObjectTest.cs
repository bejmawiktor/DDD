using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using DDD.Tests.Unit.Utils;
using Utils.Validation;
using SingleValueIncorrectDataCase = (
    int Value,
    int NextValue,
    System.AggregateException AggregateException
);
using SingleValueOneOfExtendedValidatorCase = (
    int Value,
    int NextValue,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.ExtendedSingleValueValidatedValueObjectFake> UpdateAction,
    System.AggregateException AggregateException
);
using SingleValueOneOfValidatorCase = (
    int Value,
    int NextValue,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.SingleValueValidatedValueObjectFake> UpdateAction,
    System.AggregateException AggregateException
);
using ValueObjectIncorrectDataCase = (
    string TextField,
    int IntField,
    System.AggregateException AggregateException
);
using ValueObjectOneOfExtendedValidatorsCase = (
    string TextField,
    int IntField,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.ExtendedValidatedValueObjectFake> UpdateAction,
    System.AggregateException AggregateException
);
using ValueObjectOneOfValidatorsCase = (
    string TextField,
    int IntField,
    System.Action<DDD.Tests.Unit.Domain.Validation.TestDoubles.ValidatedValueObjectFake> UpdateAction,
    System.AggregateException AggregateException
);

namespace DDD.Tests.Unit.Domain.Validation;

internal class ValueObjectTest
{
    public static IEnumerable<
        Func<TestDataRow<ValueObjectIncorrectDataCase>>
    > CreateIncorrectDataTestData()
    {
        yield return TestCase.Of<ValueObjectIncorrectDataCase>(
            (
                "",
                5,
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<ValueObjectIncorrectDataCase>(
            (
                "example text",
                -1,
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<ValueObjectIncorrectDataCase>(
            (
                "example text",
                -5,
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<ValueObjectIncorrectDataCase>(
            (
                "example text",
                11,
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
        yield return TestCase.Of<ValueObjectIncorrectDataCase>(
            (
                "",
                11,
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field greater than ten"
        );
        yield return TestCase.Of<ValueObjectIncorrectDataCase>(
            (
                "",
                -11,
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field less than zero"
        );
        yield return TestCase.Of<ValueObjectIncorrectDataCase>(
            (
                "",
                -5,
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Empty text field and int field less than zero and forbidden value"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<ValueObjectOneOfValidatorsCase>>
    > CreateIncorrectDataForOneOfValidatorsTestData()
    {
        yield return TestCase.Of<ValueObjectOneOfValidatorsCase>(
            (
                "my example text",
                5,
                (ValidatedValueObjectFake valueObject) =>
                {
                    valueObject.TextField = "";
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<ValueObjectOneOfValidatorsCase>(
            (
                "example text",
                5,
                (ValidatedValueObjectFake valueObject) =>
                {
                    valueObject.IntField = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<ValueObjectOneOfValidatorsCase>(
            (
                "example text",
                5,
                (ValidatedValueObjectFake valueObject) =>
                {
                    valueObject.IntField = -5;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<ValueObjectOneOfValidatorsCase>(
            (
                "example text",
                5,
                (ValidatedValueObjectFake valueObject) =>
                {
                    valueObject.IntField = 11;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<ValueObjectOneOfExtendedValidatorsCase>>
    > CreateIncorrectDataForOneOfExtendedValidatorsTestData()
    {
        yield return TestCase.Of<ValueObjectOneOfExtendedValidatorsCase>(
            (
                "my example text",
                5,
                (ExtendedValidatedValueObjectFake valueObject) =>
                {
                    valueObject.TextField = "";
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidationSource.TextField),
                        ExtendedValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    )
                )
            ),
            "Empty text field"
        );
        yield return TestCase.Of<ValueObjectOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                (ExtendedValidatedValueObjectFake valueObject) =>
                {
                    valueObject.IntField = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidationSource.IntField),
                        ExtendedValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero"
        );
        yield return TestCase.Of<ValueObjectOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                (ExtendedValidatedValueObjectFake valueObject) =>
                {
                    valueObject.IntField = -5;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidationSource.IntField),
                        ExtendedValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidationSource.IntField),
                        ExtendedValueObjectValidatorFake.MinusFiveIntFieldErrorMessage
                    )
                )
            ),
            "Int field less than zero and forbidden value"
        );
        yield return TestCase.Of<ValueObjectOneOfExtendedValidatorsCase>(
            (
                "example text",
                5,
                (ExtendedValidatedValueObjectFake valueObject) =>
                {
                    valueObject.IntField = 11;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(ValueObjectValidationSource.IntField),
                        ExtendedValueObjectValidatorFake.GreaterThanTenIntFieldErrorMessage
                    )
                )
            ),
            "Int field greater than ten"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<SingleValueIncorrectDataCase>>
    > CreateSingleValueIncorrectDataTestData()
    {
        yield return TestCase.Of<SingleValueIncorrectDataCase>(
            (
                -1,
                1,
                new AggregateException(
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.LessThanZeroValueErrorMessage
                    )
                )
            ),
            "Value less than zero"
        );
        yield return TestCase.Of<SingleValueIncorrectDataCase>(
            (
                1,
                -1,
                new AggregateException(
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.NextValue),
                        SingleValueValueObjectValidatorFake.LessThanZeroNextValueErrorMessage
                    )
                )
            ),
            "Next value less than zero"
        );
        yield return TestCase.Of<SingleValueIncorrectDataCase>(
            (
                -5,
                1,
                new AggregateException(
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.LessThanZeroValueErrorMessage
                    ),
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.MinusFiveValueErrorMessage
                    )
                )
            ),
            "Value less than zero and forbidden value"
        );
        yield return TestCase.Of<SingleValueIncorrectDataCase>(
            (
                11,
                1,
                new AggregateException(
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.GreaterThanTenValueErrorMessage
                    )
                )
            ),
            "Value greater than ten"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<SingleValueOneOfValidatorCase>>
    > CreateSingleValueIncorrectDataForOneOfValidatorTestData()
    {
        yield return TestCase.Of<SingleValueOneOfValidatorCase>(
            (
                1,
                1,
                (SingleValueValidatedValueObjectFake valueObject) =>
                {
                    valueObject.NextValue = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.NextValue),
                        SingleValueValueObjectValidatorFake.LessThanZeroNextValueErrorMessage
                    )
                )
            ),
            "Next value less than zero"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<SingleValueOneOfExtendedValidatorCase>>
    > CreateSingleValueIncorrectDataForOneOfExtendedValidatorTestData()
    {
        yield return TestCase.Of<SingleValueOneOfExtendedValidatorCase>(
            (
                1,
                1,
                (ExtendedSingleValueValidatedValueObjectFake valueObject) =>
                {
                    valueObject.NextValue = -1;
                },
                new AggregateException(
                    new ValidationException(
                        nameof(SingleValueValueObjectValidationSource.NextValue),
                        ExtendedSingleValueValueObjectValidatorFake.LessThanZeroNextValueErrorMessage
                    )
                )
            ),
            "Next value less than zero"
        );
    }

    [Test]
    public async Task TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedValueObjectFake valueObject = new("example text", 5);

        using (Assert.Multiple())
        {
            _ = await Assert.That(valueObject.TextField).IsEqualTo("example text");
            _ = await Assert.That(valueObject.IntField).IsEqualTo(5);
        }
    }

    [Test]
    public async Task TestExtendedConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedValueObjectFake valueObject = new("example text", 5);

        using (Assert.Multiple())
        {
            _ = await Assert.That(valueObject.TextField).IsEqualTo("example text");
            _ = await Assert.That(valueObject.IntField).IsEqualTo(5);
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
            () => new ValidatedValueObjectFake(textField, intField)
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
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ExtendedValidatedValueObjectFake(textField, intField)
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
        ValidatedValueObjectFake valueObject =
            new("example text", 5) { TextField = "second text", IntField = 5 };

        using (Assert.Multiple())
        {
            _ = await Assert.That(valueObject.TextField).IsEqualTo("second text");
            _ = await Assert.That(valueObject.IntField).IsEqualTo(5);
        }
    }

    [Test]
    public async Task TestExtendedValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedValueObjectFake valueObject =
            new("example text", 5) { TextField = "second text", IntField = 5 };

        using (Assert.Multiple())
        {
            _ = await Assert.That(valueObject.TextField).IsEqualTo("second text");
            _ = await Assert.That(valueObject.IntField).IsEqualTo(5);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateIncorrectDataForOneOfValidatorsTestData))]
    public async Task TestValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        Action<ValidatedValueObjectFake> updateAction,
        AggregateException aggregateException
    )
    {
        ValidatedValueObjectFake valueObject = new(textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(valueObject)
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
        Action<ExtendedValidatedValueObjectFake> updateAction,
        AggregateException aggregateException
    )
    {
        ExtendedValidatedValueObjectFake valueObject = new(textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(valueObject)
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
    public async Task TestConstructingSingleValue_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        SingleValueValidatedValueObjectFake valueObject = new(5, 5);

        using (Assert.Multiple())
        {
            _ = await Assert.That(valueObject.Value).IsEqualTo(5);
            _ = await Assert.That(valueObject.NextValue).IsEqualTo(5);
        }
    }

    [Test]
    public async Task TestExtendedConstructingSingleValue_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedSingleValueValidatedValueObjectFake valueObject = new(5, 5);

        using (Assert.Multiple())
        {
            _ = await Assert.That(valueObject.Value).IsEqualTo(5);
            _ = await Assert.That(valueObject.NextValue).IsEqualTo(5);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateSingleValueIncorrectDataTestData))]
    public async Task TestValidateSingleValue_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        int value,
        int nextValue,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new SingleValueValidatedValueObjectFake(value, nextValue)
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
    [MethodDataSource(nameof(CreateSingleValueIncorrectDataTestData))]
    public async Task TestExtendedValidateSingleValue_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        int value,
        int nextValue,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ExtendedSingleValueValidatedValueObjectFake(value, nextValue)
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
    public async Task TestValidateSingleValueWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        SingleValueValidatedValueObjectFake valueObject = new(5, 5) { NextValue = 9 };

        _ = await Assert.That(valueObject.NextValue).IsEqualTo(9);
    }

    [Test]
    public async Task TestExtendedValidateSingleValueWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedSingleValueValidatedValueObjectFake valueObject = new(5, 5) { NextValue = 9 };

        _ = await Assert.That(valueObject.NextValue).IsEqualTo(9);
    }

    [Test]
    [MethodDataSource(nameof(CreateSingleValueIncorrectDataForOneOfValidatorTestData))]
    public async Task TestValidateSingleValueWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        int value,
        int nextValue,
        Action<SingleValueValidatedValueObjectFake> updateAction,
        AggregateException aggregateException
    )
    {
        SingleValueValidatedValueObjectFake valueObject = new(value, nextValue);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(valueObject)
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
    [MethodDataSource(nameof(CreateSingleValueIncorrectDataForOneOfExtendedValidatorTestData))]
    public async Task TestExtendedValidateSingleValueWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        int value,
        int nextValue,
        Action<ExtendedSingleValueValidatedValueObjectFake> updateAction,
        AggregateException aggregateException
    )
    {
        ExtendedSingleValueValidatedValueObjectFake valueObject = new(value, nextValue);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(valueObject)
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
