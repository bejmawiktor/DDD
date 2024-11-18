using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Domain.Utils;
using DDD.Domain.Validation;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
public class ValidatorTest
{
    public static IEnumerable<TestCaseData> MultipleExceptionsTestCase
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[] { new("exception"), new ArgumentNullException("argument") },
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[]
                    {
                        new InvalidOperationException("invalid operation"),
                        new ArgumentNullException("argument"),
                    },
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(2)"
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
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(3)"
            );

            yield return new TestCaseData(
                new object[]
                {
                    new Exception[] { new InvalidOperationException("invalid operation") },
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(4)"
            );
        }
    }

    public static IEnumerable<object> SuccessResultsTestCase => ["result", 1, new IntIdFake(12)];

    [Test]
    public void TestThrow_WhenUsingWithoutScope_ThenThrowException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<Exception>().And.Message.EqualTo("exception"),
            () => Validator<Exception>.Throw(new Exception("exception"))
        );
        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>().And.Message.EqualTo("Invalid operation"),
            () => Validator<Exception>.Throw(new InvalidOperationException("Invalid operation"))
        );
    }

    [Test]
    public void TestThrow_WhenUsingWithinValidateScope_ThenDontThrowException()
    {
        Assert.DoesNotThrow(
            () =>
                Validator<Exception>.ValidateMany<object>(() =>
                {
                    Validator<Exception>.Throw(new InvalidOperationException("exception"));
                    return new object();
                })
        );
    }

    [Test]
    public void TestThrow_WhenGivingNullInsteadOfException_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exception"),
            () => Validator<Exception>.Throw<Exception>(null!)
        );
    }

    [Test]
    public void TestThrow_WhenGivingNullInsteadOfExceptionWithinValidationScope_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exception"),
            () =>
                Validator<Exception>.ValidateMany(() =>
                {
                    Validator<Exception>.Throw<Exception>(null!);

                    return new object();
                })
        );
    }

    [Test]
    public void TestValidateMany_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationAction"),
            () => Validator<Exception>.ValidateMany(null!)
        );
    }

    [Test]
    public void TestValidateMany_WhenThrowingExceptionThatIsNotInBaseOfValidator_ThenThrowException()
    {
        void throwAction() =>
            Validator<InvalidOperationException>.Throw(new InvalidOperationException());

        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>(),
            () => Validator<ArgumentNullException>.ValidateMany(throwAction)
        );
    }

    [Test]
    public void TestValidateManyWithValue_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationFunc"),
            () => Validator<Exception>.ValidateMany<object>(null!)
        );
    }

    [Test]
    public void TestValidateManyWithValue_WhenThrowingExceptionThatIsNotInBaseOfValidator_ThenThrowException()
    {
        string throwFunction()
        {
            Validator<InvalidOperationException>.Throw(new InvalidOperationException());

            return "result";
        }

        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>(),
            () => Validator<ArgumentNullException>.ValidateMany(throwFunction)
        );
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public void TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult<Exception> result = Validator<Exception>.ValidateMany(() =>
        {
            exceptions.ToList().ForEach(exception => Validator<Exception>.Throw(exception));
        });

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(exceptions));
            Assert.That(result, Is.EqualTo(Result.Failure));
        });
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public void TestValidateManyWithValue_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult<object, Exception> result = Validator<Exception>.ValidateMany(() =>
        {
            exceptions.ToList().ForEach(exception => Validator<Exception>.Throw(exception));

            return new object();
        });

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(exceptions!));
            Assert.That(result, Is.EqualTo(Result.Failure));
            Assert.That(result.Value, Is.EqualTo(null));
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public void TestValidateMany_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSuccessfull(
        object value
    )
    {
        ValidationResult<Exception> result = Validator<Exception>.ValidateMany(() => { });

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(Result.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public void TestValidateManyWithValue_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSuccessfull(
        object value
    )
    {
        ValidationResult<object, Exception> result = Validator<Exception>.ValidateMany(() => value);

        Assert.Multiple(() =>
        {
            Assert.That(value, Is.EqualTo(result.Value));
            Assert.That(result, Is.EqualTo(Result.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }

    [Test]
    public void TestValidateManyAsync_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationFunc"),
            async () => await Validator<Exception>.ValidateManyAsync(null!)
        );
    }

    [Test]
    public void TestValidateManyAsync_WhenThrowingExceptionThatIsNotInBaseOfValidator_ThenThrowException()
    {
        Task throwAction() =>
            Task.Run(
                () => Validator<InvalidOperationException>.Throw(new InvalidOperationException())
            );

        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>(),
            async () => await Validator<ArgumentNullException>.ValidateManyAsync(throwAction)
        );
    }

    [Test]
    public void TestValidateManyAsyncWithValue_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationFunc"),
            async () => await Validator<Exception>.ValidateManyAsync<object>(null!)
        );
    }

    [Test]
    public void TestValidateManyAsyncWithValue_WhenThrowingExceptionThatIsNotInBaseOfValidator_ThenThrowException()
    {
        Task<string> throwFunction()
        {
            Validator<InvalidOperationException>.Throw(new InvalidOperationException());

            return Task.FromResult("result");
        }

        _ = Assert.ThrowsAsync(
            Is.InstanceOf<InvalidOperationException>(),
            async () => await Validator<ArgumentNullException>.ValidateManyAsync(throwFunction)
        );
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public async Task TestValidateManyAsync_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult<Exception> result = await Validator<Exception>.ValidateManyAsync(
            () =>
                Task.Run(() =>
                {
                    exceptions.ToList().ForEach(exception => Validator<Exception>.Throw(exception));
                })
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(exceptions));
            Assert.That(result, Is.EqualTo(Result.Failure));
        });
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public async Task TestValidateManyAsyncWithValue_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult<object, Exception> result = await Validator<Exception>.ValidateManyAsync(
            () =>
                Task.Run(() =>
                {
                    exceptions.ToList().ForEach(exception => Validator<Exception>.Throw(exception));

                    return new object();
                })
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(exceptions));
            Assert.That(result, Is.EqualTo(Result.Failure));
            Assert.That(result.Value, Is.EqualTo(null));
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public async Task TestValidateManyAsync_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSucessfull(
        object value
    )
    {
        ValidationResult<Exception> result = await Validator<Exception>.ValidateManyAsync(
            () => Task.CompletedTask
        );

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(Result.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public async Task TestValidateManyAsyncWithValue_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSucessfull(
        object value
    )
    {
        ValidationResult<object, Exception> result = await Validator<Exception>.ValidateManyAsync(
            () => Task.FromResult(value)
        );

        Assert.Multiple(() =>
        {
            Assert.That(value, Is.EqualTo(result.Value));
            Assert.That(result, Is.EqualTo(Result.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }
}
