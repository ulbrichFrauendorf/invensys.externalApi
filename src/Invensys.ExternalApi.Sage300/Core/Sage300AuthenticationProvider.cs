using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Authentication.Models.Result;
using Invensys.ExternalApi.Common.Authentication.Validators;
using Invensys.ExternalApi.Common.Http.Extensions;
using Invensys.ExternalApi.Sage300.Core.Models.Request;
using Invensys.ExternalApi.Sage300.Core.Models.Response;
using Invensys.ExternalApi.Sage300.Interfaces;
using System.Net.Http.Json;

namespace Invensys.ExternalApi.Sage300.Core;
public class Sage300AuthenticationProvider(IHttpClientFactory httpClientFactory)
   : AuthenticationProvider<object[]>(httpClientFactory)
   , ISage300AuthenticationProvider
{
    protected override async Task<AuthenticationResult<object[]>> Authenticate(AccessTokenRequest accessTokenRequest)
    {
        var tokenRequest = AccessTokenRequestValidator.Validate<ResourceOwnerPasswordCredentialTokenRequest>(accessTokenRequest);

        var response = await _httpClient.PostAsync(tokenRequest.AuthorizationUrl, tokenRequest.ToFormData());

        var sage300TokenResponse = await response.Content.ReadFromJsonAsync<Sage300ApiTokenResponse>();

        Guard.Against.Null(sage300TokenResponse, nameof(sage300TokenResponse));

        response.EnsureSuccessStatusCode();

        var cookieToken = string.Empty;

        if (response.Headers.TryGetValues("set-cookie", out var values))
        {
            cookieToken = values.First();
        }

        var authResult = new AuthenticationResult<object[]>();

        authResult.SetAccessToken(cookieToken, sage300TokenResponse.ExpirySeconds);

        return authResult;
    }
}
