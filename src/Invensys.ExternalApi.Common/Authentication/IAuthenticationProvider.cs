using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Authentication.Models.Result;

namespace Invensys.ExternalApi.Common.Authentication;

public interface IAuthenticationProvider
{
   /// <summary>
   /// Gets the access token.
   /// </summary>
   /// <param name="tokenRequest">The access token request.</param>
   /// <param name="forceRefresh">If set to <c>true</c>, forces a refresh of the token.</param>
   /// <returns>The access token.</returns>
   Task<string> GetAccessToken(AccessTokenRequest tokenRequest, bool forceRefresh = false);
}

/// <summary>
/// Interface representing an authentication provider.
/// </summary>
/// <typeparam name="T">The type of the authentication result data.</typeparam>
public interface IAuthenticationProvider<T> : IAuthenticationProvider
{

   /// <summary>
   /// Gets the authentication result.
   /// </summary>
   /// <param name="accessTokenRequest">The access token request.</param>
   /// <param name="forceRefresh">If set to <c>true</c>, forces a refresh of the token.</param>
   /// <returns>The authentication result.</returns>
   Task<AuthenticationResult<T>> GetAuthenticationResult(AccessTokenRequest accessTokenRequest, bool forceRefresh = false);
}
