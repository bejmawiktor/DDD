using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using DDD.Domain.Validation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Utils.Functional;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

[TestFixture]
public class ErrorExtensionsTest
{
    public static IEnumerable<TestCaseData> CreateErrorWithReasonsTestData(string testName)
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

        yield return new TestCaseData(
            "/test",
            new AggregateError<IError>(messageOnlyValidationError),
            new ValidationProblemDetails(
                new Dictionary<string, string[]>() { { "", [messageOnlyValidationError.Message] } }
            )
            {
                Detail = messageOnlyValidationError.Message,
                Instance = "/test",
                Status = (int)HttpStatusCode.BadRequest,
            }
        ).SetName($"{testName}(Single Validation Error)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Field Name Validation Error)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Two Errors)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Multiple Error With FieldName)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Same FieldName Errors)");
        yield return new TestCaseData(
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
                    { secondFieldValidationError.FieldName, [secondFieldValidationError.Message] },
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
        ).SetName($"{testName}(Multi FieldName Errors)");
        yield return new TestCaseData(
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
                        [secondFieldValidationError.Message, secondField2ValidationError.Message]
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
        ).SetName($"{testName}(Multi FieldName Multi Messages Errors)");
        yield return new TestCaseData(
            "/test",
            new AggregateError<IError>("not found", notFoundError),
            new ProblemDetails()
            {
                Detail = "not found",
                Instance = "/test",
                Status = (int)HttpStatusCode.NotFound,
            }
        ).SetName($"{testName}(8)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(Multi NotFound Errors)");
    }

    public static IEnumerable<TestCaseData> CreateErrorTestData(string testName)
    {
        yield return new TestCaseData(
            "/test",
            "my error test",
            new ProblemDetails() { Detail = "my error test", Instance = "/test" }
        ).SetName($"{testName}(1)");
        yield return new TestCaseData(
            "/test2",
            "my error test 2",
            new ProblemDetails() { Detail = "my error test 2", Instance = "/test2" }
        ).SetName($"{testName}(2)");
        yield return new TestCaseData(
            null,
            "my error test 2",
            new ProblemDetails() { Detail = "my error test 2", Instance = null }
        ).SetName($"{testName}(3)");
    }

    [TestCaseSource(
        nameof(CreateErrorWithReasonsTestData),
        new object[]
        {
            nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned),
        }
    )]
    public void TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned(
        string? path,
        AggregateError<IError> error,
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

        ProblemDetails problemDetails = error.ToProblemDetails(
            path is not null ? httpContextMock.Object : null
        );

        Assert.Multiple(() =>
        {
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                Assert.That(
                    validationProblemDetails.Errors,
                    Is.EquivalentTo((expectedProblemDetails as ValidationProblemDetails)!.Errors)
                );
            }

            Assert.That(
                problemDetails.Detail?.Replace("\r\n", "\n"),
                Is.EqualTo(expectedProblemDetails.Detail?.Replace("\r\n", "\n"))
            );
            Assert.That(problemDetails.Status, Is.EqualTo(expectedProblemDetails.Status));
            Assert.That(problemDetails.Instance, Is.EqualTo(expectedProblemDetails.Instance));
            Assert.That(
                problemDetails.Extensions["traceId"],
                Is.EqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null))
            );
        });
    }

    [TestCaseSource(
        nameof(CreateErrorTestData),
        new object[] { nameof(TestToProblemDetails_WhenErrorGiven_ThenProblemDetailsIsReturned) }
    )]
    public void TestToProblemDetails_WhenErrorGiven_ThenProblemDetailsIsReturned(
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

        Assert.Multiple(() =>
        {
            Assert.That(
                validationProblemDetails.Detail?.Replace("\r\n", "\n"),
                Is.EqualTo(expectedProblemDetails.Detail?.Replace("\r\n", "\n"))
            );
            Assert.That(
                validationProblemDetails.Status,
                Is.EqualTo((int)HttpStatusCode.BadRequest)
            );
            Assert.That(
                validationProblemDetails.Instance,
                Is.EqualTo(expectedProblemDetails.Instance)
            );
            Assert.That(
                validationProblemDetails.Extensions["traceId"],
                Is.EqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null))
            );
        });
    }
}
