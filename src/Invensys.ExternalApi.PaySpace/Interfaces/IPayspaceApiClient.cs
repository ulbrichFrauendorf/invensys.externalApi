using Invensys.ExternalApi.Common.Authentication.Models.Request;

namespace Invensys.ExternalApi.PaySpace.Interfaces;

public interface IPaySpaceApiClient
{
    Task<T> GetAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url);
    Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url);
    Task<List<T>> GetListAsyncWithListFilter<T>(
       JwtAccessTokenRequest accessTokenRequest,
       string url,
       string filterKey,
       IEnumerable<string> filterList
    );
}
