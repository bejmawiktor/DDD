using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utils.Functional;
using Utils.Validation;

namespace DDD.Domain.Validation.AspNetCore;

/// <summary>
/// Extension methods that translate domain validation errors into ASP.NET Core
/// <see cref="ProblemDetails"/> so they can be returned from HTTP endpoints.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Converts a domain error into an RFC 7807 <see cref="ProblemDetails"/>
    /// payload. Aggregate errors are expanded into field-keyed validation
    /// problems, and a lone <c>NotFoundError</c> yields a 404 response; other
    /// errors map to 400 Bad Request. A <c>traceId</c> is attached when available.
    /// </summary>
    /// <typeparam name="TError">The concrete error type.</typeparam>
    /// <param name="error">The error to convert.</param>
    /// <param name="httpContext">
    /// The current HTTP context, used to populate the request path and trace
    /// identifier. Optional.
    /// </param>
    /// <returns>The problem details describing the error.</returns>
    public static ProblemDetails ToProblemDetails<TError>(
        this TError error,
        HttpContext? httpContext = null
    )
        where TError : IError => ErrorExtensions.CreateProblemDetails((dynamic)error, httpContext);

    private static ProblemDetails CreateProblemDetails(IError error, HttpContext? httpContext)
    {
        ProblemDetails problemDetails = new()
        {
            Instance = httpContext?.Request.Path,
            Detail = error.Message,
            Status = (int)HttpStatusCode.BadRequest,
        };

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails<TError>(
        IAggregateError<TError> errors,
        HttpContext? httpContext
    )
        where TError : IError
    {
        IEnumerable<KeyValuePair<string, string[]>> validationProblemDetailsErrors =
            ErrorExtensions.ConvertErrorsToKeyValuePairs(errors);

        if (errors.Errors.OfType<NotFoundError>().Count() == 1 && errors.Errors.Count() == 1)
        {
            return ErrorExtensions.CreateNotFoundProblemDetails(errors, httpContext);
        }

        return ErrorExtensions.CreateBadRequestProblemDetails(
            errors,
            httpContext,
            validationProblemDetailsErrors
        );
    }

    private static IEnumerable<KeyValuePair<string, string[]>> ConvertErrorsToKeyValuePairs<TError>(
        IAggregateError<TError> errors
    )
        where TError : IError =>
        errors
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

    private static ProblemDetails CreateNotFoundProblemDetails<TError>(
        IAggregateError<TError> errors,
        HttpContext? httpContext
    )
        where TError : IError
    {
        ProblemDetails problemDetails = new()
        {
            Detail = errors.Message,
            Status = (int)HttpStatusCode.NotFound,
            Instance = httpContext?.Request.Path,
        };
        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return problemDetails;
    }

    private static ValidationProblemDetails CreateBadRequestProblemDetails<TError>(
        IAggregateError<TError> errors,
        HttpContext? httpContext,
        IEnumerable<KeyValuePair<string, string[]>> validationProblemDetailsErrors
    )
        where TError : IError
    {
        ValidationProblemDetails validationProblemDetails = new(
            new Dictionary<string, string[]>(validationProblemDetailsErrors)
        )
        {
            Detail = errors.Message,
            Status = (int)HttpStatusCode.BadRequest,
            Instance = httpContext?.Request.Path,
        };
        validationProblemDetails.Extensions["traceId"] =
            Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return validationProblemDetails;
    }
}
