using Ardalis.GuardClauses;
using Invensys.Api.Common.Authentication.Models.Request;
using Invensys.Api.Common.Authentication.Models.Result;

namespace Invensys.Api.Common.Authentication;

/// <summary>
/// Abstract class representing an authentication provider.
/// </summary>
/// <typeparam name="T">The type of the authentication result data.</typeparam>
public abstract class AuthenticationProvider<T>(IHttpClientFactory httpClientFactory) : IAuthenticationProvider<T>
{
   private AuthenticationResult<T> _authenticationResult = new();

   //Standard HttpClient with no custom configuration or handlers used for authentication
   protected readonly HttpClient _httpClient = httpClientFactory.CreateClient();

   /// <inheritdoc />
   public async Task<AuthenticationResult<T>> GetAuthenticationResult(AccessTokenRequest accessTokenRequest, bool forceRefresh = false)
   {
      if (ShouldRefreshToken(forceRefresh))
      {
         _authenticationResult = await Authenticate(accessTokenRequest);
      }

      Guard.Against.Null(_authenticationResult, nameof(_authenticationResult));

      return _authenticationResult;
   }

   /// <inheritdoc />
   public async Task<string> GetAccessToken(AccessTokenRequest tokenRequest, bool forceRefresh = false)
   {
      var authenticationResult = await GetAuthenticationResult(tokenRequest, forceRefresh);

      var accessToken = authenticationResult.AccessToken;

      Guard.Against.NullOrEmpty(accessToken, nameof(accessToken));

      return accessToken;
   }

   private bool ShouldRefreshToken(bool forceRefresh)
   {
      return string.IsNullOrWhiteSpace(_authenticationResult?.AccessToken) ||
             _authenticationResult?.AccessTokenExpiryDateTime.ToUniversalTime() <= DateTime.UtcNow ||
             forceRefresh;
   }

   /// <summary>
   /// Authenticates the specified access token request.
   /// </summary>
   /// <param name="accessTokenRequest">The access token request.</param>
   /// <returns>The authentication result.</returns>
   protected abstract Task<AuthenticationResult<T>> Authenticate(AccessTokenRequest accessTokenRequest);
}
