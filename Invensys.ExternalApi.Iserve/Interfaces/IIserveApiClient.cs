using Invensys.ExternalApi.Common.Authentication.Models.Request;

namespace Invensys.ExternalApi.Iserve.Interfaces;

public interface IIserveApiClient
{
   Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url);
}
