using Invensys.ExternalApi.Common.Authentication.Models.Request;
using Invensys.ExternalApi.PaySpace.Core.Attributes;
using Invensys.ExternalApi.PaySpace.Interfaces;

namespace Invensys.ExternalApi.PaySpace.Core;

public class PaySpaceApiBase(IPaySpaceApiClient payspaceApiClient)
{
   protected readonly IPaySpaceApiClient _payspaceApiClient = payspaceApiClient;

   protected Task<T> GetAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url) =>
      _payspaceApiClient.GetAsync<T>(accessTokenRequest, PaySpaceApiAttribute.BuildPath(GetType(), url));

   protected Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url) =>
      _payspaceApiClient.GetListAsync<T>(accessTokenRequest, PaySpaceApiAttribute.BuildPath(GetType(), url));

   protected Task<List<T>> GetListAsyncWithListFilter<T>(
      JwtAccessTokenRequest accessTokenRequest,
      string url,
      string filterKey,
      IEnumerable<string> filterList
   ) =>
      _payspaceApiClient.GetListAsyncWithListFilter<T>(
         accessTokenRequest,
         PaySpaceApiAttribute.BuildPath(GetType(), url),
         filterKey,
         filterList
      );

   protected Task<T> PostAsync<T>(
      JwtAccessTokenRequest accessTokenRequest,
      string url,
      object body,
      CancellationToken cancellationToken = default
   ) =>
      _payspaceApiClient.PostAsync<T>(
         accessTokenRequest,
         PaySpaceApiAttribute.BuildPath(GetType(), url),
         body,
         cancellationToken
      );
}
