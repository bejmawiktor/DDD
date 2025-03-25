using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utils.Functional;
using Utils.Validation;

namespace DDD.Domain.Validation.AspNetCore;

public static class ErrorExtensions
{
    public static ProblemDetails ToProblemDetails<TError>(
        this TError error,
        HttpContext? httpContext = null
    )
        where TError : IError
    {
        return ErrorExtensions.CreateProblemDetails((dynamic)error, httpContext);
    }

    private static ProblemDetails CreateProblemDetails(IError error, HttpContext? httpContext)
    {
        ProblemDetails problemDetails =
            new()
            {
                Instance = httpContext?.Request.Path,
                Detail = error.Message,
                Status = (int)HttpStatusCode.BadRequest,
            };

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails<TAggregateError, TError>(
        this IAggregateError<TAggregateError, TError> error,
        HttpContext? httpContext
    )
        where TAggregateError : IAggregateError<TAggregateError, TError>
        where TError : IError
    {
        IEnumerable<KeyValuePair<string, string[]>> validationProblemDetailsErrors = error
            .Errors.Select(error =>
                error is ValidationError validationError
                    ? (new { FieldName = validationError.FieldName ?? "", validationError.Message })
                    : (new { FieldName = "", error.Message })
            )
            .GroupBy(
                error => error.FieldName,
                error => error.Message,
                (fieldName, messages) =>
                    new KeyValuePair<string, string[]>(fieldName, messages.ToArray())
            );

        if (error.Errors.OfType<NotFoundError>().Count() == 1 && error.Errors.Count() == 1)
        {
            ProblemDetails problemDetails =
                new()
                {
                    Detail = error.Message,
                    Status = (int)HttpStatusCode.NotFound,
                    Instance = httpContext?.Request.Path,
                };
            problemDetails.Extensions["traceId"] =
                Activity.Current?.Id ?? httpContext?.TraceIdentifier;

            return problemDetails;
        }

        ValidationProblemDetails validationProblemDetails =
            new(new Dictionary<string, string[]>(validationProblemDetailsErrors))
            {
                Detail = error.Message,
                Status = (int)HttpStatusCode.BadRequest,
                Instance = httpContext?.Request.Path,
            };
        validationProblemDetails.Extensions["traceId"] =
            Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return validationProblemDetails;
    }
}
