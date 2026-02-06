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
   public async Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url)
   {
      var response = await SendRequestWithAuthRetry<IserveResponse<T>>(accessTokenRequest, async () => await _httpClient.GetAsync(url));

      return response.Response;
   }
}
