using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Http;
using Invensys.ExternalApi.Common.Http.Models.Enums;
using Invensys.ExternalApi.IntegraFlow.Core.Configuration;
using Invensys.ExternalApi.IntegraFlow.Interfaces;
using Invensys.ExternalApi.Sage300.Core.Models.Request;

namespace Invensys.ExternalApi.IntegraFlow.Core;

public class IntegraFlowApiClient(
   IHttpClientFactory httpClientFactory,
   IIntegraFlowAuthenticationProvider invensysAuthenticationProvider
)
   : ExternalApiClient(
      httpClientFactory,
      invensysAuthenticationProvider,
      "invensys",
      HeaderType.Authorization,
      TokenAppendType.Header
   ), IIntegraFlowApiClient
{
   public async Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url)
   {
      var response = await SendRequestWithAuthRetry<IntegraFlowResponse<T>>(accessTokenRequest, async () => await _httpClient.GetAsync(url));

      return response.Response;
   }
}
