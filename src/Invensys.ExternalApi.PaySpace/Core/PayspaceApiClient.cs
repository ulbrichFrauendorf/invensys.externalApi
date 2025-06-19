using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Http;
using Invensys.ExternalApi.Common.Http.Models.Enums;
using Invensys.ExternalApi.Odata;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core;

public class PaySpaceApiClient(
   IHttpClientFactory httpClientFactory,
   IPaySpaceAuthenticationProvider paySpaceAuthenticationProvider
)
   : ExternalApiClient(
      httpClientFactory,
      paySpaceAuthenticationProvider,
      PaySpaceHttpClient.PaySpaceRateLimitedApi,
      HeaderType.Authorization,
      TokenAppendType.Header
   ),
      IPaySpaceApiClient
{
   public async Task<T> GetAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url)
   {
      return await SendRequestWithAuthRetry<T>(accessTokenRequest, async () => await _httpClient.GetAsync(url));
   }

   public async Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url)
   {
      return await GetAllPagesWithAuthRetry<T>(accessTokenRequest, url);
   }

   public async Task<List<T>> GetListAsyncWithListFilter<T>(
      JwtAccessTokenRequest accessTokenRequest,
      string url,
      string filterKey,
      IEnumerable<string?> filterList
   )
   {
      var queryList = new List<T>();
      var maxQueryElements = 100;
      var startIndex = 0;
      var filterQueryCount = 0;

      do
      {
         var partialFilters = filterList.Skip(startIndex).Take(maxQueryElements);

         filterQueryCount += partialFilters.Count();

         var filterListString = string.Join("', '", partialFilters
            .Select(s => s == null ? string.Empty : Uri.EscapeDataString(stringToEscape: s))
            .Where(x => x != null));
         
         var partialFilterString = Uri.EscapeDataString(stringToEscape: filterListString);

         var filter = $"$filter={filterKey} in ('{filterListString}')";

         var filteredUrl = url.Contains('?') ? url + "&" + filter : url + "?" + filter;

         queryList.AddRange(await GetListAsync<T>(accessTokenRequest, filteredUrl));

         startIndex += maxQueryElements;
      } while (filterQueryCount < filterList.Count());

      return queryList;
   }

   private async Task<List<T>> GetAllPagesWithAuthRetry<T>(JwtAccessTokenRequest accessTokenRequest, string initialUrl)
   {
      var returnList = new List<T>();

      var nextUrl = initialUrl;

      while (nextUrl != null)
      {
         var serializedResponse = await SendRequestWithAuthRetry<ListResponse<T>>(
            accessTokenRequest,
            async () => await _httpClient.GetAsync(nextUrl)
         );

         nextUrl = serializedResponse?.OdataNextLink?.ToString();

         if (serializedResponse != null)
            returnList.AddRange(serializedResponse.Value);
      }

      return returnList;
   }
}
