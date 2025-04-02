using Invensys.ExternalApi.Common.Http;
using Invensys.ExternalApi.Common.Http.Models.Enums;
using Invensys.ExternalApi.Sage300.Core.Models.Request;
using Invensys.ExternalApi.Sage300.Interfaces;

namespace Invensys.ExternalApi.Sage300.Core;

public class Sage300ApiClient(IHttpClientFactory httpClientFactory, ISage300AuthenticationProvider sage300AuthenticationProvider)
   : ExternalApiClient(httpClientFactory, sage300AuthenticationProvider, Sage300HttpClient.Sage300RateLimitedApi, HeaderType.Cookie, TokenAppendType.Header)
   , ISage300ApiClient
{
    public async Task<T> GetAsync<T>(ResourceOwnerPasswordCredentialTokenRequest accessTokenRequest, string url)
    {
        return await SendRequestWithAuthRetry<T>(accessTokenRequest, async () => await _httpClient.PostAsync(url, null));
    }

    public async Task<List<T>> GetListAsync<T>(ResourceOwnerPasswordCredentialTokenRequest accessTokenRequest, string url)
    {
        var response = await SendRequestWithAuthRetry<Sage300Response<T>>(accessTokenRequest, async () => await _httpClient.PostAsync(url, null));

        return response.Data ?? [];
    }
}
