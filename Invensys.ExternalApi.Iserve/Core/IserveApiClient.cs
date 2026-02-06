using System.Collections.Generic;
using System.Net.Http.Json;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Http;
using Invensys.ExternalApi.Common.Http.Models.Enums;
using Invensys.ExternalApi.Iserve.Interfaces;

namespace Invensys.ExternalApi.Iserve.Core;

public class IserveApiClient(
   IHttpClientFactory httpClientFactory,
   IIserveAuthenticationProvider invensysAuthenticationProvider
)
   : ExternalApiClient(
      httpClientFactory,
      invensysAuthenticationProvider,
      "invensys",
      HeaderType.Authorization,
      TokenAppendType.Header
   ), IIserveApiClient
{
   public async Task<List<TRequest>> GetListAsync<TRequest>(JwtAccessTokenRequest accessTokenRequest, string url)
   {
      var response = await SendRequestWithAuthRetry<IserveResponse<List<TRequest>>>(accessTokenRequest, async () => await _httpClient.GetAsync(url));

      return response.Response;
   }

   public async Task<TRequest> GetAsync<TRequest>(JwtAccessTokenRequest accessTokenRequest, string url)
   {
      var response = await SendRequestWithAuthRetry<IserveResponse<TRequest>>(accessTokenRequest, async () => await _httpClient.GetAsync(url));

      return response.Response;
   }

   public async Task<TResponse> PostAsync<TResponse, TRequest>(JwtAccessTokenRequest accessTokenRequest, string url, TRequest requestBody)
   {
      var response = await SendRequestWithAuthRetry<IserveResponse<TResponse>>(accessTokenRequest, async () => await _httpClient.PostAsJsonAsync(url, requestBody));

      return response.Response;
   }
}
