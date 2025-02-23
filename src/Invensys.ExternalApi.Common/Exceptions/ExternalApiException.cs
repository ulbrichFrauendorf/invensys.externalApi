namespace Invensys.Api.Common.Exceptions;

/// <summary>
/// Represents an exception that occurs when calling an external API.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExternalApiException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
/// </remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
public class ExternalApiException(string? message, Exception? innerException) : Exception(message, innerException) { }
