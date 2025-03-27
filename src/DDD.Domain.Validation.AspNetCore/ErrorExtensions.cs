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
        where TError : IError => ErrorExtensions.CreateProblemDetails((dynamic)error, httpContext);

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
        this IAggregateError<TAggregateError, TError> errors,
        HttpContext? httpContext
    )
        where TAggregateError : IAggregateError<TAggregateError, TError>
        where TError : IError
    {
        IEnumerable<KeyValuePair<string, string[]>> validationProblemDetailsErrors =
            ErrorExtensions.ConvertErrorsToKeyValuePair(errors);

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

    private static IEnumerable<KeyValuePair<string, string[]>> ConvertErrorsToKeyValuePair<
        TAggregateError,
        TError
    >(IAggregateError<TAggregateError, TError> errors)
        where TAggregateError : IAggregateError<TAggregateError, TError>
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

    private static ProblemDetails CreateNotFoundProblemDetails<TAggregateError, TError>(
        IAggregateError<TAggregateError, TError> errors,
        HttpContext? httpContext
    )
        where TAggregateError : IAggregateError<TAggregateError, TError>
        where TError : IError
    {
        ProblemDetails problemDetails =
            new()
            {
                Detail = errors.Message,
                Status = (int)HttpStatusCode.NotFound,
                Instance = httpContext?.Request.Path,
            };
        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        return problemDetails;
    }

    private static ValidationProblemDetails CreateBadRequestProblemDetails<TAggregateError, TError>(
        IAggregateError<TAggregateError, TError> errors,
        HttpContext? httpContext,
        IEnumerable<KeyValuePair<string, string[]>> validationProblemDetailsErrors
    )
        where TAggregateError : IAggregateError<TAggregateError, TError>
        where TError : IError
    {
        ValidationProblemDetails validationProblemDetails =
            new(new Dictionary<string, string[]>(validationProblemDetailsErrors))
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
