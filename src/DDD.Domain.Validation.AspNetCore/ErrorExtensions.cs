using System.Diagnostics;
using System.Net;
using DDD.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Domain.Validation.AspNetCore;

public static class ErrorExtensions
{
    public static ProblemDetails ToProblemDetails(
        this IError error,
        HttpContext? httpContext = null
    )
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

    public static ProblemDetails ToProblemDetails<TException>(
        this IError<TException> error,
        HttpContext? httpContext
    )
        where TException : Exception
    {
        IEnumerable<KeyValuePair<string, string[]>> validationProblemDetailsErrors = error
            .Reasons.Select(exception =>
                exception is ValidationException validationException
                    ? (
                        new
                        {
                            FieldName = validationException.FieldName ?? "",
                            validationException.Message,
                        }
                    )
                    : (new { FieldName = "", exception.Message })
            )
            .GroupBy(
                error => error.FieldName,
                error => error.Message,
                (fieldName, messages) =>
                    new KeyValuePair<string, string[]>(fieldName, messages.ToArray())
            );

        if (
            error.Reasons.OfType<NotFoundException>().Count() == error.Reasons.Count()
            && error.Reasons.Count() == 1
        )
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
