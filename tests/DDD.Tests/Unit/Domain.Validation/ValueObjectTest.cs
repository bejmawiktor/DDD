using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using NUnit.Framework;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation;

internal class ValueObjectTest
{
    public static IEnumerable<TestCaseData> CreateIncorrectDataTestData(string testName)
    {
        yield return new TestCaseData(
            "",
            5,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
            "example text",
            -1,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
            "example text",
            -5,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.MinusFiveIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
            "example text",
            11,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.GreaterThanTenIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Greater Than Ten IntField)");
        yield return new TestCaseData(
            "",
            11,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.GreaterThanTenIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Empty TextField and Greater Than Ten IntField)");
        yield return new TestCaseData(
            "",
            -11,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Empty TextField and Less Than Zero IntField)");
        yield return new TestCaseData(
            "",
            -5,
            new AggregateException(
                [
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
                    ),
                ]
            )
        ).SetName($"{testName}(Empty TextField and Less Than Zero Forbidden Value IntField)");
    }

    public static IEnumerable<TestCaseData> CreateIncorrectDataForOneOfValidatorsTestData(
        string testName
    )
    {
        yield return new TestCaseData(
            "my example text",
            5,
            (ValidatedValueObjectFake valueObject) =>
            {
                valueObject.TextField = "";
            },
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.TextField),
                        ValueObjectValidatorFake.EmptyTextFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ValidatedValueObjectFake valueObject) =>
            {
                valueObject.IntField = -1;
            },
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ValidatedValueObjectFake valueObject) =>
            {
                valueObject.IntField = -5;
            },
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.LessThanZeroIntFieldErrorMessage
                    ),
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.MinusFiveIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ValidatedValueObjectFake valueObject) =>
            {
                valueObject.IntField = 11;
            },
            new AggregateException(
                [
                    new ValidationException(
                        nameof(ValueObjectValidatorFake.IntField),
                        ValueObjectValidatorFake.GreaterThanTenIntFieldErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Greater Than Ten IntField)");
    }

    public static IEnumerable<TestCaseData> CreateSingleValueIncorrectDataTestData(string testName)
    {
        yield return new TestCaseData(
            -1,
            1,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.LessThanZeroValueErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero Value)");
        yield return new TestCaseData(
            1,
            -1,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.NextValue),
                        SingleValueValueObjectValidatorFake.LessThanZeroNextValueErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero Next Value)");
        yield return new TestCaseData(
            -5,
            1,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.LessThanZeroValueErrorMessage
                    ),
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.MinusFiveValueErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value)");
        yield return new TestCaseData(
            11,
            1,
            new AggregateException(
                [
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.Value),
                        SingleValueValueObjectValidatorFake.GreaterThanTenValueErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Greater Than Ten Value)");
    }

    public static IEnumerable<TestCaseData> CreateSingleValueIncorrectDataForOneOfValidatorTestData(
        string testName
    )
    {
        yield return new TestCaseData(
            1,
            1,
            (SingleValueValidatedValueObjectFake valueObject) =>
            {
                valueObject.NextValue = -1;
            },
            new AggregateException(
                [
                    new ValidationException(
                        nameof(SingleValueValueObjectValidatorFake.NextValue),
                        SingleValueValueObjectValidatorFake.LessThanZeroNextValueErrorMessage
                    ),
                ]
            )
        ).SetName($"{testName}(Less Than Zero Next Value)");
    }

    [Test]
    public void TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedValueObjectFake valueObject = new("example text", 5);

        Assert.Multiple(() =>
        {
            Assert.That(valueObject.TextField, Is.EqualTo("example text"));
            Assert.That(valueObject.IntField, Is.EqualTo(5));
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataTestData),
        new object[] { nameof(TestValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown) }
    )]
    public void TestValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ValidatedValueObjectFake(textField, intField)
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

    [Test]
    public void TestValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedValueObjectFake valueObject =
            new("example text", 5) { TextField = "second text", IntField = 5 };

        Assert.Multiple(() =>
        {
            Assert.That(valueObject.TextField, Is.EqualTo("second text"));
            Assert.That(valueObject.IntField, Is.EqualTo(5));
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataForOneOfValidatorsTestData),
        new object[]
        {
            nameof(
                TestValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown
            ),
        }
    )]
    public void TestValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
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

    [Test]
    public void TestConstructingSingleValue_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        SingleValueValidatedValueObjectFake valueObject = new(5, 5);

        Assert.Multiple(() =>
        {
            Assert.That(valueObject.Value, Is.EqualTo(5));
            Assert.That(valueObject.NextValue, Is.EqualTo(5));
        });
    }

    [TestCaseSource(
        nameof(CreateSingleValueIncorrectDataTestData),
        new object[]
        {
            nameof(TestValidateSingleValue_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown),
        }
    )]
    public void TestValidateSingleValue_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        int value,
        int nextValue,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new SingleValueValidatedValueObjectFake(value, nextValue)
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

    [Test]
    public void TestValidateSingleValueWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        SingleValueValidatedValueObjectFake valueObject = new(5, 5) { NextValue = 9 };

        Assert.Multiple(() =>
        {
            Assert.That(valueObject.NextValue, Is.EqualTo(9));
        });
    }

    [TestCaseSource(
        nameof(CreateSingleValueIncorrectDataForOneOfValidatorTestData),
        new object[]
        {
            nameof(
                TestValidateSingleValueWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown
            ),
        }
    )]
    public void TestValidateSingleValueWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
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
