using System.Diagnostics;
using System.Net;
using DDD.Domain.Validation.AspNetCore;
using DDD.Tests.Unit.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utils.Functional;
using Utils.Validation;
using ErrorCase = (
    string? Path,
    string Error,
    Microsoft.AspNetCore.Mvc.ProblemDetails ExpectedProblemDetails
);
using ErrorWithReasonsCase = (
    string? Path,
    object Error,
    Microsoft.AspNetCore.Mvc.ProblemDetails ExpectedProblemDetails
);

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

public class ErrorExtensionsTest
{
    public static IEnumerable<
        Func<TestDataRow<ErrorWithReasonsCase>>
    > CreateErrorWithReasonsTestData()
    {
        ValidationError messageOnlyValidationError = new("my error");
        ValidationError withFieldValidationError = new("fieldName", "my validation error");
        ValidationError withField2ValidationError = new("fieldName", "my validation error 2");
        ValidationError secondFieldValidationError = new("fieldName2", "my validation error 2");
        ValidationError secondField2ValidationError = new("fieldName2", "my validation error 3");
        Error argumentError = new("my argument error");
        Error simpleError = new("simple error");
        NotFoundError notFoundError = new("not found error");
        NotFoundError secondNotFoundError = new("not found exception 2");

        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(messageOnlyValidationError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [messageOnlyValidationError.Message] },
                    }
                )
                {
                    Detail = messageOnlyValidationError.Message,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Single validation error"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test2",
                new AggregateError<IError>(withFieldValidationError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { withFieldValidationError.FieldName!, [withFieldValidationError.Message] },
                    }
                )
                {
                    Detail = withFieldValidationError.Message,
                    Instance = "/test2",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Field name validation error"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                null,
                new AggregateError<IError>(simpleError, argumentError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = null,
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Two errors"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test2",
                new AggregateError<IError>(
                    withFieldValidationError,
                    simpleError,
                    argumentError,
                    messageOnlyValidationError,
                    notFoundError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",

                            [
                                simpleError.Message,
                                argumentError.Message,
                                messageOnlyValidationError.Message,
                                notFoundError.Message,
                            ]
                        },
                        { withFieldValidationError.FieldName, [withFieldValidationError.Message] },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                      - {messageOnlyValidationError.Message}
                      - {notFoundError.Message}
                    """,
                    Instance = "/test2",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple errors with field name"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldValidationError,
                    withField2ValidationError,
                    simpleError,
                    argumentError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                        {
                            withFieldValidationError.FieldName,
                            [withFieldValidationError.Message, withField2ValidationError.Message]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {withField2ValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Same field name errors"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldValidationError,
                    secondFieldValidationError,
                    withField2ValidationError,
                    simpleError,
                    argumentError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                        {
                            withFieldValidationError.FieldName,
                            [withFieldValidationError.Message, withField2ValidationError.Message]
                        },
                        {
                            secondFieldValidationError.FieldName,
                            [secondFieldValidationError.Message]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {secondFieldValidationError.Message}
                      - {withField2ValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple field name errors"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldValidationError,
                    secondFieldValidationError,
                    withField2ValidationError,
                    secondField2ValidationError,
                    simpleError,
                    argumentError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                        {
                            withFieldValidationError.FieldName,
                            [withFieldValidationError.Message, withField2ValidationError.Message]
                        },
                        {
                            secondFieldValidationError.FieldName,

                            [
                                secondFieldValidationError.Message,
                                secondField2ValidationError.Message,
                            ]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {secondFieldValidationError.Message}
                      - {withField2ValidationError.Message}
                      - {secondField2ValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple field name errors with multiple messages"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("not found", notFoundError),
                new ProblemDetails()
                {
                    Detail = "not found",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.NotFound,
                }
            ),
            "Single not found error"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("not found", notFoundError, secondNotFoundError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [notFoundError.Message, secondNotFoundError.Message] },
                    }
                )
                {
                    Detail = "not found",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple not found errors"
        );
    }

    public static IEnumerable<Func<TestDataRow<ErrorCase>>> CreateErrorTestData()
    {
        yield return TestCase.Of<ErrorCase>(
            (
                "/test",
                "my error test",
                new ProblemDetails() { Detail = "my error test", Instance = "/test" }
            ),
            "Error with path"
        );
        yield return TestCase.Of<ErrorCase>(
            (
                "/test2",
                "my error test 2",
                new ProblemDetails() { Detail = "my error test 2", Instance = "/test2" }
            ),
            "Error with different path"
        );
        yield return TestCase.Of<ErrorCase>(
            (
                null,
                "my error test 2",
                new ProblemDetails() { Detail = "my error test 2", Instance = null }
            ),
            "Error with null path"
        );
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorWithReasonsTestData))]
    public async Task TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned(
        string? path,
        object errorValue,
        ProblemDetails expectedProblemDetails
    )
    {
        AggregateError<IError> error = (AggregateError<IError>)errorValue;
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());

        ProblemDetails problemDetails = error.ToProblemDetails(
            path is not null ? httpContextMock.Object : null
        );

        using (Assert.Multiple())
        {
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                await Assert
                    .That(validationProblemDetails.Errors)
                    .IsEquivalentTo((expectedProblemDetails as ValidationProblemDetails)!.Errors);
            }

            await Assert
                .That(problemDetails.Detail?.Replace("\r\n", "\n"))
                .IsEqualTo(expectedProblemDetails.Detail?.Replace("\r\n", "\n"));
            await Assert.That(problemDetails.Status).IsEqualTo(expectedProblemDetails.Status);
            await Assert.That(problemDetails.Instance).IsEqualTo(expectedProblemDetails.Instance);
            await Assert
                .That(problemDetails.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorTestData))]
    public async Task TestToProblemDetails_WhenErrorGiven_ThenProblemDetailsIsReturned(
        string? path,
        string error,
        ProblemDetails expectedProblemDetails
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Mock<IError> errorMock = new();
        _ = errorMock.Setup(error => error.Message).Returns(error);

        ProblemDetails validationProblemDetails = errorMock.Object.ToProblemDetails(
            path is not null ? httpContextMock.Object : null
        );

        using (Assert.Multiple())
        {
            await Assert
                .That(validationProblemDetails.Detail?.Replace("\r\n", "\n"))
                .IsEqualTo(expectedProblemDetails.Detail?.Replace("\r\n", "\n"));
            await Assert
                .That(validationProblemDetails.Status)
                .IsEqualTo((int)HttpStatusCode.BadRequest);
            await Assert
                .That(validationProblemDetails.Instance)
                .IsEqualTo(expectedProblemDetails.Instance);
            await Assert
                .That(validationProblemDetails.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }
}
