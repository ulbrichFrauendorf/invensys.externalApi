using Invensys.ExternalApi.Common.Authentication.Models.Request;

namespace Invensys.ExternalApi.Iserve.Interfaces;

public interface IIserveApiClient
{
   Task<TRequest> GetAsync<TRequest>(JwtAccessTokenRequest accessTokenRequest, string url);
   Task<List<TRequest>> GetListAsync<TRequest>(JwtAccessTokenRequest accessTokenRequest, string url);
   Task<TResponse> PostAsync<TResponse, TRequest>(JwtAccessTokenRequest accessTokenRequest, string url, TRequest requestBody);
}
