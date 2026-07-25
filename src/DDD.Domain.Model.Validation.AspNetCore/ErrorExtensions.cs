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
    /// payload. Aggregate errors are flattened, including nested aggregates, and
    /// expanded into field-keyed validation problems; a lone <c>NotFoundError</c>
    /// yields a 404 response, other errors map to 400 Bad Request.
    /// Every <see cref="ExtendedError"/> involved,
    /// whether given directly or aggregated, contributes its
    /// <see cref="ExtendedError.Extensions"/> as extension members; keys claimed by
    /// more than one aggregated error end up holding an array of the values.
    /// A <c>traceId</c> is attached when available.
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

    private static ProblemDetails CreateProblemDetails(
        ExtendedError error,
        HttpContext? httpContext
    )
    {
        ProblemDetails problemDetails = new()
        {
            Instance = httpContext?.Request.Path,
            Detail = error.Message,
            Status = (int)HttpStatusCode.BadRequest,
        };

        foreach (KeyValuePair<string, object?> extension in error.Extensions)
        {
            problemDetails.Extensions[extension.Key] = extension.Value;
        }

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails<TError>(
        IAggregateError<TError> errors,
        HttpContext? httpContext
    )
        where TError : IError
    {
        IReadOnlyCollection<IError> flattenedErrors =
        [
            .. ErrorExtensions.Flatten(errors.Errors.Cast<IError>()),
        ];

        if (flattenedErrors.OfType<NotFoundError>().Count() == 1 && flattenedErrors.Count == 1)
        {
            return ErrorExtensions.CreateNotFoundProblemDetails(
                errors.Message,
                flattenedErrors,
                httpContext
            );
        }

        return ErrorExtensions.CreateBadRequestProblemDetails(
            errors.Message,
            flattenedErrors,
            httpContext
        );
    }

    /// <summary>
    /// Expands the errors into a flat sequence of leaf errors, descending into every
    /// nested aggregate. Problem details are flat by definition, so an error buried in
    /// a nested aggregate has to be lifted to the top level to be represented at all.
    /// </summary>
    private static IEnumerable<IError> Flatten(IEnumerable<IError> errors) =>
        errors.SelectMany(error =>
            error is IAggregateError<IError> nestedErrors
                ? ErrorExtensions.Flatten(nestedErrors.Errors)
                : [error]
        );

    private static IEnumerable<KeyValuePair<string, string[]>> ConvertErrorsToKeyValuePairs(
        IEnumerable<IError> errors
    ) =>
        errors
            .Select(error =>
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

    private static ProblemDetails CreateNotFoundProblemDetails(
        string message,
        IEnumerable<IError> errors,
        HttpContext? httpContext
    )
    {
        ProblemDetails problemDetails = new()
        {
            Detail = message,
            Status = (int)HttpStatusCode.NotFound,
            Instance = httpContext?.Request.Path,
        };

        ErrorExtensions.AddExtensions(problemDetails, errors);

        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return problemDetails;
    }

    /// <summary>
    /// Copies the extension members of every <see cref="ExtendedError"/> found among
    /// the aggregated errors into the problem details. When more than one error
    /// contributes the same key, the values are collected into an array, in the order
    /// the errors appear.
    /// </summary>
    private static void AddExtensions(ProblemDetails problemDetails, IEnumerable<IError> errors)
    {
        IEnumerable<IGrouping<string, object?>> groupedExtensions = errors
            .OfType<ExtendedError>()
            .SelectMany(error => error.Extensions)
            .GroupBy(extension => extension.Key, extension => extension.Value);

        foreach (IGrouping<string, object?> extension in groupedExtensions)
        {
            object?[] values = [.. extension];

            problemDetails.Extensions[extension.Key] = values.Length == 1 ? values[0] : values;
        }
    }

    private static ValidationProblemDetails CreateBadRequestProblemDetails(
        string message,
        IEnumerable<IError> errors,
        HttpContext? httpContext
    )
    {
        ValidationProblemDetails validationProblemDetails = new(
            new Dictionary<string, string[]>(ErrorExtensions.ConvertErrorsToKeyValuePairs(errors))
        )
        {
            Detail = message,
            Status = (int)HttpStatusCode.BadRequest,
            Instance = httpContext?.Request.Path,
        };

        ErrorExtensions.AddExtensions(validationProblemDetails, errors);

        validationProblemDetails.Extensions["traceId"] =
            Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return validationProblemDetails;
    }
}
