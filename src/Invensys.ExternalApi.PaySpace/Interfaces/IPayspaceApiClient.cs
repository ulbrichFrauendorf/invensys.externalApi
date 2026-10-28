using Invensys.ExternalApi.Common.Authentication.Models.Request;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceApiClient
{
   /// <summary>Posts an object without transient retries; exposes PaySpace validation envelopes as typed exceptions.</summary>
   /// <typeparam name="T">The response model.</typeparam>
   /// <param name="accessTokenRequest">The PaySpace authentication request.</param>
   /// <param name="url">The resource URL relative to the configured API domain, including the collection prefix.</param>
   /// <param name="body">The request model or a JSON object.</param>
   /// <param name="cancellationToken">Cancels authentication waiting and the HTTP request.</param>
   /// <returns>The deserialized response.</returns>
   /// <exception cref="Core.Exceptions.PaySpaceValidationException">PaySpace returned Success=false.</exception>
   Task<T> PostAsync<T>(
      JwtAccessTokenRequest accessTokenRequest,
      string url,
      object body,
      CancellationToken cancellationToken = default
   );

   Task<T> GetAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url);
   Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url);
   Task<List<T>> GetListAsyncWithListFilter<T>(
      JwtAccessTokenRequest accessTokenRequest,
      string url,
      string filterKey,
      IEnumerable<string> filterList
   );
}
