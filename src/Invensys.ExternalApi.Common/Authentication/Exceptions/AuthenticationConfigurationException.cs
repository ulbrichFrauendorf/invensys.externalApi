namespace Invensys.ExternalApi.Common.Authentication.Exceptions;

/// <summary>
/// Exception thrown when there is a configuration error in authentication.
/// </summary>
[Serializable]
public sealed class AuthenticationConfigurationException : Exception
{
   /// <summary>
   /// Initializes a new instance of the <see cref="AuthenticationConfigurationException"/> class.
   /// </summary>
   public AuthenticationConfigurationException() { }

   /// <summary>
   /// Initializes a new instance of the <see cref="AuthenticationConfigurationException"/> class with a specified error message.
   /// </summary>
   /// <param name="message">The message that describes the error.</param>
   public AuthenticationConfigurationException(string? message)
      : base(message) { }

   /// <summary>
   /// Initializes a new instance of the <see cref="AuthenticationConfigurationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
   /// </summary>
   /// <param name="message">The message that describes the error.</param>
   /// <param name="innerException">The exception that is the cause of the current exception.</param>
   public AuthenticationConfigurationException(string? message, Exception? innerException)
      : base(message, innerException) { }
}
