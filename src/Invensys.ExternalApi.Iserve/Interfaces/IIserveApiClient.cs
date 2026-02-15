using Invensys.ExternalApi.Common.Authentication.Models.Request;

namespace Invensys.ExternalApi.Iserve.Interfaces;

public interface IIserveApiClient
{
   Task<TResponse> PostAsync<TResponse, TRequest>(JwtAccessTokenRequest accessTokenRequest, string url, TRequest requestBody);
}
