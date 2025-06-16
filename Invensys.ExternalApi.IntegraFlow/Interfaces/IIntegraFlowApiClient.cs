using Invensys.ExternalApi.Common.Authentication.Models.Request;

namespace Invensys.ExternalApi.IntegraFlow.Interfaces;

public interface IIntegraFlowApiClient
{
   Task<List<T>> GetListAsync<T>(JwtAccessTokenRequest accessTokenRequest, string url);
}
