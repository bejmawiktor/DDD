using Utils.Functional;

namespace DDD.Domain.Validation.AspNetCore;

/// <summary>
/// An <see cref="Error"/> that carries additional key/value data next to its
/// message. <see cref="ErrorExtensions.ToProblemDetails{TError}"/> copies that data
/// into the extension members of the
/// <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> returned to the client.
/// </summary>
/// <param name="message">The error message. Must not be <see langword="null"/> or empty.</param>
/// <param name="extensions">
/// The additional members describing the error. Must not be <see langword="null"/>,
/// but may be empty.
/// </param>
/// <exception cref="ArgumentNullException">
/// <paramref name="message"/> or <paramref name="extensions"/> is <see langword="null"/>.
/// </exception>
/// <exception cref="ArgumentException"><paramref name="message"/> is empty.</exception>
public class ExtendedError(string message, IDictionary<string, object?> extensions) : Error(message)
{
    /// <summary>
    /// Initializes a new error with the given message and no extensions.
    /// <see cref="Extensions"/> can be filled in afterwards.
    /// </summary>
    /// <param name="message">The error message. Must not be <see langword="null"/> or empty.</param>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="message"/> is empty.</exception>
    public ExtendedError(string message)
        : this(message, new Dictionary<string, object?>()) { }

    /// <summary>
    /// Gets the additional members describing the error.
    /// </summary>
    public IDictionary<string, object?> Extensions { get; } =
        extensions ?? throw new ArgumentNullException(nameof(extensions));
}
