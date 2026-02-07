using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using NUnit.Framework;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation;

internal class AggregateRootTest
{
    public static IEnumerable<TestCaseData> CreateIncorrectDataTestData(string testName)
    {
        yield return new TestCaseData(
            "",
            5,
            new AggregateException(
                new ValidationException(
                    nameof(AggregateRootValidatorFake.TextField),
                    AggregateRootValidatorFake.EmptyTextFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
            "example text",
            -1,
            new AggregateException(
                new ValidationException(
                    nameof(AggregateRootValidatorFake.IntField),
                    AggregateRootValidatorFake.LessThanZeroIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
            "example text",
            11,
            new AggregateException(
                new ValidationException(
                    nameof(AggregateRootValidatorFake.IntField),
                    AggregateRootValidatorFake.GreaterThanTenIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Greater Than Ten IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Empty TextField and Greater Than Ten IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Empty TextField and Less Than Zero IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Empty TextField and Less Than Zero Forbidden Value IntField)");
    }

    public static IEnumerable<TestCaseData> CreateIncorrectDataForOneOfValidatorsTestData(
        string testName
    )
    {
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Greater Than Ten IntField)");
    }

    public static IEnumerable<TestCaseData> CreateIncorrectDataForOneOfExtendedValidatorsTestData(
        string testName
    )
    {
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Greater Than Ten IntField)");
    }

    [Test]
    public void TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedAggregateRootFake aggregateRoot = new(10, "example text", 5);

        Assert.Multiple(() =>
        {
            Assert.That(aggregateRoot.TextField, Is.EqualTo("example text"));
            Assert.That(aggregateRoot.IntField, Is.EqualTo(5));
        });
    }

    [Test]
    public void TestExtendedConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedAggregateRootFake aggregateRoot = new(10, "example text", 5);

        Assert.Multiple(() =>
        {
            Assert.That(aggregateRoot.TextField, Is.EqualTo("example text"));
            Assert.That(aggregateRoot.IntField, Is.EqualTo(5));
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
            () => new ValidatedAggregateRootFake(10, textField, intField)
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
            nameof(TestExtendedValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown),
        }
    )]
    public void TestExtendedValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ExtendedValidatedAggregateRootFake(10, textField, intField)
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
        ValidatedAggregateRootFake aggregateRoot =
            new(10, "example text", 5) { TextField = "second text", IntField = 5 };

        Assert.Multiple(() =>
        {
            Assert.That(aggregateRoot.TextField, Is.EqualTo("second text"));
            Assert.That(aggregateRoot.IntField, Is.EqualTo(5));
        });
    }

    [Test]
    public void TestExtendedValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedAggregateRootFake aggregateRoot =
            new(10, "example text", 5) { TextField = "second text", IntField = 5 };

        Assert.Multiple(() =>
        {
            Assert.That(aggregateRoot.TextField, Is.EqualTo("second text"));
            Assert.That(aggregateRoot.IntField, Is.EqualTo(5));
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
        Action<ValidatedAggregateRootFake> updateAction,
        AggregateException aggregateException
    )
    {
        ValidatedAggregateRootFake aggregateRoot = new(10, textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(aggregateRoot)
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
        nameof(CreateIncorrectDataForOneOfExtendedValidatorsTestData),
        new object[]
        {
            nameof(
                TestExtendedValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown
            ),
        }
    )]
    public void TestExtendedValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
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
