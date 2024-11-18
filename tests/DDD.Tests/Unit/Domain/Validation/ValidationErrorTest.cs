using System;
using System.Collections.Generic;
using DDD.Domain.Utils;
using DDD.Domain.Validation;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
public class ValidationErrorTest
{
    public static IEnumerable<TestCaseData> MultipleExceptionsTestCase
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[] { new ArgumentNullException("argument") },
                    new ArgumentNullException("argument").Message,
                }
            ).SetName(
                $"{nameof(TestConstruct_WhenExceptionsGiven_ThenMultipleReasonsMessageIsSet)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[]
                    {
                        new InvalidOperationException("invalid operation"),
                        new ArgumentNullException("argument"),
                    },
                    $"""
                    Multiple errors found:
                      - {new InvalidOperationException("invalid operation").Message}
                      - {new ArgumentNullException("argument").Message}
                    """,
                }
            ).SetName(
                $"{nameof(TestConstruct_WhenExceptionsGiven_ThenMultipleReasonsMessageIsSet)}(2)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[]
                    {
                        new InvalidOperationException("invalid operation"),
                        new ArgumentException("argument"),
                        new("exception"),
                    },
                    $"""
                    Multiple errors found:
                      - {new InvalidOperationException("invalid operation").Message}
                      - {new ArgumentException("argument").Message}
                      - {new Exception("exception").Message}
                    """,
                }
            ).SetName(
                $"{nameof(TestConstruct_WhenExceptionsGiven_ThenMultipleReasonsMessageIsSet)}(3)"
            );
        }
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public void TestConstruct_WhenExceptionsGiven_ThenMultipleReasonsMessageIsSet(
        IEnumerable<Exception> exceptions,
        string resultMessage
    )
    {
        ValidationError<Exception> validationError = new(exceptions);

        Assert.That(validationError.Message, Is.EqualTo(resultMessage));
    }

    [Test]
    public void TestConstruct_WhenExceptionsGiven_ThenReasonsIsSet()
    {
        IEnumerable<Exception> exceptions =
        [
            new Exception("excpetion"),
            new ArgumentNullException("argument"),
        ];

        ValidationError<Exception> validationError = new(exceptions);

        Assert.That(validationError.Reasons, Is.EqualTo(exceptions));
    }

    [Test]
    public void TestConstructing_WhenNullExceptionIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exceptions"),
            () => new ValidationError<Exception>((null as IEnumerable<Exception>)!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyExceptionsAreGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty exceptions given."),
            () => new ValidationError<Exception>([])
        );
    }

    [Test]
    public void TestConstructingWithReason_WhenNullMessageIsGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("message"),
            () => new ValidationError<Exception>((null as string)!)
        );
    }

    [Test]
    public void TestConstructing_WhenEmptyMessageIsGiven_ThenArgumentExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentException>()
                .And.Property(nameof(ArgumentException.Message))
                .EqualTo("Empty message given."),
            () => new ValidationError<Exception>(string.Empty)
        );
    }

    public static IEnumerable<TestCaseData> ToStringMultipleExceptionsTestCase
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[] { new ArgumentNullException("argument") },
                    new ArgumentNullException("argument").Message,
                }
            ).SetName(
                $"{nameof(TestConstruct_WhenExceptionsGiven_ThenMultipleReasonsMessageIsSet)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[]
                    {
                        new InvalidOperationException("invalid operation"),
                        new ArgumentNullException("argument"),
                    },
                    $"""
                    Multiple errors found:
                      - {new InvalidOperationException("invalid operation").Message}
                      - {new ArgumentNullException("argument").Message}
                    """,
                }
            ).SetName(
                $"{nameof(TestConstruct_WhenExceptionsGiven_ThenMultipleReasonsMessageIsSet)}(2)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[]
                    {
                        new InvalidOperationException("invalid operation"),
                        new ArgumentException("argument"),
                        new("exception"),
                    },
                    $"""
                    Multiple errors found:
                      - {new InvalidOperationException("invalid operation").Message}
                      - {new ArgumentException("argument").Message}
                      - {new Exception("exception").Message}
                    """,
                }
            ).SetName(
                $"{nameof(TestConstruct_WhenExceptionsGiven_ThenMultipleReasonsMessageIsSet)}(3)"
            );
        }
    }

    [Test]
    [TestCaseSource(nameof(ToStringMultipleExceptionsTestCase))]
    public void TestToString_WhenExceptionsGiven_ThenMultipleReasonsStringIsReturned(
        IEnumerable<Exception> exceptions,
        string resultMessage
    )
    {
        ValidationError<Exception> validationError = new(exceptions);

        Assert.That(validationError.ToString(), Is.EqualTo(resultMessage));
    }
}
