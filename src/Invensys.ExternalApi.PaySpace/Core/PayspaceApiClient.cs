using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ardalis.GuardClauses;
using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.Common.Http;
using Invensys.ExternalApi.Common.Http.Models.Enums;
using Invensys.ExternalApi.Odata;
using Invensys.ExternalApi.PaySpace.Core.Configuration;
using Invensys.ExternalApi.PaySpace.Core.Exceptions;
using Invensys.ExternalApi.PaySpace.Entities.Errors;
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
   private static readonly JsonSerializerOptions WriteOptions =
      new(JsonSerializerDefaults.Web)
      {
         DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
         Converters = { new PaySpaceDateConverter() }
      };

   private sealed class PaySpaceDateConverter : JsonConverter<DateOnly>
   {
      public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
         var value = reader.GetString();

         if (
            value != null
            && value.Length >= 10
            && DateOnly.TryParseExact(
               value[..10],
               "yyyy-MM-dd",
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out var date
            )
            && (
               value.Length == 10
               || DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
            )
         )
         {
            return date;
         }

         throw new JsonException("Expected a PaySpace date or timestamp.");
      }

      public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
      {
         writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
      }
   }

   public async Task<T> PostAsync<T>(
      JwtAccessTokenRequest accessTokenRequest,
      string url,
      object body,
      CancellationToken cancellationToken = default
   )
   {
      Guard.Against.Null(body, nameof(body));

      var payload = JsonSerializer.SerializeToElement(body, body.GetType(), WriteOptions);

      Guard.Against.InvalidInput(
         payload,
         nameof(body),
         jsonElement => jsonElement.ValueKind == JsonValueKind.Object,
         "The record must be a JSON object."
      );

      for (var attempt = 0; attempt < 2; attempt++)
      {
         var token = await _authenticationProvider
            .GetAccessToken(accessTokenRequest, attempt > 0)
            .WaitAsync(cancellationToken);

         using var request = new HttpRequestMessage(HttpMethod.Post, url);

         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

         request.Headers.UserAgent.ParseAdd("payspace.com");

         request.Content = JsonContent.Create(payload);

         using var response = await _httpClient.SendAsync(request, cancellationToken);

         if (response.StatusCode == HttpStatusCode.Unauthorized && attempt == 0)
         {
            continue;
         }

         var content = await response.Content.ReadAsStringAsync(cancellationToken);

         JsonElement result;

         try
         {
            result = JsonSerializer.Deserialize<JsonElement>(content);
         }
         catch (JsonException) when (!response.IsSuccessStatusCode)
         {
            response.EnsureSuccessStatusCode();

            throw;
         }

         if (result.ValueKind == JsonValueKind.Object)
         {
            // Only an explicit failure flag identifies the vendor's validation envelope.
            var failure = result
               .EnumerateObject()
               .Any(property =>
                  property.Name.Equals("Success", StringComparison.OrdinalIgnoreCase)
                  && property.Value.ValueKind == JsonValueKind.False
               );

            if (failure)
            {
               var validation = result.Deserialize<PaySpaceValidationResponse>(WriteOptions)!;

               throw new PaySpaceValidationException(response.StatusCode, validation);
            }
         }

         response.EnsureSuccessStatusCode();

         return result.Deserialize<T>(WriteOptions)
            ?? throw new InvalidOperationException("PaySpace returned an empty response.");
      }

      throw new InvalidOperationException("The PaySpace request did not complete.");
   }

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

         var filterListString = string.Join(
            "', '",
            partialFilters
               .Select(s => s == null ? string.Empty : Uri.EscapeDataString(stringToEscape: s))
               .Where(x => x != null)
         );

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
