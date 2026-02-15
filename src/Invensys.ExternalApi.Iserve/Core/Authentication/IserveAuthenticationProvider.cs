using System.Net.Http.Json;
using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Authentication.Models.Result;
using Invensys.ExternalApi.Common.Authentication.Validators;
using Invensys.ExternalApi.Common.Http.Extensions;
using Invensys.ExternalApi.Iserve.Core.Authentication.Models.Response;
using Invensys.ExternalApi.Iserve.Core.Configuration;
using Invensys.ExternalApi.Iserve.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Invensys.ExternalApi.Iserve.Core.Authentication;

public class IserveAuthenticationProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : AuthenticationProvider<object[]>(httpClientFactory), IIserveAuthenticationProvider
{
   protected override async Task<AuthenticationResult<object[]>> Authenticate(AccessTokenRequest accessTokenRequest)
   {
      var jwtAccessTokenRequest = AccessTokenRequestValidator.Validate<ClientCredentialsTokenRequest>(accessTokenRequest);

      var invensysConfig = configuration.GetSection(nameof(IserveConfig)).Get<IserveConfig>();

      Guard.Against.Null(invensysConfig, nameof(invensysConfig));

      var response = await _httpClient.PostAsync(invensysConfig.AuthorizationUrl, jwtAccessTokenRequest.ToFormData());

      var invensysAuthTokenResponse = await response.Content.ReadFromJsonAsync<IserveAuthTokenResponse>();

      Guard.Against.Null(invensysAuthTokenResponse, nameof(invensysAuthTokenResponse));

      Guard.Against.NullOrEmpty(invensysAuthTokenResponse.AccessToken, nameof(invensysAuthTokenResponse.AccessToken));

      var result = new AuthenticationResult<object[]>();

      result.SetAccessToken(invensysAuthTokenResponse.AccessToken, invensysAuthTokenResponse.ExpiresIn);

      return result;
   }
}
