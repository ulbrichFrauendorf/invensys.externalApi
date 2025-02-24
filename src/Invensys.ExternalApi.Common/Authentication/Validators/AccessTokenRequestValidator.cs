using Ardalis.GuardClauses;
using Invensys.Api.Common.Authentication.Exceptions;
using Invensys.Api.Common.Authentication.Models.Request;

namespace Invensys.Api.Common.Authentication.Validators
{
    /// <summary>
    /// Provides validation methods for access token requests.
    /// </summary>
    public static class AccessTokenRequestValidator
    {
        /// <summary>
        /// Validates the specified access token request.
        /// </summary>
        /// <typeparam name="T">The type of the access token request.</typeparam>
        /// <param name="request">The access token request to validate.</param>
        /// <returns>The validated access token request.</returns>
        /// <exception cref="AuthenticationConfigurationException">Thrown when the request is not of the expected type.</exception>
        public static T Validate<T>(AccessTokenRequest request) where T : AccessTokenRequest
        {
            Guard.Against.Null(request, nameof(request));

            return request is T validRequest
               ? validRequest
               : throw new AuthenticationConfigurationException($"{typeof(T).Name} object expected");
        }
    }
}
