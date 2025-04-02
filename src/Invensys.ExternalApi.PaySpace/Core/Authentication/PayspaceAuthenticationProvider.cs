using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Authentication.Models.Result;
using Invensys.ExternalApi.Common.Authentication.Validators;
using Invensys.ExternalApi.Common.Http.Extensions;
using Invensys.ExternalApi.PaySpace.Core.Authentication.Models;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Entities.Company;
using Invensys.ExternalApi.PaySpace.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Invensys.ExternalApi.PaySpace.Core.Authentication;

public class PaySpaceAuthenticationProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
   : AuthenticationProvider<GroupCompany[]>(httpClientFactory),
      IPaySpaceAuthenticationProvider
{
    protected override async Task<AuthenticationResult<GroupCompany[]>> Authenticate(AccessTokenRequest accessTokenRequest)
    {
        var jwtAccessTokenRequest = AccessTokenRequestValidator.Validate<JwtAccessTokenRequest>(accessTokenRequest);

        var paySpaceConfig = configuration.GetSection(nameof(PaySpaceConfig)).Get<PaySpaceConfig>();

        Guard.Against.Null(paySpaceConfig, nameof(paySpaceConfig));

        var response = await _httpClient.PostAsync(paySpaceConfig.AuthorizationUrl, jwtAccessTokenRequest.ToFormData());

        var payspaceTokenResponse = await response.Content.ReadFromJsonAsync<PaySpaceApiTokenResponse>();

        Guard.Against.Null(payspaceTokenResponse, nameof(payspaceTokenResponse));

        Guard.Against.NullOrEmpty(payspaceTokenResponse.AccessToken, nameof(payspaceTokenResponse.AccessToken));

        var authResult = new AuthenticationResult<GroupCompany[]>
        {
            AuthenticationResultData = payspaceTokenResponse.GroupCompanies
        };

        authResult.SetAccessToken(payspaceTokenResponse.AccessToken, payspaceTokenResponse.ExpiresIn);

        return authResult;
    }
}