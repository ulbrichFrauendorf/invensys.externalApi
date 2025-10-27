using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Exceptions;
using Invensys.ExternalApi.Common.Http.Models.Enums;

namespace Invensys.ExternalApi.Common.Http;

/// <summary>
/// Provides methods to get an authenticated HttpClient.
/// </summary>
public class ExternalApiClient
{
   protected readonly HttpClient _httpClient;
   protected readonly IAuthenticationProvider _authenticationProvider;

   private readonly HeaderType _headerType;
   private readonly TokenAppendType _tokenAppendType;

   protected ExternalApiClient(
      IHttpClientFactory httpClientFactory,
      IAuthenticationProvider authenticationProvider,
      string httpClientName,
      HeaderType headerType,
      TokenAppendType tokenAppendType
   )
   {
      _httpClient = httpClientFactory.CreateClient(httpClientName);
      _authenticationProvider = authenticationProvider;
      _headerType = headerType;
      _tokenAppendType = tokenAppendType;
   }

   protected async Task<T> SendRequestWithAuthRetry<T>(
      AccessTokenRequest accessTokenRequest,
      Func<Task<HttpResponseMessage>> requestFunc
   )
   {
      await AuthenticateHttpClient(accessTokenRequest);

      var response = await requestFunc();

      if (response.StatusCode == HttpStatusCode.Unauthorized)
      {
         // Refresh the token and retry once
         await Task.Delay(1000);
         await AuthenticateHttpClient(accessTokenRequest, true);

         response = await requestFunc();
      }

      try
      {
         response.EnsureSuccessStatusCode();
      }
      catch (Exception ex)
      {
         var res = await response.Content.ReadAsStringAsync();

         throw new ExternalApiException($"API operation unsuccessful, {res}", ex);
      }

      T? content;
      try
      {
          content = await response.Content.ReadFromJsonAsync<T>();
      }
      catch (Exception ex){
         var res = await response.Content.ReadAsStringAsync();

         throw new ExternalApiException($"API operation unsuccessful, {res}", ex);
      }

      Guard.Against.Null(content, nameof(content));

      return content!;
   }

   /// <inheritdoc />
   public async Task AuthenticateHttpClient(AccessTokenRequest accessTokenRequest, bool forceRefresh = false)
   {
      var token = await _authenticationProvider.GetAccessToken(accessTokenRequest, forceRefresh: forceRefresh);

      var tokenString = _headerType == HeaderType.Authorization ? $"bearer {token}" : token;

      switch (_tokenAppendType)
      {
         case TokenAppendType.Query:
            AppendTokenToQuery(tokenString, _headerType.ToString());
            break;
         case TokenAppendType.Header:
            AppendTokenToHeader(tokenString, _headerType.ToString());
            break;
         case TokenAppendType.None:
            break;
         default:
            break;
      }
   }

   private void AppendTokenToQuery(string tokenString, string tokenPrefix)
   {
      Guard.Against.Null(
         _httpClient.BaseAddress,
         nameof(_httpClient.BaseAddress),
         "Please ensure a base address has been configured in the HTTP pipeline."
      );

      var uriBuilder = new UriBuilder(_httpClient.BaseAddress);

      var query = uriBuilder.Query;

      uriBuilder.Query =
         query.Length > 1 ? $"{query[1..]}&{tokenPrefix}={tokenString}" : $"{tokenPrefix}={tokenString}";

      _httpClient.BaseAddress = uriBuilder.Uri;
   }

   private void AppendTokenToHeader(string tokenString, string tokenPrefix)
   {
      if (tokenPrefix.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
      {
         _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            tokenString.Replace("bearer ", "", StringComparison.OrdinalIgnoreCase)
         );
      }
      else
      {
         _httpClient.DefaultRequestHeaders.Remove(tokenPrefix); // Ensure no duplicate
         _httpClient.DefaultRequestHeaders.Add(tokenPrefix, tokenString);
      }
   }
}
