using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using DDD.Domain.Utils;
using DDD.Domain.Validation;
using DDD.Domain.Validation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

[TestFixture]
public class ErrorExtensionsTest
{
    public static IEnumerable<TestCaseData> ErrorWithReasonsTestData
    {
        get
        {
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>([new Exception("my exception")]),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [new Exception("my exception").Message] },
                    }
                )
                {
                    Detail = new Exception("my exception").Message,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(1)"
            );
            yield return new TestCaseData(
                "/test2",
                new ValidationError<Exception>(
                    [new ValidationException("fieldName", "my validation exception")]
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "fieldName",

                            [
                                new ValidationException(
                                    "fieldName",
                                    "my validation exception"
                                ).Message,
                            ]
                        },
                    }
                )
                {
                    Detail = new ValidationException(
                        "fieldName",
                        "my validation exception"
                    ).Message,
                    Instance = "/test2",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(2)"
            );
            yield return new TestCaseData(
                null,
                new ValidationError<Exception>(
                    [new Exception("my exception"), new ArgumentException("my argument exception")]
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",

                            [
                                new Exception("my exception").Message,
                                new ArgumentException("my argument exception").Message,
                            ]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {new Exception("my exception").Message}
                      - {new ArgumentException("my argument exception").Message}
                    """,
                    Instance = null,
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(3)"
            );
            yield return new TestCaseData(
                "/test2",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                        new ValidationException("validation exception without fieldName"),
                        new NotFoundException("not found exception"),
                    ]
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",

                            [
                                new Exception("my exception").Message,
                                new ArgumentException("my argument exception").Message,
                                new ValidationException(
                                    "validation exception without fieldName"
                                ).Message,
                                new NotFoundException("not found exception").Message,
                            ]
                        },
                        {
                            "fieldName",
                            [new ValidationException("fieldName", "validation exception").Message]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {new ValidationException("fieldName", "validation exception").Message}
                      - {new Exception("my exception").Message}
                      - {new ArgumentException("my argument exception").Message}
                      - {new ValidationException("validation exception without fieldName").Message}
                      - {new NotFoundException("not found exception").Message}
                    """,
                    Instance = "/test2",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(4)"
            );
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new ValidationException("fieldName", "validation exception 2"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                    ]
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",

                            [
                                new Exception("my exception").Message,
                                new ArgumentException("my argument exception").Message,
                            ]
                        },
                        {
                            "fieldName",

                            [
                                new ValidationException(
                                    "fieldName",
                                    "validation exception"
                                ).Message,
                                new ValidationException(
                                    "fieldName",
                                    "validation exception 2"
                                ).Message,
                            ]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {new ValidationException("fieldName", "validation exception").Message}
                      - {new ValidationException("fieldName", "validation exception 2").Message}
                      - {new Exception("my exception").Message}
                      - {new ArgumentException("my argument exception").Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(5)"
            );
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new ValidationException("fieldName2", "validation exception 2"),
                        new ValidationException("fieldName", "validation exception 2"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                    ]
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",

                            [
                                new Exception("my exception").Message,
                                new ArgumentException("my argument exception").Message,
                            ]
                        },
                        {
                            "fieldName",

                            [
                                new ValidationException(
                                    "fieldName",
                                    "validation exception"
                                ).Message,
                                new ValidationException(
                                    "fieldName",
                                    "validation exception 2"
                                ).Message,
                            ]
                        },
                        {
                            "fieldName2",

                            [
                                new ValidationException(
                                    "fieldName2",
                                    "validation exception 2"
                                ).Message,
                            ]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {new ValidationException("fieldName", "validation exception").Message}
                      - {new ValidationException("fieldName2", "validation exception 2").Message}
                      - {new ValidationException("fieldName", "validation exception 2").Message}
                      - {new Exception("my exception").Message}
                      - {new ArgumentException("my argument exception").Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(6)"
            );
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new ValidationException("fieldName2", "validation exception 2"),
                        new ValidationException("fieldName", "validation exception 2"),
                        new ValidationException("fieldName2", "validation exception 3"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                    ]
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",

                            [
                                new Exception("my exception").Message,
                                new ArgumentException("my argument exception").Message,
                            ]
                        },
                        {
                            "fieldName",

                            [
                                new ValidationException(
                                    "fieldName",
                                    "validation exception"
                                ).Message,
                                new ValidationException(
                                    "fieldName",
                                    "validation exception 2"
                                ).Message,
                            ]
                        },
                        {
                            "fieldName2",

                            [
                                new ValidationException(
                                    "fieldName2",
                                    "validation exception 2"
                                ).Message,
                                new ValidationException(
                                    "fieldName2",
                                    "validation exception 3"
                                ).Message,
                            ]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {new ValidationException("fieldName", "validation exception").Message}
                      - {new ValidationException("fieldName2", "validation exception 2").Message}
                      - {new ValidationException("fieldName", "validation exception 2").Message}
                      - {new ValidationException("fieldName2", "validation exception 3").Message}
                      - {new Exception("my exception").Message}
                      - {new ArgumentException("my argument exception").Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(7)"
            );
            yield return new TestCaseData(
                "/test",
                new Error<Exception>("not found", [new NotFoundException("not found exception")]),
                new ProblemDetails()
                {
                    Detail = "not found",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.NotFound,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(8)"
            );
            yield return new TestCaseData(
                "/test",
                new Error<Exception>(
                    "not found",
                    [
                        new NotFoundException("not found exception"),
                        new NotFoundException("not found exception 2"),
                    ]
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",

                            [
                                new NotFoundException("not found exception").Message,
                                new NotFoundException("not found exception 2").Message,
                            ]
                        },
                    }
                )
                {
                    Detail = "not found",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned)}(9)"
            );
        }
    }

    public static IEnumerable<TestCaseData> ErrorTestData
    {
        get
        {
            yield return new TestCaseData(
                "/test",
                "my error test",
                new ProblemDetails() { Detail = "my error test", Instance = "/test" }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorGiven_ThenProblemDetailsIsReturned)}(1)"
            );
            yield return new TestCaseData(
                "/test2",
                "my error test 2",
                new ProblemDetails() { Detail = "my error test 2", Instance = "/test2" }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorGiven_ThenProblemDetailsIsReturned)}(2)"
            );
            yield return new TestCaseData(
                null,
                "my error test 2",
                new ProblemDetails() { Detail = "my error test 2", Instance = null }
            ).SetName(
                $"{nameof(TestToProblemDetails_WhenErrorGiven_ThenProblemDetailsIsReturned)}(3)"
            );
        }
    }

    [TestCaseSource(nameof(ErrorWithReasonsTestData))]
    public void TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned(
        string? path,
        Error<Exception> error,
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

            Assert.That(problemDetails.Detail, Is.EqualTo(expectedProblemDetails.Detail));
            Assert.That(problemDetails.Status, Is.EqualTo(expectedProblemDetails.Status));
            Assert.That(problemDetails.Instance, Is.EqualTo(expectedProblemDetails.Instance));
            Assert.That(
                problemDetails.Extensions["traceId"],
                Is.EqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null))
            );
        });
    }

    [TestCaseSource(nameof(ErrorTestData))]
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
            Assert.That(validationProblemDetails.Detail, Is.EqualTo(expectedProblemDetails.Detail));
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
